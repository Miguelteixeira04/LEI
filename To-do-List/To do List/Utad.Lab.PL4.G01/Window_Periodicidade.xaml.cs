using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
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
    public partial class Window_Periodicidade : Window
    {
        private Periodicidade periodicidade;

        private Tarefa tarefa;

        public event EventHandler<string> MudaPeriodicidade;

        public event EventHandler<string> MudaTipo;

        public event EventHandler<bool> MudaSegunda;

        public event EventHandler<bool> MudaTerca;

        public event EventHandler<bool> MudaQuarta;

        public event EventHandler<bool> MudaQuinta;

        public event EventHandler<bool> MudaSexta;

        public event EventHandler<bool> MudaSabado;

        public event EventHandler<bool> MudaDomingo;

        private static Dictionary<int, string> ChangePeriodicidade = new Dictionary<int, string>();

        string initialStartDate;
        string initialEndDate;
        
        public Window_Periodicidade(string hora_inicio, string hora_fim, string min_inicio, string min_fim, string startDate, string endDate, int id, string selectedPeriodicidade, string selectedTipo, bool d1, bool d2, bool d3, bool d4, bool d5, bool d6, bool d7)
        {
            periodicidade = new Periodicidade();

            InitializeComponent();
            PopulateTimePickers();
            this.WindowState = WindowState.Maximized;

            initialStartDate = startDate;
            initialEndDate = endDate;
            
            tbId.Text = id.ToString();
            dtInicio.Text = startDate.ToString();
            dtFim.Text = endDate.ToString();
            cbPeriodicidade.SelectedValue = selectedPeriodicidade;
            cbHoraInicial.SelectedItem = hora_inicio;
            cbHoraFinal.SelectedItem = hora_fim;
            cbMinInicial.SelectedItem = min_inicio;
            cbMinFinal.SelectedItem = min_fim;
            cbTipo.Text = selectedTipo;
            cbSegunda.IsChecked = d1;
            cbTerca.IsChecked = d2;
            cbQuarta.IsChecked = d3;
            cbQuinta.IsChecked = d4;
            cbSexta.IsChecked = d5;
            cbSabado.IsChecked = d6;
            cbDomingo.IsChecked = d7;
        }

        private void cbTipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Obtém o item selecionado na ComboBox
            string itemSelecionado = ((ComboBoxItem)cbTipo.SelectedItem).Content.ToString();

            // Executa a ação correspondente ao item selecionado
            switch (itemSelecionado)
            {
                case "Fixa":
                    // Ação a ser executada quando "Todos os dias" for selecionado
                    // Oculta o GroupBox se "Todos os dias" for selecionado
                    LidarFixa();
                    string newTipo = ((ComboBoxItem)cbTipo.SelectedItem)?.Content.ToString();
                    break;
                case "Personalizada":
                    // Chama o método para lidar com o evento "Personalizada"
                    LidarPersonalizada();
                    string newTipo_ = ((ComboBoxItem)cbTipo.SelectedItem)?.Content.ToString();
                    break;
                default:
                    break;
            }
        }

        // Método para lidar com o evento "Personalizada"
        private void LidarPersonalizada()
        {
            // Exibe o GroupBox se "Personalizada" for selecionado
            gbDiasSemana.Visibility = Visibility.Visible;
            cal.Visibility = Visibility.Collapsed;
        }

        private void LidarFixa()
        {
            gbDiasSemana.Visibility = Visibility.Collapsed;
            cal.Visibility = Visibility.Visible;
        }

        private void imgConfirmar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(cbTipo.SelectedItem != null && cbTipo.SelectedItem != null && cbHoraInicial.SelectedItem != null && cbHoraFinal.SelectedItem != null  && cbMinInicial.SelectedItem != null && cbMinFinal.SelectedItem != null)
            {
                MessageBox.Show("Dados enviados com sucesso!!!");
                MudaPeriodicidade?.Invoke(this, ((ComboBoxItem)cbPeriodicidade.SelectedItem).Content.ToString());
                MudaTipo?.Invoke(this, ((ComboBoxItem)cbTipo.SelectedItem).Content.ToString());
                this.Close();
            }    
            else
                MessageBox.Show("Por favor preencha todos os dados antes de enviar!!!");
        }

        private void PopulateTimePickers()
        {
            for (int i = 0; i < 24; i++)
            {
                cbHoraInicial.Items.Add(i.ToString("D2"));
                cbHoraFinal.Items.Add(i.ToString("D2"));
            }

            for (int i = 0; i < 60; i++)
            {
                cbMinInicial.Items.Add(i.ToString("D2"));
                cbMinFinal.Items.Add(i.ToString("D2"));
            }
        }

        private void cbPeriodicidade_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string newPeriodicidade = ((ComboBoxItem)cbPeriodicidade.SelectedItem)?.Content.ToString();
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
            Window_Tarefas window_Tarefas   = new Window_Tarefas();
            window_Tarefas.Show();  
            this.Close();   
        }  

        private void cbSegunda_Click(object sender, RoutedEventArgs e)
        {
            bool newd1 = cbSegunda.IsChecked ?? false;
            MudaSegunda?.Invoke(this, newd1);
        }

        private void cbTerca_Click(object sender, RoutedEventArgs e)
        {
            bool newd2 = cbTerca.IsChecked ?? false;
            MudaTerca?.Invoke(this, newd2);
        }

        private void cbQuarta_Click(object sender, RoutedEventArgs e)
        {
            bool newd3 = cbQuarta.IsChecked ?? false;
            MudaQuarta?.Invoke(this, newd3);
        }

        private void cbQuinta_Click(object sender, RoutedEventArgs e)
        {
            bool newd4 = cbQuinta.IsChecked ?? false;
            MudaQuinta?.Invoke(this, newd4);
        }

        private void cbSexta_Click(object sender, RoutedEventArgs e)
        {
            bool newd5 = cbSexta.IsChecked ?? false;
            MudaSexta?.Invoke(this, newd5);
        }

        private void cbSabado_Click(object sender, RoutedEventArgs e)
        {
            bool newd6 = cbSabado.IsChecked ?? false;
            MudaSabado?.Invoke(this, newd6);
        }

        private void cbDomingo_Click(object sender, RoutedEventArgs e)
        {
            bool newd7 = cbDomingo.IsChecked ?? false;
            MudaDomingo?.Invoke(this, newd7);
        }
    }
}
