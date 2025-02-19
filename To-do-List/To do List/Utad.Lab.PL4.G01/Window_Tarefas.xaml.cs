using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Utad.Lab.PL4.G01.Classes;

namespace Utad.Lab.PL4.G01
{
    public partial class Window_Tarefas : Window
    {
        public App app;
        public Window_Tarefas()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;

            app = App.Current as App;
            lv_tarefas.ItemsSource = app.Classes.Tarefas;

            // elementos que não devem aparecer no inicio
            tblc_descricao.Visibility = Visibility.Hidden;
            btn_lapis.Visibility = Visibility.Hidden;
            lb_descricao.Visibility = Visibility.Hidden;
            cb_mudarestado.Visibility = Visibility.Hidden;
            lb_mudarestado.Visibility = Visibility.Hidden;
            btn_eliminar.Visibility = Visibility.Hidden;
        }
        private void btn_confalert_Click(object sender, RoutedEventArgs e)
        {
            // Obtém o item de dados associado à linha do botão clicado
            var button = sender as Button;
            var item = button?.DataContext as Tarefa; // Substitua 'Tarefa' pelo tipo do seu item de dados

            if (item != null)
            {
                // Extraia os dados necessários do item de dados
                string selectedAlert = item.Alerta;
                string formattedTime = item.HoraInicio; // Assume que HoraInicio já está formatado como "HH:mm"
                string formattedEndTime = item.HoraFim; // Assume que HoraFim já está formatado como "HH:mm"
                string startDate = item.DataInicio.ToString("dd/MM/yyyy");
                string endDate = item.DataTermino.ToString("dd/MM/yyyy");
                int id = item.Id;

                // Adicione a linha para obter a descrição da tarefa como nome da tarefa
                string nomeTarefa = item.Titulo;
                

                Window_Alertas windowAlerta = new Window_Alertas(selectedAlert, formattedTime, formattedEndTime, startDate, endDate, id, nomeTarefa, this); // Modifique esta linha
                windowAlerta.AlertChanged += (s, newAlert) =>
                {
                    item.Alerta = newAlert;
                    lv_tarefas.Items.Refresh(); // Atualiza a ListView para refletir a mudança
                };
                windowAlerta.ShowDialog();
            }
        }

        private void btn_confPerio_Click(object sender, RoutedEventArgs e)
        {
            // Obtém o item de dados associado à linha do botão clicado
            var button = sender as Button;
            var item = button?.DataContext as Tarefa; // Substitua 'Tarefa' pelo tipo do seu item de dados
            var Item = button?.DataContext as Periodicidade;

            if (item != null)
            {
                // Extraia os dados necessários do item de dados
                string hora_inicio = item.HoraInicial;
                string hora_fim = item.HoraFinal;
                string min_inicio = item.MinInicial;
                string min_fim = item.MinFinal;
                string startDate = item.DataInicio.ToString("dd/MM/yyyy");
                string endDate = item.DataTermino.ToString("dd/MM/yyyy");
                int id = item.Id;
                string selectedPeriodicidade = item.Periodicidade;
                string selectedTipo = item.Tipo;
                bool d1 = item.d1;
                bool d2 = item.d2;
                bool d3 = item.d3;
                bool d4 = item.d4;
                bool d5 = item.d5;
                bool d6 = item.d6;
                bool d7 = item.d7;

                // Crie e mostre a nova janela com os dados necessários
                Window_Periodicidade windowPeriodicidade = new Window_Periodicidade(hora_inicio, hora_fim, min_inicio, min_fim , startDate, endDate, id, selectedPeriodicidade, selectedTipo, d1, d2, d3, d4, d5, d6, d7);
                windowPeriodicidade.MudaPeriodicidade += (s, newPerio) =>
                {
                    item.Periodicidade = newPerio;
                    lv_tarefas.Items.Refresh(); // Atualiza a ListView para refletir a mudança
                };
                windowPeriodicidade.MudaTipo += (s, newTipo) =>
                {
                    item.Tipo = newTipo;
                    lv_tarefas.Items.Refresh();
                };
                windowPeriodicidade.MudaSegunda += (s, newd1) =>
                {
                    item.d1 = newd1;
                    lv_tarefas.Items.Refresh();
                };
                windowPeriodicidade.MudaTerca += (s, newd2) =>
                {
                    item.d2 = newd2;
                    lv_tarefas.Items.Refresh();
                };
                windowPeriodicidade.MudaQuarta += (s, newd3) =>
                {
                    item.d3 = newd3;
                    lv_tarefas.Items.Refresh();
                };
                windowPeriodicidade.MudaQuinta += (s, newd4) =>
                {
                    item.d4 = newd4;
                    lv_tarefas.Items.Refresh();
                };
                windowPeriodicidade.MudaSexta += (s, newd5) =>
                {
                    item.d5 = newd5;
                    lv_tarefas.Items.Refresh();
                };
                windowPeriodicidade.MudaSabado += (s, newd6) =>
                {
                    item.d6 = newd6;
                    lv_tarefas.Items.Refresh();
                };
                windowPeriodicidade.MudaDomingo += (s, newd7) =>
                {
                    item.d7 = newd7;
                    lv_tarefas.Items.Refresh();
                };
                windowPeriodicidade.ShowDialog();
            }
        }
        private void Filtrar(Window_FiltroTarefas filtro)
        {
            List<Tarefa> tarefas_filtradas = new List<Tarefa>(app.Classes.Tarefas); // lista de todas as tarefas

            // filtro por dia
            if (filtro.Dia.HasValue)
            {
                List<Tarefa> tarefas_dia = new List<Tarefa>();
                foreach (Tarefa tarefa in tarefas_filtradas)
                {
                    if (tarefa.DataInicio.Date == filtro.Dia.Value.Date)
                    {
                        tarefas_dia.Add(tarefa);
                    }
                }
                tarefas_filtradas = tarefas_dia;
            }

            // filtro 7 dias
            if (filtro.PrimeiroDiaSemana.HasValue && filtro.UltimoDiaSemana.HasValue)
            {
                List<Tarefa> tarefas_semana = new List<Tarefa>();
                foreach (Tarefa tarefa in tarefas_filtradas)
                {
                    if (tarefa.DataInicio.Date >= filtro.PrimeiroDiaSemana.Value.Date && tarefa.DataInicio.Date <= filtro.UltimoDiaSemana.Value.Date)
                    {
                        tarefas_semana.Add(tarefa);
                    }
                }
                tarefas_filtradas = tarefas_semana;
            }

            //filtro intervalo dias
            if (filtro.DataInicio.HasValue && filtro.DataFim.HasValue)
            {
                List<Tarefa> tarefas_intervalo = new List<Tarefa>();
                foreach (Tarefa tarefa in tarefas_filtradas)
                {
                    if (tarefa.DataInicio.Date >= filtro.DataInicio.Value.Date && tarefa.DataInicio.Date <= filtro.DataFim.Value.Date)
                    {
                        tarefas_intervalo.Add(tarefa);
                    }
                }
                tarefas_filtradas = tarefas_intervalo;
            }
            // atualizar a listview
            lv_tarefas.ItemsSource = tarefas_filtradas;
        }
        private void btn_filtro_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Opacity = 0.5;
            Window_FiltroTarefas window_FiltroTarefas = new Window_FiltroTarefas(this);
            
            // definir posicao no ecra
            window_FiltroTarefas.Left = 250; // posição horizontal
            window_FiltroTarefas.Top = 180; // posição vertical

            if (window_FiltroTarefas.ShowDialog() == true)
                Filtrar(window_FiltroTarefas);
            else
                lv_tarefas.ItemsSource = app.Classes.Tarefas.ToList(); // restaura a lista de tarefas
                this.Opacity = 1.0;
        }

        private void btn_lapis_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (lv_tarefas.SelectedItem is Tarefa selectedTarefa)
            {
                this.Opacity = 0.5;

                Window_Descricao window_Descricao = new Window_Descricao(this, selectedTarefa.Descricao);
                window_Descricao.Left = 490; // posição horizontal
                window_Descricao.Top = 240; // posição vertical

                if (window_Descricao.ShowDialog() == true)
                {
                    selectedTarefa.Descricao = window_Descricao.Descricao;
                    tblc_descricao.Text = window_Descricao.Descricao; // Atualiza a descrição na ListView
                    lv_tarefas.Items.Refresh(); // atualizar listview
                }

                else
                    this.Opacity = 1.0;
            }
        }

        private void btn_add_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Window_AdicionarTarefa window_AdicionarTarefa = new Window_AdicionarTarefa();
            window_AdicionarTarefa.ShowDialog();
            lv_tarefas.ItemsSource = app.Classes.Tarefas;
        }

        // mudar a descricao consoante o addtarefas
        private void lv_tarefas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lv_tarefas.SelectedItem is Tarefa selectedTarefa)
            {
                // elementos que aparecem quando se seleciona uma tarefa
                tblc_descricao.Visibility = Visibility.Visible;
                btn_lapis.Visibility = Visibility.Visible;
                lb_descricao.Visibility = Visibility.Visible;
                cb_mudarestado.Visibility = Visibility.Visible;
                lb_mudarestado.Visibility = Visibility.Visible;
                btn_eliminar.Visibility = Visibility.Visible;
                tblc_descricao.Text = selectedTarefa.Descricao;

                // atualizar combobox para o estado da tarefa selecionada
                foreach (ComboBoxItem item in cb_mudarestado.Items)
                {
                    if (item.Content.ToString() == selectedTarefa.Estado)
                    {
                        cb_mudarestado.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        // mudar o estado consoante a textbox
        private void cb_mudarestado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lv_tarefas.SelectedItem is Tarefa selectedTarefa && cb_mudarestado.SelectedItem is ComboBoxItem selectedItem)
            {
                selectedTarefa.Estado = selectedItem.Content.ToString();
                lv_tarefas.Items.Refresh(); // atualiza o estado na ListView
            }
        }

        private void btn_eliminar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult resultado = MessageBox.Show("Tem a certeza que deseja eliminar a tarefa?", "Eliminar Tarefa", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (resultado == MessageBoxResult.Yes)
            {
                if (lv_tarefas.SelectedItem is Tarefa selectedTarefa)
                {
                    app.Classes.Tarefas.Remove(selectedTarefa);
                    lv_tarefas.ItemsSource = app.Classes.Tarefas; // atualiza o estado na ListView

                    // ocultar os elementos que só aparecem quando uma tarefa é selecionada
                    tblc_descricao.Visibility = Visibility.Hidden;
                    btn_lapis.Visibility = Visibility.Hidden;
                    lb_descricao.Visibility = Visibility.Hidden;
                    cb_mudarestado.Visibility = Visibility.Hidden;
                    lb_mudarestado.Visibility = Visibility.Hidden;
                    btn_eliminar.Visibility = Visibility.Hidden;
                }
                else
                    MessageBox.Show("Operação cancelada!", "Eliminar Tarefa", MessageBoxButton.OKCancel);
            }
        }

        private void menu_inicio_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void menu_perfil_Click_1(object sender, RoutedEventArgs e)
        {
            Window_perfil window_perfil = new Window_perfil();
            window_perfil.Show();
            this.Close();
        }
    }
}
