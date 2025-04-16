using System;
using System.Collections.Generic;
using System.IO;
using ClosedXML.Excel; // Para usar ClosedXML
using System.Net.Sockets;
using System.Text;
using System.Threading;
class Wavy
{
    static string estadoAtual = "Operacional";
    static bool aguardandoConfirmacao = false;
    static ManualResetEvent statusAtualizado = new ManualResetEvent(false); // Novo sinalizador
    static NetworkStream stream;
    static TcpClient cliente;
    // Ficheiros (agora listados como listas de strings)
    static List<string> humidade, ondulacao, pressao, temperaturaAgua, temperaturaAr, vento;
    // Linha atual
    static int linhaAtual = 0;
    //Ficheiros
    static bool aguardandoRetomada = false;

    static void Main()
    {
        string wavyID = "WavyA6";
        string agregadorID = "Agregador 456";
        cliente = new TcpClient("127.0.0.1", 4500);
        stream = cliente.GetStream();

        // Caminho para a pasta "Data source" dentro do repositório
        string basePath = "C:/Users/mikeb/source/repos/SD_PL3_78321_78282_78806/WavyA6/Data";

        // Carregar dados dos arquivos Excel usando ClosedXML
        humidade = ReadExcelColumn(Path.Combine(basePath, "Humidade.xlsx"), 1);  // Coluna A
        ondulacao = ReadExcelColumn(Path.Combine(basePath, "Ondulação.xlsx"), 1); // Coluna A
        pressao = ReadExcelColumn(Path.Combine(basePath, "Pressão.xlsx"), 1); // Coluna A
        temperaturaAgua = ReadExcelColumn(Path.Combine(basePath, "Temperatura_Agua.xlsx"), 1); // Coluna A
        temperaturaAr = ReadExcelColumn(Path.Combine(basePath, "Temperatura_Ar.xlsx"), 1); // Coluna A
        vento = ReadExcelColumn(Path.Combine(basePath, "Vento.xlsx"), 1); // Coluna A
        Thread.Sleep(2000);

        string mensagem = $"Registo;{wavyID}";
        Console.WriteLine($"(Envio) Registo: {wavyID} -> {agregadorID}\n");
        byte[] dados = Encoding.UTF8.GetBytes(mensagem);
        stream.Write(dados, 0, dados.Length);
        byte[] buffer = new byte[1024];
        int bytesLidos = stream.Read(buffer, 0, buffer.Length);
        Console.WriteLine($"(Recebi) {Encoding.UTF8.GetString(buffer, 0, bytesLidos)}; Estado atual: Operacional; Associada a {agregadorID}");
        Console.WriteLine($"-------------------------------------------------\n");
        Thread.Sleep(2000);

        // Inicia a thread para enviar o status
        Thread statusThread = new Thread(EnviarStatus);

        statusThread.Start();  // Agora a thread começa a rodar em paralelo

        // Enviar dados para o agregador
        Thread enviarDadosThread = new Thread(EnviarDados);
        enviarDadosThread.Start();

        // Restante do código para comunicação
        while (true)
        {
            bytesLidos = stream.Read(buffer, 0, buffer.Length);
            if (bytesLidos == 0) break;
            string mensagemRecebida = Encoding.UTF8.GetString(buffer, 0, bytesLidos);
            string[] partes = mensagemRecebida.Split(';');
            if (partes[0] == "NOVO_STATUS" || partes[0] == "STATUS")
            {
                if (partes.Length < 3)
                {
                    Console.WriteLine("Mensagem de STATUS inválida!\n");
                    continue;
                }
                string novoEstado = partes[2].Trim(); // Trim para remover espaços extras
                if (novoEstado == estadoAtual)
                {
                    Console.WriteLine($"(Recebi) {mensagemRecebida}\n");
                    Console.WriteLine($"(Recebi) Atualização de STATUS MANTEVE {novoEstado}");
                    Console.WriteLine($"-------------------------------------------------\n");
                }
                else
                {
                    Console.WriteLine($"(Recebi) {mensagemRecebida}\n");
                    Console.WriteLine($"(Recebi) Atualização de STATUS. {estadoAtual} -> {novoEstado}");
                    if (novoEstado == "Manutenção")
                    {
                        Thread.Sleep(2000); // Aguarda por 2 segundos
                        Console.WriteLine($"\nA {wavyID} está em manutenção e não enviará nem STATUS nem DADOS até receber nova atualização...");
                    }
                    else if (novoEstado == "Desativada")
                    {
                        Thread.Sleep(2000); // Aguarda por 2 segundos
                        Console.WriteLine($"\nA {wavyID} está desativada e vai-se desligar...");
                        try
                        {
                            Console.WriteLine($"Wavy {wavyID} está sendo desativada...");
                            string mensagemEncerramento = "DESATIVADO";
                            byte[] bufferEncerramento = Encoding.UTF8.GetBytes(mensagemEncerramento);
                            stream.Write(bufferEncerramento, 0, bufferEncerramento.Length);
                            stream.Close();
                            cliente.Close();
                            Console.WriteLine("Conexão encerrada com sucesso.");
                            Environment.Exit(0);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Erro ao encerrar a conexão: " + ex.Message);
                            Environment.Exit(1); // Encerra com código de erro
                        }
                    }
                    else if (novoEstado == "Operacional" && estadoAtual == "Manutenção")
                    {
                        Thread.Sleep(2000); // Aguarda por 2 segundos
                        Console.WriteLine($"\nA {wavyID} voltou ao estado operacional, mas aguardará a sincronização com outras Wavys antes de enviar dados.");
                        statusRecemAtualizado = true; // Marca que houve uma mudança de estado
                    }
                    else
                    {
                        Thread.Sleep(2000); // Aguarda por 2 segundos
                        Console.WriteLine($"\nA {wavyID} está em estado desconhecido: {novoEstado}");
                    }
                }
                estadoAtual = novoEstado;
                aguardandoConfirmacao = false;
                aguardandoRetomada = false; // Reset the pause state when receiving a status update
                statusAtualizado.Set();
                statusAtualizado.Reset();
            }
            else if (partes[0] == "Confirmação STATUS")
            {
                Console.WriteLine($"(Recebi) {mensagemRecebida}\n");
                aguardandoConfirmacao = false;
            }
            else if (mensagemRecebida.StartsWith("CONFIRMAÇÃO DADOS"))
            {
                if (!aguardandoConfirmacao) // Só imprime se ainda está aguardando
                {
                    Console.WriteLine($"\n(Recebi) {mensagemRecebida}");
                    Console.WriteLine($"-------------------------------------------------\n");
                    aguardandoConfirmacao = false; // Reseta a flag após receber a confirmação
                }
            }
            else if (partes[0] == "PAUSA")
            {
                Console.WriteLine($"(Recebi) {mensagemRecebida}");
                Console.WriteLine($"Recebido comando de pausa do Agregador. A parar o envio de dados...");
                Console.WriteLine($"-------------------------------------------------\n");

                // Set the pause flag
                aguardandoRetomada = true;

                // If we're in the middle of sending data, wait for it to complete
                while (sendingData)
                {
                    Thread.Sleep(100);
                }
            }
            else
            {
                Console.WriteLine($"Mensagem recebida: {mensagemRecebida}\n");
            }
        }
    }
    static void EnviarStatus()
    {
        string wavyID = "WavyA6";
        while (true)
        {
            if (estadoAtual == "Desativada")
            {
                try
                {
                    Console.WriteLine($"Wavy {wavyID} está sendo desativada...");
                    string mensagemEncerramento = "DESATIVADO";
                    byte[] bufferEncerramento = Encoding.UTF8.GetBytes(mensagemEncerramento);
                    stream.Write(bufferEncerramento, 0, bufferEncerramento.Length);
                    stream.Close();
                    cliente.Close();
                    Console.WriteLine("Conexão encerrada com sucesso.");
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao encerrar a conexão: " + ex.Message);
                    Environment.Exit(1);
                }
            }
            /*else if (estadoAtual == "Manutenção")
            {
                //Thread.Sleep(10 * 60 * 1000); // Aguarda por 10 minutos
                Thread.Sleep(30000); //30s
                estadoAtual = "Operacional";
                Console.WriteLine("Wavy voltou ao estado operacional.\n");
            }*/
            else if (estadoAtual == "Manutenção")
            {
                // A Wavy em manutenção não envia status automaticamente
                // Aguarda até que o estado seja alterado pelo agregador
                Thread.Sleep(30000); // Aguarda 30 segundos antes de verificar novamente
            }
            else
            {
                if (!aguardandoConfirmacao)
                {
                    Thread.Sleep(10000); // Espera 10 segundos
                    Console.WriteLine($"(Envio) STATUS {wavyID} : {estadoAtual}\n");
                    byte[] dados = Encoding.UTF8.GetBytes($"STATUS;{wavyID};{estadoAtual}");
                    stream.Write(dados, 0, dados.Length);
                    aguardandoConfirmacao = true;
                }
                Thread.Sleep(10 * 60 * 1000); // Aguarda 10 minutos entre envios
            }
        }
    }
    // Método para ler dados de uma coluna específica de um arquivo Excel
    static List<string> ReadExcelColumn(string path, int colIndex)
    {
        var valores = new List<string>();
        using (var workbook = new XLWorkbook(path))
        {
            var planilha = workbook.Worksheet(1); // Acessa a primeira planilha
            var coluna = planilha.Column(colIndex); // Acessa a coluna especificada (base 1)
            foreach (var celula in coluna.CellsUsed())
            {
                valores.Add(celula.GetValue<string>());
            }
        }
        return valores;
    }

    // Add these static variables to each Wavy class
    static bool resumingFromPause = false;
    static int cycleTime = 15000; // Total time for one complete cycle (all 3 Wavys)
    static volatile bool sendingData = false;

    static bool statusRecemAtualizado = false;


    static void EnviarDados()
    {
        string wavyID = "WavyA6";
        string agregadorID = "Agregador456";
        statusAtualizado.WaitOne();

        // WavyA1 starts first in each cycle
        int slotInCycle = 2; // 0 = first position
        int slotTime = 5000; // Each Wavy gets 5 seconds to send data

        // Initial delay - WavyA1 starts first
        Thread.Sleep(slotInCycle * slotTime);

        int contador = 0;
        int totalDados = 32;
        DateTime lastSendTime = DateTime.Now;

        // Variável para controlar se estamos aguardando que todas as Wavys recebam status
        bool aguardandoSincronizacao = false;

        while (contador < totalDados)
        {

            if (statusRecemAtualizado)
            {
                Console.WriteLine("Aguardando sincronização com outras Wavys...");
                Thread.Sleep(5000);
                statusRecemAtualizado = false;
                lastSendTime = DateTime.Now;
            }

            // Check if we're paused or not operational
            if (aguardandoRetomada || estadoAtual != "Operacional")
            {
                Thread.Sleep(1000);

                if (!aguardandoRetomada && estadoAtual == "Operacional")
                {
                    resumingFromPause = true;
                    aguardandoSincronizacao = true;
                    lastSendTime = DateTime.Now;
                }
                continue;
            }

            TimeSpan timeSinceLastSend = DateTime.Now - lastSendTime;

            if (resumingFromPause)
            {
                Console.WriteLine("Aguardando confirmação de que todas as Wavys receberam as suas atualizações de estado...");
                Console.WriteLine("-------------------------------------------------\n");
                Thread.Sleep(2000);

                resumingFromPause = false;

                Thread.Sleep(10000 + 1000);
            }

            else if (timeSinceLastSend.TotalMilliseconds < cycleTime)
            {

                int waitTime = (int)(cycleTime - timeSinceLastSend.TotalMilliseconds);

                int remainingWait = waitTime;
                while (remainingWait > 0 && !aguardandoRetomada)
                {
                    int sleepTime = Math.Min(1000, remainingWait);
                    Thread.Sleep(sleepTime);
                    remainingWait -= sleepTime;

                    if (aguardandoRetomada)
                    {
                        break;
                    }
                }

                if (aguardandoRetomada)
                {
                    continue;
                }
            }

            if (aguardandoRetomada)
            {
                continue;
            }

            sendingData = true;

            // Prepare the data to send
            string dadosEnviados = $"DADOS;{wavyID};Humidade: {humidade[linhaAtual]}, Ondulação: {ondulacao[linhaAtual]}, " +
                                   $"Pressão: {pressao[linhaAtual]}, Temperatura_Agua: {temperaturaAgua[linhaAtual]}, " +
                                   $"Temperatura_Ar: {temperaturaAr[linhaAtual]}, Vento: {vento[linhaAtual]}";


            if (!aguardandoRetomada)
            {
                byte[] dados = Encoding.UTF8.GetBytes(dadosEnviados);
                stream.Write(dados, 0, dados.Length);
                Console.WriteLine($"(Envio) {dadosEnviados}");
                contador++;
                linhaAtual = (linhaAtual + 1) % humidade.Count;

                lastSendTime = DateTime.Now;
            }

            // Clear the sending flag
            sendingData = false;
        }
        Console.WriteLine("Já não existem mais dados para enviar.");
    }
}
