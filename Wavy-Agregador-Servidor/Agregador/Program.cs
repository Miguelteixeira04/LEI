using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ClosedXML.Excel;
class Agregador
{
    static Dictionary<string, string> estadosWavys = new Dictionary<string, string>();
    static List<string> wavysConectadas = new List<string>();
    static bool primeiraVezMenu = true;
    static int totalWavysRegistradas = 0;
    static HashSet<string> wavysQueEnviaramStatus = new HashSet<string>();

    // escrever no excel : WAVY associadas ao AGREGADOR
    static Dictionary<string, HashSet<string>> wavyDataTypes = new Dictionary<string, HashSet<string>>();
    static Dictionary<string, DateTime> wavyLastSync = new Dictionary<string, DateTime>();

    // Variáveis para armazenar dados das Wavys
    static string[] dados_humidade, dados_ondulacao, dados_pressao, dados_temperatura_Agua, dados_temperatura_Ar, dados_vento;
    static int linhaAtual = 0;
    //fazer pausa

    static int totalDadosRecebidos = 0;
    static bool pausaEnviada = false;
    static ManualResetEvent menuConcluido = new ManualResetEvent(false);

    // Lista circular para armazenar os últimos 12 dados recebidos
    static readonly int MAX_DADOS_ARMAZENADOS = 12;
    static Dictionary<string, Queue<string>> ultimosDadosPorWavy = new Dictionary<string, Queue<string>>();
    static void Main()
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 4000);
        listener.Start();
        Console.WriteLine("Agregador aguardando conexões na porta 4000...\n");
        // Caminho para a pasta 
        string basePath = "C:/Users/mikeb/source/repos/SD_PL3_78321_78282_78806/Agregador/Data";
        // Carregar dados dos arquivos CSV
        dados_humidade = File.ReadAllLines(Path.Combine(basePath, "Dados-Humidade.xlsx"));
        dados_ondulacao = File.ReadAllLines(Path.Combine(basePath, "Dados-Ondulação.xlsx"));
        dados_pressao = File.ReadAllLines(Path.Combine(basePath, "Dados-Pressão.xlsx"));
        dados_temperatura_Agua = File.ReadAllLines(Path.Combine(basePath, "Dados-Temperatura_Agua.xlsx"));
        dados_temperatura_Ar = File.ReadAllLines(Path.Combine(basePath, "Dados-Temperatura_Ar.xlsx"));
        dados_vento = File.ReadAllLines(Path.Combine(basePath, "Dados-Vento.xlsx"));

        // Registro no Servidor
        string agregadorID = "Agregador123";
        string mensagemServidor = $"Registo {agregadorID}";
        string respostaServidor = EnviarParaServidor(mensagemServidor);
        Console.WriteLine($"(Envio) Registo: {agregadorID} Servidor\n");
        Console.WriteLine($"(Recebi) Servidor: {respostaServidor}");
        Console.WriteLine($"-------------------------------------------------\n");
        while (true)
        {
            TcpClient cliente = listener.AcceptTcpClient();

            Thread thread = new Thread(HandleClient);

            thread.Start(cliente);


        }
    }
    static Dictionary<string, NetworkStream> streamsWavys = new Dictionary<string, NetworkStream>();
    private static void HandleClient(object obj)
    {
        TcpClient cliente = (TcpClient)obj;
        NetworkStream stream = cliente.GetStream();
        byte[] buffer = new byte[1024];
        string wavyID = "";
        string agregadorID = "Agregador123";
        try
        {
            while (true)
            {
                int bytesLidos = stream.Read(buffer, 0, buffer.Length);
                if (bytesLidos == 0) break;
                string mensagem = Encoding.UTF8.GetString(buffer, 0, bytesLidos);
                string[] partes = mensagem.Split(';');
                // Trata a mensagem especial "DESATIVADO"
                if (mensagem == "DESATIVADO")
                {
                    if (wavyID != "")
                    {
                        //Aqui
                        Console.WriteLine($"A aguardar a confirmação de desativação...");
                        Console.WriteLine($"(Recebi) {wavyID} foi desativada e removida do agregador.");
                        estadosWavys.Remove(wavyID);
                        streamsWavys.Remove(wavyID);
                        wavysConectadas.Remove(wavyID);
                        // Enviar os últimos dados armazenados para o servidor antes de remover
                        if (ultimosDadosPorWavy.ContainsKey(wavyID))
                        {
                            EnviarUltimosDadosParaServidor(wavyID);
                            ultimosDadosPorWavy.Remove(wavyID);
                        }
                    }
                    break;
                }
                if (partes.Length < 2) continue;
                string tipo = partes[0];
                wavyID = partes[1];
                if (tipo == "Registo")
                {
                    Console.WriteLine($"(Recebi) Pedido Registo -> {wavyID}\n");
                    estadosWavys[wavyID] = "Desconhecido";
                    wavysConectadas.Add(wavyID);
                    totalWavysRegistradas++;
                    streamsWavys[wavyID] = stream;
                    // Inicializar a fila de dados para esta Wavy
                    if (!ultimosDadosPorWavy.ContainsKey(wavyID))
                    {
                        ultimosDadosPorWavy[wavyID] = new Queue<string>();
                    }
                    // Inicializar o conjunto de tipos de dados
                    if (!wavyDataTypes.ContainsKey(wavyID))
                    {
                        wavyDataTypes[wavyID] = new HashSet<string>();
                    }
                    // Inicializar a última sincronização
                    wavyLastSync[wavyID] = DateTime.Now;
                    // Atualizar o arquivo Excel de Wavys associadas
                    UpdateWavyAssociadasExcel();
                    string respostaWavy = $"Confirmação Registo; {wavyID}";
                    byte[] respostaBytes = Encoding.UTF8.GetBytes(respostaWavy);
                    stream.Write(respostaBytes, 0, respostaBytes.Length);
                    Console.WriteLine($"(Envio) Confirmação Registo {wavyID}");
                    Console.WriteLine("--------------------------------------\n");
                }
                else if (tipo == "STATUS")
                {
                    string estadoAtual = partes[2];
                    estadosWavys[wavyID] = estadoAtual;
                    Console.WriteLine($"(Recebi) STATUS {wavyID} : {estadoAtual}\n");
                    wavysQueEnviaramStatus.Add(wavyID);
                    if (primeiraVezMenu && wavysQueEnviaramStatus.Count == totalWavysRegistradas)
                    {
                        ExibirEstados();
                        MostrarMenuAlteracaoEstado();
                        primeiraVezMenu = false;
                    }
                }
                else if (tipo == "DADOS")
                {
                    string dadosReais = mensagem.Substring(mensagem.IndexOf(';', mensagem.IndexOf(';') + 1) + 1).Trim();
                    Console.WriteLine($"(Recebi) {mensagem}");
                    // Armazenar os dados recebidos na fila circular
                    if (!ultimosDadosPorWavy.ContainsKey(wavyID))
                    {
                        ultimosDadosPorWavy[wavyID] = new Queue<string>();
                    }
                    // Adicionar os dados à fila
                    Queue<string> filaWavy = ultimosDadosPorWavy[wavyID];
                    filaWavy.Enqueue(dadosReais);
                    // Manter apenas os últimos MAX_DADOS_ARMAZENADOS dados
                    while (filaWavy.Count > MAX_DADOS_ARMAZENADOS)
                    {
                        filaWavy.Dequeue();
                    }
                    // Incrementar o contador de dados recebidos
                    totalDadosRecebidos++;
                    // Atualizar a última sincronização
                    wavyLastSync[wavyID] = DateTime.Now;
                    // Processar e extrair os tipos de dados
                    string[] dados = dadosReais.Split(',');
                    string[] tiposDados = { "Humidade", "Ondulação", "Pressão", "Temperatura_Agua", "Temperatura_Ar", "Vento" };
                    // Inicializar o conjunto de tipos de dados se não existir
                    if (!wavyDataTypes.ContainsKey(wavyID))
                    {
                        wavyDataTypes[wavyID] = new HashSet<string>();
                    }
                    // Adicionar os tipos de dados ao conjunto
                    for (int i = 0; i < dados.Length && i < tiposDados.Length; i++)
                    {
                        wavyDataTypes[wavyID].Add(tiposDados[i]);
                    }
                    // Processar e escrever os dados nos arquivos correspondentes
                    ProcessarDados(dados, wavyID);
                    // Atualizar o arquivo Excel de Wavys associadas
                    UpdateWavyAssociadasExcel();
                    string confirmacaoDados = $"CONFIRMAÇÃO DADOS;{wavyID}";
                    byte[] confirmacaoBytes = Encoding.UTF8.GetBytes(confirmacaoDados);
                    stream.Write(confirmacaoBytes, 0, confirmacaoBytes.Length);
                    Console.WriteLine($"(Envio) {agregadorID} -> {wavyID}: Confirmação de receção dos dados.");
                    Console.WriteLine($"-------------------------------------------------\n");
                    // Check if we've received 12 data points and need to pause
                    if (totalDadosRecebidos % 12 == 0 && !pausaEnviada)
                    {
                        // Set flag to prevent sending multiple pause commands
                        pausaEnviada = true;
                        // Enviar comando de pausa para todas as Wavys
                        EnviarComandoPausa();
                        // Start a new thread to show the menu and handle state changes
                        Thread menuThread = new Thread(() => {
                            // Display current states of all Wavys
                            ExibirEstados();
                            // Show the menu to change states
                            MostrarMenuAlteracaoEstado();
                            // Signal that the menu interaction is complete
                            menuConcluido.Set();
                            // Reset the pause flag so we can pause again after 10 more data points
                            pausaEnviada = false;
                            // Reset the counter to start counting from 0 again
                            totalDadosRecebidos = 0;
                        });
                        menuThread.Start();
                    }
                }
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Erro de conexão com {wavyID}: {ex.Message}");
        }
        finally
        {
            if (wavyID != "" && streamsWavys.ContainsKey(wavyID))
            {
                streamsWavys.Remove(wavyID);
                estadosWavys.Remove(wavyID);
                wavysConectadas.Remove(wavyID);
            }
            cliente.Close();
        }
    }
    // Método para enviar os últimos dados de uma Wavy específica para o servidor
    private static void EnviarUltimosDadosParaServidor(string wavyID)
    {
        if (estadosWavys.ContainsKey(wavyID) && estadosWavys[wavyID] != "Desativada")
        {
            if (!ultimosDadosPorWavy.ContainsKey(wavyID) || ultimosDadosPorWavy[wavyID].Count == 0)
            {
                Console.WriteLine($"Não há dados armazenados para a {wavyID}");
                return;
            }

            Queue<string> dadosWavy = ultimosDadosPorWavy[wavyID];

            // Construir a mensagem de lote com todos os dados
            StringBuilder mensagemLote = new StringBuilder($"BATCH_DADOS;{wavyID}");
            foreach (string dados in dadosWavy)
            {
                mensagemLote.Append($";{dados}");
            }

            // Enviar APENAS o lote para o servidor, removendo o envio individual
            string resposta = EnviarParaServidor(mensagemLote.ToString());
            Console.WriteLine($"(Envio) Lote de dados da {wavyID} para o servidor");
            Console.WriteLine($"(Recebi) Resposta do servidor: {resposta}");
            Console.WriteLine($"-------------------------------------------------\n");
        }
    }


    // Add this at the class level (outside any method)
    private static readonly Dictionary<string, object> fileLocks = new Dictionary<string, object>();
    private static readonly object dictionaryLock = new object();
    private static void ProcessarDados(string[] dados, string wavyID)
    {
        string agregadorID = "Agregador123";
        string[] tiposDados = { "Humidade", "Ondulação", "Pressão", "Temperatura_Agua", "Temperatura_Ar", "Vento" };
        // Lista para armazenar dados que não foram salvos
        List<string> dadosNaoSalvos = new List<string>();
        for (int i = 0; i < dados.Length; i++)
        {
            string tipoDado = tiposDados[i];
            string caminhoArquivo = $"C:/Users/mikeb/source/repos/SD_PL3_78321_78282_78806/Agregador/Data/Dados-{tipoDado}.xlsx";
            string dadoCompleto = dados[i].Trim();
            string[] partesDado = dadoCompleto.Split(':');
            string valorDado = partesDado.Length > 1 ? partesDado[1].Trim() : "N/A";
            // Obter ou criar um objeto de bloqueio para este arquivo
            object fileLock;
            lock (dictionaryLock)
            {
                if (!fileLocks.ContainsKey(caminhoArquivo))
                {
                    fileLocks[caminhoArquivo] = new object();
                }
                fileLock = fileLocks[caminhoArquivo];
            }
            try
            {
                // Verificar se o arquivo existe antes de tentar abri-lo
                if (!File.Exists(caminhoArquivo))
                {
                    try
                    {
                        // Criar um novo arquivo Excel se não existir
                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.AddWorksheet("Sheet1");
                            workbook.SaveAs(caminhoArquivo);
                        }
                    }
                    catch (Exception)
                    {
                        dadosNaoSalvos.Add($"{tipoDado}: {valorDado}");
                        continue; // Pular para o próximo dado
                    }
                }
                lock (fileLock)
                {
                    try
                    {
                        using (var workbook = new XLWorkbook(caminhoArquivo))
                        {
                            var worksheet = workbook.Worksheet(1);
                            // Encontrar a última linha usada
                            int ultimaLinha = worksheet.LastRowUsed()?.RowNumber() ?? 0;
                            // Se não houver dados, começar da linha 1
                            if (ultimaLinha == 0)
                            {
                                worksheet.Cell(1, 1).Value = $"{wavyID}: {valorDado}";
                            }
                            else
                            {
                                // Verificar se precisamos adicionar uma linha em branco (a cada 10 entradas)
                                // Contamos quantas células não vazias existem
                                int celulasNaoVazias = 0;
                                for (int linha = 1; linha <= ultimaLinha; linha++)
                                {
                                    if (!string.IsNullOrWhiteSpace(worksheet.Cell(linha, 1).GetString()))
                                    {
                                        celulasNaoVazias++;
                                    }
                                }
                                // Se o número de células não vazias é múltiplo de 10, adicionar uma linha em branco
                                if (celulasNaoVazias > 0 && celulasNaoVazias % 12 == 0)
                                {
                                    // A próxima linha será após uma linha em branco
                                    worksheet.Cell(ultimaLinha + 2, 1).Value = $"{wavyID}: {valorDado}";
                                }
                                else
                                {
                                    // Adicionar na próxima linha
                                    worksheet.Cell(ultimaLinha + 1, 1).Value = $"{wavyID}: {valorDado}";
                                }
                            }
                            // Salvar o arquivo
                            try
                            {
                                workbook.Save();
                            }
                            catch (Exception)
                            {
                                dadosNaoSalvos.Add($"{tipoDado}: {valorDado}");
                            }
                        }
                    }
                    catch (Exception)
                    {
                        dadosNaoSalvos.Add($"{tipoDado}: {valorDado}");
                    }
                }
            }
            catch (Exception)
            {
                dadosNaoSalvos.Add($"{tipoDado}: {valorDado}");
            }
        }
        // Exibir mensagens de erro para dados não salvos
        foreach (var dadoNaoSalvo in dadosNaoSalvos)
        {
            Console.WriteLine($"Dado- {dadoNaoSalvo} não foi salvo....");
        }
        // Exibir mensagem de sucesso
        Console.WriteLine($"\nDados da {wavyID} salvos com sucesso!\n");
    }
    private static void ExibirEstados()
    {
        Console.WriteLine("STATUS das Wavys Associadas:");
        foreach (var wavy in estadosWavys)
        {
            Console.WriteLine($"{wavy.Key}: {wavy.Value}");
        }
    }
    private static void EnviarComandoPausa()
    {
        Console.WriteLine("(Envio) Comando de pausa para todas as Wavys associadas...\n");
        foreach (var wavy in streamsWavys)
        {
            string wavyID = wavy.Key;
            NetworkStream stream = wavy.Value;
            // Só envia pausa para Wavys operacionais
            if (estadosWavys.ContainsKey(wavyID) && estadosWavys[wavyID] == "Operacional")
            {
                string comandoPausa = $"PAUSA;{wavyID}";
                byte[] pausaBytes = Encoding.UTF8.GetBytes(comandoPausa);
                stream.Write(pausaBytes, 0, pausaBytes.Length);
                Console.WriteLine($"(Envio) {comandoPausa}");
            }
        }
        // Enviar os últimos dados para o servidor antes de pausar
        EnviarTodosUltimosDadosParaServidor();
    }
    // Método para enviar os últimos dados de todas as Wavys para o servidor
    private static void EnviarTodosUltimosDadosParaServidor()
    {
        Console.WriteLine("\n(Envio) Dados de todas as Wavys associadas para o servidor...\n");
        foreach (var wavy in ultimosDadosPorWavy)
        {
            EnviarUltimosDadosParaServidor(wavy.Key);
        }
    }
    private static void UpdateWavyAssociadasExcel()
    {
        string caminhoArquivo = "C:/Users/mikeb/source/repos/SD_PL3_78321_78282_78806/Agregador/Data/Wavys associadas.xlsx";
        object fileLock;
        // Obter ou criar um objeto de bloqueio para este arquivo
        lock (dictionaryLock)
        {
            if (!fileLocks.ContainsKey(caminhoArquivo))
            {
                fileLocks[caminhoArquivo] = new object();
            }
            fileLock = fileLocks[caminhoArquivo];
        }
        lock (fileLock)
        {
            try
            {
                // Verificar se o arquivo existe, se não, criar
                if (!File.Exists(caminhoArquivo))
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.AddWorksheet("Wavys");
                        // Adicionar cabeçalho
                        worksheet.Cell(1, 1).Value = "WAVY_ID";
                        worksheet.Cell(1, 2).Value = "Status";
                        worksheet.Cell(1, 3).Value = "Data";
                        worksheet.Cell(1, 4).Value = "Last Sync";
                        worksheet.Cell(1, 5).Value = "Data Types";
                        // Formatar cabeçalho
                        var headerRange = worksheet.Range(1, 1, 1, 5);
                        headerRange.Style.Font.Bold = true;
                        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                        workbook.SaveAs(caminhoArquivo);
                    }
                }
                // Abrir o arquivo existente e atualizar os dados
                using (var workbook = new XLWorkbook(caminhoArquivo))
                {
                    var worksheet = workbook.Worksheet(1);
                    // Para cada Wavy, atualizar ou adicionar uma linha
                    int row = 2; // Começar após o cabeçalho
                    foreach (var wavyId in estadosWavys.Keys)
                    {
                        // Verificar se a Wavy já existe no arquivo
                        bool wavyFound = false;
                        int lastRow = worksheet.LastRowUsed()?.RowNumber() ?? 1;
                        for (int i = 2; i <= lastRow; i++)
                        {
                            if (worksheet.Cell(i, 1).GetString() == wavyId)
                            {
                                row = i;
                                wavyFound = true;
                                break;
                            }
                        }
                        // Obter os tipos de dados que a Wavy enviou
                        string dataTypes = "";
                        if (wavyDataTypes.ContainsKey(wavyId))
                        {
                            dataTypes = string.Join(";", wavyDataTypes[wavyId]);
                        }

                        // Obter a última sincronização
                        string lastSync = "";
                        if (wavyLastSync.ContainsKey(wavyId))
                        {
                            lastSync = wavyLastSync[wavyId].ToString("dd/MM/yyyy HH:mm:ss");
                        }

                        // Atualizar ou adicionar a linha
                        worksheet.Cell(row, 1).Value = wavyId;
                        worksheet.Cell(row, 2).Value = estadosWavys[wavyId];

                        // Aqui está a alteração: escrever os Data Types na coluna 3
                        worksheet.Cell(row, 3).Value = dataTypes;

                        worksheet.Cell(row, 4).Value = lastSync;

                        // Verificar se a Wavy já enviou dados
                        if (wavyDataTypes.ContainsKey(wavyId) && wavyDataTypes[wavyId].Count > 0)
                        {
                            worksheet.Cell(row, 5).Value = "Dados encaminhados para o servidor";
                        }
                        else
                        {
                            worksheet.Cell(row, 5).Value = "Dados a serem agregadoos";
                        }

                        // Se a Wavy não foi encontrada, incrementar a linha para a próxima
                        if (!wavyFound)
                        {
                            row++;
                        }
                    }
                    // Ajustar largura das colunas
                    worksheet.Columns().AdjustToContents();
                    // Salvar o arquivo
                    workbook.Save();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar o arquivo 'Wavys associadas.xlsx': {ex.Message}");
            }
        }
    }
    private static void MostrarMenuAlteracaoEstado()
    {
        bool continuarAlterando = true;
        List<string> wavysAlteradas = new List<string>();
        while (continuarAlterando)
        {
            // Lista das Wavys que ainda não foram alteradas
            List<string> wavysRestantes = new List<string>();
            foreach (var wavy in estadosWavys)
            {
                if (!wavysAlteradas.Contains(wavy.Key) && (wavy.Value == "Operacional" || wavy.Value == "Manutenção"))
                {
                    wavysRestantes.Add(wavy.Key);
                }
            }
            // Se o estado atual da Wavy for Desativada, aparecer "(Recebi) {wavyID} foi desativada e removida do agregador."
            if (wavysRestantes.Count == 0)
            {
                Console.WriteLine("\nNão há Wavys restantes para alterar.");
                break;
            }

            // Se é a primeira vez no menu, pergunta se deseja alterar alguma Wavy
            if (wavysAlteradas.Count == 0)
            {
                while (true)
                {
                    Console.WriteLine("--------------------------------------");
                    Console.WriteLine("Deseja alterar o estado de alguma Wavy?");
                    Console.WriteLine("1 - Sim\n2 - Não");
                    string opcao = Console.ReadLine();
                    if (opcao == "1" || opcao == "2")
                    {
                        if (opcao == "2")
                        {
                            Console.WriteLine("\nNenhuma alteração foi feita...\n");
                            // **Envia a mensagem para Wavys cujo estado não foi alterado**
                            foreach (var wavy in estadosWavys)
                            {
                                if (!wavysAlteradas.Contains(wavy.Key))
                                {
                                    string mensagemManterEstado = $"STATUS;{wavy.Key};{wavy.Value}";
                                    if (streamsWavys.ContainsKey(wavy.Key))
                                    {
                                        byte[] mensagemBytes = Encoding.UTF8.GetBytes(mensagemManterEstado);
                                        streamsWavys[wavy.Key].Write(mensagemBytes, 0, mensagemBytes.Length);
                                        Console.WriteLine($"(Envio) {mensagemManterEstado}");
                                    }
                                }
                            }
                            Console.WriteLine($"\n-------------------------------------------------");

                            // Aguarda um tempo para garantir que todas as mensagens foram processadas
                            Thread.Sleep(2000);
                            return;
                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Opção inválida! Por favor, insira um número válido...");
                    }
                }
            }
            // Exibe apenas se houver 2 ou mais opções
            if (wavysRestantes.Count >= 2)
            {
                while (true)
                {
                    Console.WriteLine("\nQual Wavy deseja alterar?");
                    for (int i = 0; i < wavysRestantes.Count; i++)
                    {
                        Console.WriteLine($"{i + 1} - {wavysRestantes[i]}");
                    }
                    string escolha = Console.ReadLine();
                    int wavyIndex = -1;
                    if (int.TryParse(escolha, out wavyIndex) && wavyIndex >= 1 && wavyIndex <= wavysRestantes.Count)
                    {
                        wavyIndex--; // Ajusta para o índice zero-based
                        string wavyEscolhida = wavysRestantes[wavyIndex];
                        Console.WriteLine($"\nPara qual estado deseja alterar {wavyEscolhida}?");
                        Console.WriteLine("a - Operacional\nb - Manutenção\nc - Desativada");
                        string novoEstado;
                        string estadoSelecionado = "";
                        while (true)
                        {
                            novoEstado = Console.ReadLine().ToLower();
                            estadoSelecionado = novoEstado switch
                            {
                                "a" => "Operacional",
                                "b" => "Manutenção",
                                "c" => "Desativada",
                                _ => ""
                            };
                            if (!string.IsNullOrEmpty(estadoSelecionado))
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Opção inválida! Por favor, insira uma letra válida...\n");
                            }
                        }
                        if (!string.IsNullOrEmpty(estadoSelecionado) && estadoSelecionado != estadosWavys[wavyEscolhida])
                        {
                            string confirmacao = $"NOVO_STATUS;{wavyEscolhida};{estadoSelecionado}";
                            if (streamsWavys.ContainsKey(wavyEscolhida))
                            {
                                byte[] confirmacaoBytes = Encoding.UTF8.GetBytes(confirmacao);
                                streamsWavys[wavyEscolhida].Write(confirmacaoBytes, 0, confirmacaoBytes.Length);
                                Console.WriteLine($"\n(Envio) {confirmacao}\n");
                            }
                            estadosWavys[wavyEscolhida] = estadoSelecionado;
                            wavysAlteradas.Add(wavyEscolhida);
                            // Atualizar o arquivo Excel de Wavys associadas
                            UpdateWavyAssociadasExcel();
                        }
                        else
                        {
                            Console.WriteLine("Estado inválido ou igual ao atual!");
                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Opção inválida! Por favor, insira um número válido...\n");
                    }
                }
            }
            else if (wavysRestantes.Count == 1)
            {
                string wavyEscolhida = wavysRestantes[0];
                while (true)
                {
                    Console.WriteLine($"\nPara qual estado deseja alterar {wavyEscolhida}?");
                    Console.WriteLine("a - Operacional\nb - Manutenção\nc - Desativada");
                    string novoEstado;
                    string estadoSelecionado = "";
                    while (true)
                    {
                        novoEstado = Console.ReadLine().ToLower();
                        estadoSelecionado = novoEstado switch
                        {
                            "a" => "Operacional",
                            "b" => "Manutenção",
                            "c" => "Desativada",
                            _ => ""
                        };
                        if (!string.IsNullOrEmpty(estadoSelecionado))
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Opção inválida! Por favor, insira uma letra válida...\n");
                        }
                    }
                    if (!string.IsNullOrEmpty(estadoSelecionado) && estadoSelecionado != estadosWavys[wavyEscolhida])
                    {
                        string confirmacao = $"NOVO_STATUS;{wavyEscolhida};{estadoSelecionado}";
                        if (streamsWavys.ContainsKey(wavyEscolhida))
                        {
                            byte[] confirmacaoBytes = Encoding.UTF8.GetBytes(confirmacao);
                            streamsWavys[wavyEscolhida].Write(confirmacaoBytes, 0, confirmacaoBytes.Length);
                            Console.WriteLine($"\n(Envio) {confirmacao}\n");
                        }
                        estadosWavys[wavyEscolhida] = estadoSelecionado;
                        wavysAlteradas.Add(wavyEscolhida);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Estado inválido ou igual ao atual!");
                    }
                }
            }
            // Se ainda houver Wavys restantes, pergunta se deseja continuar
            if (wavysRestantes.Count > 1)
            {
                while (true)
                {
                    Thread.Sleep(5000);
                    Console.WriteLine("\nDeseja alterar o estado de mais alguma?");
                    Console.WriteLine("1 - Sim\n2 - Não");
                    string continuarEscolha = Console.ReadLine();
                    if (continuarEscolha == "1" || continuarEscolha == "2")
                    {
                        if (continuarEscolha == "2")
                        {
                            // **Envia a mensagem para Wavys que não foram alteradas**
                            foreach (var wavy in estadosWavys)
                            {
                                if (!wavysAlteradas.Contains(wavy.Key))
                                {
                                    string mensagemManterEstado = $"STATUS;{wavy.Key};{wavy.Value}";
                                    if (streamsWavys.ContainsKey(wavy.Key))
                                    {
                                        byte[] mensagemBytes = Encoding.UTF8.GetBytes(mensagemManterEstado);
                                        streamsWavys[wavy.Key].Write(mensagemBytes, 0, mensagemBytes.Length);
                                        Console.WriteLine($"(Envio) {mensagemManterEstado}");
                                    }
                                }
                            }
                            // Adicione uma linha clara de separação e uma pequena pausa
                            Console.WriteLine("\n-------------------------------------------------");
                            Thread.Sleep(2000); // Dá tempo para que todas as Wavys processem as mensagens
                            return;
                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Opção inválida! Por favor, insira uma opção válida...\n");
                    }
                }
            }
        }
        // Reset the data counter after menu interaction
        totalDadosRecebidos = 0;
        // Signal the completion of state updates
        Console.WriteLine("\nAtualização de estados concluída. Todas as Wavys foram notificadas.");

        // Reset the pause flag only after all Wavys have been notified
        pausaEnviada = false;
        Console.WriteLine($"--------------------------------------\n");
    }



    private static string EnviarParaServidor(string mensagem)
    {
        try
        {
            using (TcpClient servidor = new TcpClient("127.0.0.1", 5000))
            {
                NetworkStream serverStream = servidor.GetStream();
                byte[] dadosServidor = Encoding.UTF8.GetBytes(mensagem);
                serverStream.Write(dadosServidor, 0, dadosServidor.Length);
                byte[] buffer = new byte[1024];
                int respostaLida = serverStream.Read(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer, 0, respostaLida);
            }
        }
        catch (Exception ex)
        {
            return $"Erro ao conectar ao Servidor: {ex.Message}";
        }
    }
}

