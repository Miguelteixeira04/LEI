using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ClosedXML.Excel;

class Servidor
{
    private static Dictionary<string, Mutex> fileMutexes = new Dictionary<string, Mutex>();
    private static readonly object mutexDictionaryLock = new object();

    private static HashSet<string> dadosProcessados = new HashSet<string>();
    private static readonly object dadosProcessadosLock = new object();

    private static Dictionary<string, int> totalRegistrosPorArquivo = new Dictionary<string, int>();
    private static readonly object registrosLock = new object();

    // 🔒 Mutex global de escrita para garantir exclusividade entre agregadores
    private static readonly Mutex agregadorEscritaMutex = new Mutex();

    // Mapeamento de WavyID para AgregadorID
    private static Dictionary<string, string> wavyToAgregador = new Dictionary<string, string>()


    {
        { "WavyA1", "Agregador123" },
        { "WavyA2", "Agregador123" },
        { "WavyA3", "Agregador123" },
        { "WavyA4", "Agregador456" },
        { "WavyA5", "Agregador456" },
        { "WavyA6", "Agregador456" }
    };

    // Adicione estas variáveis no início da classe Servidor
    private static Dictionary<string, int> batchCounterPerAgregador = new Dictionary<string, int>();
    private static readonly object batchCounterLock = new object();

    static void Main()
    {
        TcpListener servidor = new TcpListener(IPAddress.Any, 5000);
        servidor.Start();
        Console.WriteLine("Servidor iniciado na porta 5000...\n");

        string basePath = "C:/Users/mikeb/source/repos/SD_PL3_78321_78282_78806/Servidor/Data";
        string[] dataFiles = {
            "Dados_Server-Humidade.xlsx",
            "Dados_Server-Ondulação.xlsx",
            "Dados_Server-Pressão.xlsx",
            "Dados_Server-Temperatura_Agua.xlsx",
            "Dados_Server-Temperatura_Ar.xlsx",
            "Dados_Server-Vento.xlsx"
        };

        foreach (var file in dataFiles)
        {
            string filePath = Path.Combine(basePath, file);
            lock (mutexDictionaryLock)
            {
                fileMutexes[filePath] = new Mutex(false);
            }
            InicializarArquivoExcel(filePath);
            lock (registrosLock)
            {
                totalRegistrosPorArquivo[filePath] = 0;
            }
        }

        while (true)
        {
            TcpClient cliente = servidor.AcceptTcpClient();
            Thread thread = new Thread(HandleClient);
            thread.Start(cliente);
        }
    }

    private static void InicializarArquivoExcel(string caminhoArquivo)
    {
        try
        {
            if (File.Exists(caminhoArquivo))
            {
                File.Delete(caminhoArquivo);
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("Sheet1");
                worksheet.Cell(1, 1).Value = "Wavy";
                worksheet.Cell(1, 2).Value = "Valor";
                worksheet.Cell(1, 3).Value = "Data e hora de envio";
                worksheet.Cell(1, 6).Value = "Lote de Envio";
                worksheet.Cell(1, 8).Value = "Agregador Associado";

                var headerRange = worksheet.Range(1, 1, 1, 8);
                headerRange.Style.Font.Bold = true;

                workbook.SaveAs(caminhoArquivo);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao inicializar arquivo Excel: {ex.Message}");
        }
    }

    private static void HandleClient(object obj)
    {
        TcpClient cliente = (TcpClient)obj;
        NetworkStream stream = cliente.GetStream();
        byte[] buffer = new byte[2048]; // Aumentado para comportar mensagens maiores
        int bytesLidos = stream.Read(buffer, 0, buffer.Length);
        string mensagem = Encoding.UTF8.GetString(buffer, 0, bytesLidos);

        string agregadorID = ExtrairAgregadorID(mensagem);

        if (mensagem.StartsWith("Registo"))
        {
            Console.WriteLine($"(Recebi) Pedido Registo -> {mensagem}\n");
            string resposta = $"Confirmação {mensagem}";
            byte[] respostaBytes = Encoding.UTF8.GetBytes(resposta);
            stream.Write(respostaBytes, 0, respostaBytes.Length);
            Console.WriteLine($"(Envio) Confirmação de registo");
            Console.WriteLine("-----------------------------------\n");
        }
        else if (mensagem.StartsWith("STATUS"))
        {
            string confirmacao = $"Confirmação STATUS {mensagem}";
            byte[] confirmacaoBytes = Encoding.UTF8.GetBytes(confirmacao);
            stream.Write(confirmacaoBytes, 0, confirmacaoBytes.Length);
        }
        else if (mensagem.StartsWith("DADOS"))
        {
            string[] partes = mensagem.Split(';');
            if (partes.Length >= 3)
            {
                string wavyID = partes[1];
                string dados = partes[2];
                agregadorID = ExtrairAgregadorID($"DADOS;{wavyID}");

                // Envolvemos a escrita no mutex global
                bool processado;
                agregadorEscritaMutex.WaitOne();
                try
                {
                    processado = ProcessarDadosUmaVez(wavyID, dados, agregadorID);
                }
                finally
                {
                    agregadorEscritaMutex.ReleaseMutex();
                }

                string confirmacao = $"Confirmação DADOS;{wavyID}";
                byte[] confirmacaoBytes = Encoding.UTF8.GetBytes(confirmacao);
                stream.Write(confirmacaoBytes, 0, confirmacaoBytes.Length);
            }
        }
        else if (mensagem.StartsWith("BATCH_DADOS"))
        {
            string[] partes = mensagem.Split(';');
            if (partes.Length >= 2)
            {
                string wavyID = partes[1];
                agregadorID = ExtrairAgregadorID($"BATCH_DADOS;{wavyID}");

                Console.WriteLine($"(Recebi) Dados em lote vindos do {agregadorID}, capturados na {wavyID}");

                bool algumProcessado = false;

                // Incrementar o contador de lote para este agregador e wavy
                string chaveAgregadorWavy = $"{agregadorID}_{wavyID}";
                lock (batchCounterLock)
                {
                    if (!batchCounterPerAgregador.ContainsKey(chaveAgregadorWavy))
                    {
                        batchCounterPerAgregador[chaveAgregadorWavy] = 1;
                    }
                    else
                    {
                        // Incrementar o número do lote para cada novo lote
                        batchCounterPerAgregador[chaveAgregadorWavy]++;
                    }
                }

                // Acesso exclusivo garantido
                agregadorEscritaMutex.WaitOne();
                try
                {
                    int dadosProcessados = 0;

                    for (int i = 2; i < partes.Length; i++)
                    {
                        string dados = partes[i];
                        if (!string.IsNullOrEmpty(dados))
                        {
                            if (ProcessarDadosUmaVez(wavyID, dados, agregadorID))
                            {
                                algumProcessado = true;
                                dadosProcessados++;
                            }
                        }
                    }

                    Console.WriteLine($"Processados {dadosProcessados} dados do lote");
                }
                finally
                {
                    agregadorEscritaMutex.ReleaseMutex();
                }

                string confirmacao = $"Confirmação BATCH_DADOS;{wavyID}";
                byte[] confirmacaoBytes = Encoding.UTF8.GetBytes(confirmacao);
                stream.Write(confirmacaoBytes, 0, confirmacaoBytes.Length);

                if (algumProcessado)
                {
                    Console.WriteLine("Dados salvos com sucesso");
                    Console.WriteLine($"(Envio) Confirmação de registo dos dados do {agregadorID}");
                    Console.WriteLine("-----------------------------------\n");
                }
            }
        }
        else
        {
            string resposta = $"Confirmação {mensagem}";
            byte[] respostaBytes = Encoding.UTF8.GetBytes(resposta);
            stream.Write(respostaBytes, 0, respostaBytes.Length);
        }

        cliente.Close();
    }

    private static string ExtrairAgregadorID(string mensagem)
    {
        if (mensagem.StartsWith("Registo "))
        {
            return mensagem.Substring("Registo ".Length).Trim();
        }
        else if (mensagem.Contains("Agregador"))
        {
            int index = mensagem.IndexOf("Agregador");
            if (index >= 0)
            {
                string restante = mensagem.Substring(index);
                string[] partes = restante.Split(new char[] { ' ', ';', ':', ',' });
                return partes[0].Trim();
            }
        }

        if (mensagem.StartsWith("BATCH_DADOS") || mensagem.StartsWith("DADOS"))
        {
            string[] partes = mensagem.Split(';');
            if (partes.Length >= 2)
            {
                string wavyID = partes[1].Trim();
                if (wavyToAgregador.ContainsKey(wavyID))
                {
                    return wavyToAgregador[wavyID];
                }
            }
        }

        return "AgregadorDesconhecido";
    }

    private static bool ProcessarDadosUmaVez(string wavyID, string dadosString, string agregadorID)
    {
        // Verificar se os dados estão vazios
        if (string.IsNullOrWhiteSpace(dadosString))
        {
            return false;
        }

        string dadosKey = $"{wavyID}:{dadosString}";

        lock (dadosProcessadosLock)
        {
            if (dadosProcessados.Contains(dadosKey))
            {
                return false;
            }
            dadosProcessados.Add(dadosKey);
        }

        return ProcessarDados(wavyID, dadosString, agregadorID);
    }

    private static bool ProcessarDados(string wavyID, string dadosString, string agregadorID)
    {
        string basePath = "C:/Users/mikeb/source/repos/SD_PL3_78321_78282_78806/Servidor/Data";
        bool sucessoGeral = false;

        try
        {
            Dictionary<string, string> dadosPorTipo = ExtrairDadosPorTipo(dadosString);

            // Obter o número do lote para este agregador
            int numeroLote;
            lock (batchCounterLock)
            {
                // Chave composta para identificar unicamente cada combinação de agregador e wavy
                string chaveAgregadorWavy = $"{agregadorID}_{wavyID}";

                if (!batchCounterPerAgregador.ContainsKey(chaveAgregadorWavy))
                {
                    // Se é a primeira vez, começa com lote 1
                    batchCounterPerAgregador[chaveAgregadorWavy] = 1;
                }

                numeroLote = batchCounterPerAgregador[chaveAgregadorWavy];
            }

            // Se não houver dados para processar, retorne falso
            if (dadosPorTipo.Count == 0)
            {
                return false;
            }

            foreach (var kvp in dadosPorTipo)
            {
                string tipoDado = kvp.Key;
                string valorDado = kvp.Value;

                string nomeArquivo = $"Dados_Server-{tipoDado}.xlsx";
                string caminhoCompleto = Path.Combine(basePath, nomeArquivo);

                Mutex fileMutex;
                lock (mutexDictionaryLock)
                {
                    if (!fileMutexes.ContainsKey(caminhoCompleto))
                    {
                        fileMutexes[caminhoCompleto] = new Mutex(false);
                    }
                    fileMutex = fileMutexes[caminhoCompleto];
                }

                fileMutex.WaitOne();
                try
                {
                    if (!File.Exists(caminhoCompleto))
                    {
                        InicializarArquivoExcel(caminhoCompleto);
                        lock (registrosLock)
                        {
                            totalRegistrosPorArquivo[caminhoCompleto] = 0;
                        }
                    }

                    using (var workbook = new XLWorkbook(caminhoCompleto))
                    {
                        var worksheet = workbook.Worksheet(1);
                        int ultimaLinha = worksheet.LastRowUsed()?.RowNumber() ?? 1;

                        // Adicionar verificação para garantir que estamos na próxima linha
                        if (ultimaLinha < 1) ultimaLinha = 1;

                        worksheet.Cell(ultimaLinha + 1, 1).Value = wavyID;
                        worksheet.Cell(ultimaLinha + 1, 2).Value = valorDado;
                        worksheet.Cell(ultimaLinha + 1, 3).Value = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                        worksheet.Cell(ultimaLinha + 1, 6).Value = $"{numeroLote}º";
                        worksheet.Cell(ultimaLinha + 1, 8).Value = agregadorID;

                        workbook.Save();

                        lock (registrosLock)
                        {
                            if (!totalRegistrosPorArquivo.ContainsKey(caminhoCompleto))
                            {
                                totalRegistrosPorArquivo[caminhoCompleto] = 0;
                            }
                            totalRegistrosPorArquivo[caminhoCompleto]++;
                        }

                        sucessoGeral = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao processar dados de {tipoDado}: {ex.Message}");
                }
                finally
                {
                    fileMutex.ReleaseMutex();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao processar dados: {ex.Message}");
        }

        return sucessoGeral;
    }


    private static Dictionary<string, string> ExtrairDadosPorTipo(string dadosString)
    {
        Dictionary<string, string> resultado = new Dictionary<string, string>();

        // Verifica se a string está vazia
        if (string.IsNullOrWhiteSpace(dadosString))
        {
            return resultado;
        }

        string[] partes = dadosString.Split(',');

        foreach (string parte in partes)
        {
            string parteTrimmed = parte.Trim();
            if (parteTrimmed.Contains(":"))
            {
                string[] keyValue = parteTrimmed.Split(new char[] { ':' }, 2);
                if (keyValue.Length == 2)
                {
                    string chave = keyValue[0].Trim();
                    string valor = keyValue[1].Trim();

                    switch (chave)
                    {
                        case "Humidade":
                            resultado["Humidade"] = valor;
                            break;
                        case "Ondulação":
                            resultado["Ondulação"] = valor;
                            break;
                        case "Pressão":
                            resultado["Pressão"] = valor;
                            break;
                        case "Temperatura_Agua":
                            resultado["Temperatura_Agua"] = valor;
                            break;
                        case "Temperatura_Ar":
                            resultado["Temperatura_Ar"] = valor;
                            break;
                        case "Vento":
                            resultado["Vento"] = valor;
                            break;
                    }
                }
            }
        }

        return resultado;
    }
}
