using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Toolkit.Uwp.Notifications;
using Utad.Lab.PL4.G01.Classes;

namespace Utad.Lab.PL4.G01
{
    public partial class Window_Alertas : Window
    {
        private Alerta alerta;
        private App app;
        private Window_Tarefas windowTarefas;
        private static Dictionary<int, string> alertTypesByTask = new Dictionary<int, string>();
        private static Dictionary<int, string> alertNotesByTask = new Dictionary<int, string>();
        private static Dictionary<int, bool> alertStatesByTask = new Dictionary<int, bool>();
        private bool isCheckBoxChecked = true;
        public event EventHandler<string> AlertChanged;

        private int hour = 0;
        private int minute = 0;
        private string formattedDate = "";
        private string initialStartDate;
        private string initialStartTime;
        private string initialEndDate;
        private string initialEndTime;
        private System.Timers.Timer timer;
        private string nomeTarefa;

        public Window_Alertas(string selectedAlert, string formattedStartTime, string formattedEndTime, string startDate, string endDate, int id, string nomeTarefa, Window_Tarefas windowTarefas)
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;

            // Carrega os valores de cb_tipo e tb_nota_alerta
            LoadSavedAlertType(id);
            LoadSavedAlertNote(id);

            this.alerta = new Alerta
            {
                Id = id,
                Mensagem = " ",
                DataInicial = DateTime.Parse(startDate),
                DataFinal = DateTime.Parse(endDate),
                HoraInicial = TimeSpan.Parse(formattedStartTime),
                HoraFinal = TimeSpan.Parse(formattedEndTime),
                TipoEnvio = "Windows",
                TipoAlerta = "Antecipação",
                Estado = false
            };

            this.app = Application.Current as App;
            this.app.Classes.Alertas.Add(this.alerta);

            this.initialStartDate = startDate;
            this.initialStartTime = formattedStartTime;
            this.initialEndDate = endDate;
            this.initialEndTime = formattedEndTime;
            this.nomeTarefa = nomeTarefa;
            this.windowTarefas = windowTarefas;

            SetAlertType(selectedAlert, formattedStartTime, formattedEndTime, startDate, endDate);
            tb_id.Text = id.ToString();
            cb_alertas.SelectionChanged += Cb_alertas_SelectionChanged;

            LoadSavedAlertType(id);
            LoadSavedAlertNote(id);

            // carrega e guarda o estado da checkbox
            if (alertStatesByTask.TryGetValue(id, out bool savedCheckBoxState))
            {
                cb_ativar.IsChecked = savedCheckBoxState;
            }
            else
            {
                cb_ativar.IsChecked = isCheckBoxChecked; // valor da checkbox por defeito
            }
            CarregarDadosAlerta(id);
        }

        // Método para carregar os dados do ficheiro dadosAlertas.xml para a tarefa específica (id)
        private void CarregarDadosAlerta(int taskId)
        {
            // Verifica se o alerta existe para a tarefa especificada
            Alerta alerta = app.Classes.Alertas.FirstOrDefault(a => a.Id == taskId);
            if (alerta != null)
            {
                // Define os valores dos controles na janela com os dados do alerta carregado
                tb_nota_alerta.Text = alerta.Mensagem;
                // Define o tipo de envio
                foreach (ComboBoxItem item in cb_tipo.Items)
                {
                    if (item.Content.ToString() == alerta.TipoEnvio)
                    {
                        cb_tipo.SelectedItem = item;
                        break;
                    }
                }
                // Define o estado do checkbox
                cb_ativar.IsChecked = alerta.Estado;
            }
        }


        // Métodos para salvar e carregar estado
        private void SaveCheckBoxState()
        {
            int taskId = int.Parse(tb_id.Text);
            isCheckBoxChecked = cb_ativar.IsChecked == true;
            // salva o estado para o dicionario
            alertTypesByTask[taskId] = ((ComboBoxItem)cb_tipo.SelectedItem).Content.ToString();
            alertNotesByTask[taskId] = tb_nota_alerta.Text;
            // salva o estado da checkbox para o dicionario
            if (!alertStatesByTask.ContainsKey(taskId))
            {
                alertStatesByTask.Add(taskId, isCheckBoxChecked);
            }
            else
            {
                alertStatesByTask[taskId] = isCheckBoxChecked;
            }
        }

        private void LoadSavedAlertType(int taskId)
        {
            if (alertTypesByTask.TryGetValue(taskId, out string savedAlertType))
            {
                foreach (ComboBoxItem item in cb_tipo.Items)
                {
                    if (item.Content.ToString() == savedAlertType)
                    {
                        cb_tipo.SelectedItem = item;
                        break;
                    }
                }
            }
            else
            {
                cb_tipo.SelectedIndex = -1;
            }
        }

        private void LoadSavedAlertNote(int taskId)
        {
            if (alertNotesByTask.TryGetValue(taskId, out string savedAlertNote))
            {
                tb_nota_alerta.Text = savedAlertNote;
            }
            else
            {
                tb_nota_alerta.Text = "";
            }
        }

        private void Cb_alertas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string newAlertType = ((ComboBoxItem)cb_alertas.SelectedItem)?.Content.ToString();

            if (newAlertType == "Antecipação")
            {
                SetAlertType(newAlertType, initialStartTime, initialEndTime, initialStartDate, initialEndDate);
            }
            else if (newAlertType == "Execução")
            {
                SetAlertType(newAlertType, initialEndTime, initialStartTime, initialEndDate, initialStartDate);
            }
            else if (newAlertType == "Antecipação e Execução")
            {
                SetAlertType(newAlertType, initialStartTime, initialEndTime, initialStartDate, initialEndDate);
            }
        }

        private void tb_nota_alerta_GotFocus(object sender, RoutedEventArgs e)
        {
            tb_nota_alerta.Text = "";
        }

        private void cb_ativar_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cb_ativar = sender as CheckBox;
            if (cb_ativar != null)
            {
                isCheckBoxChecked = cb_ativar.IsChecked == true;
            }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SaveCheckBoxState();

            if (cb_tipo.SelectedItem == null)
            {
                MessageBox.Show("Por favor, selecione um tipo de alerta.", "Tipo de Alerta não Selecionado", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (cb_ativar.IsChecked == true)
            {
                string notaAlerta = tb_nota_alerta.Text.Trim();
                string textoPadrao = "(Insira uma nota relativa ao alarme aqui)";
                string notaParaMensagem = string.IsNullOrWhiteSpace(notaAlerta) || notaAlerta == textoPadrao ? "Sem nota definida" : notaAlerta;

                MessageBox.Show("Alerta salvo com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                AlertChanged?.Invoke(this, ((ComboBoxItem)cb_alertas.SelectedItem).Content.ToString());

                int taskId = int.Parse(tb_id.Text);
                alertTypesByTask[taskId] = ((ComboBoxItem)cb_tipo.SelectedItem).Content.ToString();
                alertNotesByTask[taskId] = tb_nota_alerta.Text;

                string selectedAlertType = ((ComboBoxItem)cb_alertas.SelectedItem).Content.ToString();

                alerta.Id = int.Parse(tb_id.Text);
                alerta.Mensagem = tb_nota_alerta.Text;
                alerta.Estado = true;

                if (selectedAlertType == "Antecipação")
                {
                    alerta.DataInicial = DateTime.Parse(tb_data.Text.Trim());
                    alerta.HoraInicial = TimeSpan.Parse(tb_hora.Text.Trim());
                }
                else if (selectedAlertType == "Execução")
                {
                    alerta.DataFinal = DateTime.Parse(tb_data.Text.Trim());
                    alerta.HoraFinal = TimeSpan.Parse(tb_hora.Text.Trim());
                }
                else if (selectedAlertType == "Antecipação e Execução")
                {
                    string[] dates = tb_data.Text.Split('&');
                    string[] times = tb_hora.Text.Split('&');

                    alerta.DataInicial = DateTime.Parse(dates[0].Trim());
                    alerta.HoraInicial = TimeSpan.Parse(times[0].Trim());

                    alerta.DataFinal = DateTime.Parse(dates[1].Trim());
                    alerta.HoraFinal = TimeSpan.Parse(times[1].Trim());
                }

                alerta.TipoAlerta = selectedAlertType;
                alerta.TipoEnvio = ((ComboBoxItem)cb_tipo.SelectedItem).Content.ToString();
                alerta.Estado = cb_ativar.IsChecked == true;

                if (((ComboBoxItem)cb_tipo.SelectedItem).Content.ToString() == "Windows" || ((ComboBoxItem)cb_tipo.SelectedItem).Content.ToString() == "Email e Windows")
                {
                    AtivaNotificacao();
                }
            }
            else
            {
                int taskId = int.Parse(tb_id.Text);
                alertTypesByTask[taskId] = ((ComboBoxItem)cb_tipo.SelectedItem).Content.ToString();
                alertNotesByTask[taskId] = tb_nota_alerta.Text;
                alerta.Estado = false;
                MessageBox.Show("Alerta Desativado", "Alerta Desativado", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            this.Close();
        }

        private void cb_tipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string tipoSelecionado = ((ComboBoxItem)cb_tipo.SelectedItem)?.Content.ToString();
        }

        private void menu_perfil_Click(object sender, RoutedEventArgs e)
        {
            Window_editar_perfil window_editar_perfil = new Window_editar_perfil();
            window_editar_perfil.Show();
        }

        // Métodos de apoio
        private void SetAlertType(string alertType, string startTime, string endTime, string startDate, string endDate)
        {
            if (alertType != null)
            {
                foreach (ComboBoxItem item in cb_alertas.Items)
                {
                    if (item.Content.ToString() == alertType)
                    {
                        cb_alertas.SelectedItem = item;
                        break;
                    }
                }
            }

            if (alertType == "Antecipação")
            {
                AdjustTime(startTime, -30);
                tb_data.Text = initialStartDate;
                tb_hora.Text = formattedDate;
            }

            else if (alertType == "Execução")
            {
                AdjustTime(endTime, 1);
                tb_data.Text = initialEndDate;
                tb_hora.Text = formattedDate;
            }

            else if (alertType == "Antecipação e Execução")
            {
                AdjustTime(startTime, -30);
                string startFormattedTime = formattedDate;

                AdjustTime(endTime, 1);
                string endFormattedTime = formattedDate;

                tb_data.Text = $"{startDate} & {endDate}";
                tb_hora.Text = $"{startFormattedTime} & {endFormattedTime}";
            }
        }

        private void AdjustTime(string time, int minuteAdjustment)
        {
            string[] timeParts = time.Split(':');
            hour = int.Parse(timeParts[0]);
            minute = int.Parse(timeParts[1]);
            minute += minuteAdjustment;

            if (minute >= 60)
            {
                minute -= 60;
                hour += 1;
            }
            else if (minute < 0)
            {
                minute += 60;
                hour -= 1;
            }
            formattedDate = $"{hour:D2}:{minute:D2}";
        }

        // Métodos de envio de email e notificação
        public void AtivaNotificacao()
        {
            DateTime currentTime = DateTime.Now;

            if ((((ComboBoxItem)cb_alertas.SelectedItem).Content.ToString() == "Antecipação e Execução"))

            {
                string[] dates = tb_data.Text.Split('&');
                string[] times = tb_hora.Text.Split('&');

                DateTime firstAlertTime, secondAlertTime;

                if (DateTime.TryParse($"{dates[0].Trim()} {times[0].Trim()}", out firstAlertTime) &&
                    DateTime.TryParse($"{dates[1].Trim()} {times[1].Trim()}", out secondAlertTime))
                {
                    TimeSpan timeToFirstAlert = firstAlertTime - currentTime;
                    TimeSpan timeToSecondAlert = secondAlertTime - currentTime;

                    if (timeToFirstAlert.TotalMilliseconds > 0)
                    {
                        Timer firstTimer = new Timer(timeToFirstAlert.TotalMilliseconds);
                        firstTimer.Elapsed += (sender, e) => EnviarNotificacao(sender, e, firstAlertTime);
                        firstTimer.Start();
                    }

                    if (timeToSecondAlert.TotalMilliseconds > 0)
                    {
                        Timer secondTimer = new Timer(timeToSecondAlert.TotalMilliseconds);
                        secondTimer.Elapsed += (sender, e) => EnviarNotificacao(sender, e, secondAlertTime);
                        secondTimer.Start();
                    }
                }
                else
                {
                    MessageBox.Show("Data ou hora inválida para notificação.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error); //estes erros estao a dar
                }
            }
            else
            {
                DateTime alertTime;

                if (DateTime.TryParse($"{tb_data.Text} {tb_hora.Text}", out alertTime))
                {
                    TimeSpan timeToNotificacao = alertTime - currentTime;

                    if (timeToNotificacao.TotalMilliseconds > 0)
                    {
                        timer = new Timer(timeToNotificacao.TotalMilliseconds);
                        timer.Elapsed += EnviarNotificacao;
                        timer.Start();
                    }
                }
                else
                {
                    MessageBox.Show("Data ou hora inválida para notificação.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error); //estes erros estao a dar
                } 
            }
        }

        private void EnviarNotificacao(object sender, ElapsedEventArgs e, DateTime alertTime)
        {
            Timer timer = sender as Timer;
            timer?.Stop();

            Dispatcher.Invoke(() =>
            {
                ComboBoxItem selectedItem = cb_alertas.SelectedItem as ComboBoxItem;
                if (selectedItem != null)
                {
                    string alertaSelecionado = selectedItem.Content.ToString();
                    Tarefa tarefa = windowTarefas.app.Classes.Tarefas.FirstOrDefault(t => t.Id == alerta.Id);
                    string mensagem = "";

                    if (alertaSelecionado == "Execução" || (alertaSelecionado == "Antecipação e Execução" && alertTime == DateTime.Parse(tb_data.Text.Split('&')[1].Trim() + " " + tb_hora.Text.Split('&')[1].Trim())))
                    {
                        if (tarefa != null && tarefa.Estado != "Terminada")
                        {
                            mensagem = $"Data de Término ultrapassada, não declarou a tarefa \"{nomeTarefa}\" como Terminada! ⚠";
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (alertaSelecionado == "Antecipação" || (alertaSelecionado == "Antecipação e Execução" && alertTime == DateTime.Parse(tb_data.Text.Split('&')[0].Trim() + " " + tb_hora.Text.Split('&')[0].Trim())))
                    {
                        mensagem = $"Data de Início aproxima-se, a tarefa \"{nomeTarefa}\" terá início dentro de 30 minutos!⚠";
                    }

                    if (!string.IsNullOrEmpty(mensagem))
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            var toastBuilder = new ToastContentBuilder()
                                .AddText("🔔 Alerta To-Do List 🔔")
                                .AddText(mensagem);

                            if (!string.IsNullOrEmpty(tb_nota_alerta.Text))
                            {
                                toastBuilder.AddText("Nota do Alerta: " + tb_nota_alerta.Text);
                            }
                            //string pathToAudio = @"Icons\notificacao_f1.mp3";

                            toastBuilder
                                //.AddAudio(new Uri(pathToAudio))
                                .AddAttributionText("Enviado às " + tb_hora.Text)
                                .Show();
                        });

                        string selectedTipo = ((ComboBoxItem)cb_tipo.SelectedItem)?.Content.ToString();
                        if (selectedTipo == "Email" || selectedTipo == "Email e Windows")
                        {
                            EnviarEmail(app.MeuPerfil.Email, "🔔 Alerta To-Do List 🔔", mensagem);
                        }
                    }
                }
            });
        }

        private void EnviarNotificacao(object sender, ElapsedEventArgs e)
        {
            EnviarNotificacao(sender, e, DateTime.Now); // Chamada padrão com a hora atual para compatibilidade com alertas únicos
        }

        private void EnviarEmail(string toEmail, string subject, string body)
        {
            try
            {
                var emailSender = new EmailSender();
                emailSender.SendEmail(toEmail, subject, body);
                MessageBox.Show("Email enviado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao enviar o email: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void menu_inicio_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void menu_perfil_Click_1(object sender, RoutedEventArgs e)
        {
            Window_perfil window_Perfil = new Window_perfil();
            window_Perfil.Show();
            this.Close();
        }

        private void menu_tarefas_Click(object sender, RoutedEventArgs e)
        {
            Window_Tarefas window_Tarefas = new Window_Tarefas();
            window_Tarefas.Show();
            this.Close();
        }
    }
}
