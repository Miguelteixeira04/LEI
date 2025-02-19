using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Utad.Lab.PL4.G01.Classes;
using System.Windows;
using System;
using System.Linq;

namespace Utad.Lab.PL4.G01
{
    public partial class Window_AdicionarTarefa : Window
    {
        private App app;
        public Window_AdicionarTarefa()
        {
            InitializeComponent();
            PopulateTimePickers();

            this.WindowState = WindowState.Maximized;
            app = App.Current as App;
        }

        private void PopulateTimePickers()
        {
            for (int i = 0; i < 24; i++)
            {
                HourComboBox1.Items.Add(i.ToString("D2"));
                HourComboBox2.Items.Add(i.ToString("D2"));
            }

            for (int i = 0; i < 60; i++)
            {
                MinuteComboBox1.Items.Add(i.ToString("D2"));
                MinuteComboBox2.Items.Add(i.ToString("D2"));
            }
        }

        private void tb_nometarefa_GotFocus(object sender, RoutedEventArgs e)
        {
            tb_nometarefa.Text = "";
        }

        private void tb_descricao_GotFocus(object sender, RoutedEventArgs e)
        {
            tb_descricao.Text = "";
        }

        private void cb_importancia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = cb_importancia.SelectedItem as ComboBoxItem;

            if (selectedItem != null && selectedItem.Content.ToString() == "Prioritária")
            {
                // Procura o item "Antecipação e Execução" na ComboBox cb_alertas
                ComboBoxItem antecipacaoExecucaoItem = null;
                foreach (ComboBoxItem item in cb_alertas.Items)
                {
                    if (item.Content.ToString() == "Antecipação e Execução")
                    {
                        antecipacaoExecucaoItem = item;
                        break;
                    }
                }

                // Se encontrou o item "Antecipação e Execução", habilita e seleciona
                if (antecipacaoExecucaoItem != null)
                {
                    antecipacaoExecucaoItem.IsEnabled = true;
                    cb_alertas.SelectedItem = antecipacaoExecucaoItem;
                    cb_alertas.IsEnabled = false;
                }
            }
            else
            {
                // Se a importância selecionada não for "Prioritária", desabilita o item "Antecipação e Execução"
                foreach (ComboBoxItem item in cb_alertas.Items)
                {
                    if (item.Content.ToString() == "Antecipação e Execução")
                    {
                        item.IsEnabled = true;
                        break;
                    }
                }
                cb_alertas.IsEnabled = true;
            }
        }

        private void btn_check_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // MENSAGENS DE ERRO
            if (string.IsNullOrWhiteSpace(tb_nometarefa.Text) || tb_nometarefa.Text == " Insira o nome da tarefa" || !dp_inicio.SelectedDate.HasValue 
                || !dp_termino.SelectedDate.HasValue || dp_termino.SelectedDate.Value < dp_inicio.SelectedDate.Value || HourComboBox1.SelectedItem == null
                || HourComboBox2.SelectedItem == null || MinuteComboBox1.SelectedItem == null || MinuteComboBox2.SelectedItem == null
                || cb_alertas.SelectedItem == null || cb_importancia.SelectedItem == null || cb_periodicidade.SelectedItem == null)
            {
                MessageBox.Show("Confirme todos os campos da tarefa!", "ERRO Adicionar Tarefa", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (dp_termino.SelectedDate.Value == dp_inicio.SelectedDate.Value)
            {
                // se a tarefa for num dia, a hora de fim tem que ser depois da hora de inicio
                if (Convert.ToInt32(HourComboBox2.SelectedItem) < Convert.ToInt32(HourComboBox1.SelectedItem) ||
                    (Convert.ToInt32(HourComboBox2.SelectedItem) == Convert.ToInt32(HourComboBox1.SelectedItem) &&
                    Convert.ToInt32(MinuteComboBox2.SelectedItem) <= Convert.ToInt32(MinuteComboBox1.SelectedItem)))
                {
                    MessageBox.Show("Confirme todos os campos da tarefa!", "ERRO Adicionar Tarefa", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            // hora:minuto
            string horaInicio = $"{HourComboBox1.SelectedItem}:{MinuteComboBox1.SelectedItem}";
            string horaFim = $"{HourComboBox2.SelectedItem}:{MinuteComboBox2.SelectedItem}";
            // hora e minuto separados
            string horaInicial = HourComboBox1.SelectedItem.ToString();
            string horaFinal = HourComboBox2.SelectedItem.ToString();
            string minInicial = MinuteComboBox1.SelectedItem.ToString();
            string minFinal = MinuteComboBox2.SelectedItem.ToString();

            string descricao = string.Empty; // garantir que a string está vazia
            if (tb_descricao.Text == " (Opcional)")
                descricao = "";
            else
                descricao = tb_descricao.Text;

            // componentes que deve ir buscar e colocar na ListView das tarefas
            Tarefa novaTarefa = new Tarefa
            {
                Id = App.NextTaskId,

                Titulo = tb_nometarefa.Text,
                Descricao = descricao,
                DataInicio = dp_inicio.SelectedDate.Value,
                DataTermino = dp_termino.SelectedDate.Value,
                HoraInicio = horaInicio,
                HoraFim = horaFim,
                NivelImportancia = (cb_importancia.SelectedItem as ComboBoxItem).Content.ToString(),
                Estado = "Por Iniciar", // estado inicial por defeito
                Periodicidade = (cb_periodicidade.SelectedItem as ComboBoxItem).Content.ToString(),
                Alerta = (cb_alertas.SelectedItem as ComboBoxItem).Content.ToString(),

                HoraInicial = horaInicial,
                HoraFinal = horaFinal,
                MinInicial = minInicial,
                MinFinal = minFinal,

                Tipo = "Fixa", // tipo da periodicidade inicial por defeito 
            };

            Periodicidade novaPeriodicidade = new Periodicidade
            {
                Id = novaTarefa.Id,
                Tipo = "Fixa", // Tipo inicial por defeito
            };

            app.Classes.Tarefas.Add(novaTarefa);
            App.NextTaskId++;
            this.Close();
        }

        private void menu_inicio_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void menu_perfil_Click(object sender, RoutedEventArgs e)
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
