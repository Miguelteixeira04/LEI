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

namespace Utad.Lab.PL4.G01
{
    public partial class Window_FiltroTarefas : Window
    {
        private Window_Tarefas window_Tarefas;
        public App app;

        public DateTime? DataInicio { get; private set; }
        public DateTime? DataFim { get; private set; }
        public DateTime? Dia { get; private set; }
        public DateTime? PrimeiroDiaSemana { get; private set; }
        public DateTime? UltimoDiaSemana { get; private set; }

        public Window_FiltroTarefas() {}
        public Window_FiltroTarefas(Window_Tarefas windowTarefas)
        {
            InitializeComponent();
            app = App.Current as App;
            window_Tarefas = windowTarefas;
        }
               
        private void rbtn_pordia_Checked(object sender, RoutedEventArgs e)
        {
            data_dia.IsEnabled = true;
            data_1dia.IsEnabled = false;
            data_primeirodia.IsEnabled = false;
            data_ultimodia.IsEnabled = false;
        }

        private void rbtn_porsemana_Checked(object sender, RoutedEventArgs e)
        {
            data_1dia.IsEnabled = true;
            data_dia.IsEnabled = false;
            data_primeirodia.IsEnabled = false;
            data_ultimodia.IsEnabled = false;
        }

        private void rbtn_entredatas_Checked(object sender, RoutedEventArgs e)
        {
            data_primeirodia.IsEnabled = true;
            data_ultimodia.IsEnabled = true;
            data_1dia.IsEnabled = false;
            data_dia.IsEnabled = false;
        }

        private void data_1dia_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (data_1dia.SelectedDate.HasValue)
            {
                data_7dia.SelectedDate = data_1dia.SelectedDate.Value.AddDays(6); // adicionar 6 dias automaticamente para fazer a semana
            }
        }

        private void btn_restaurar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false; // dá reset ao filtro
            this.Close();

            // restaurar opacidade da janela tarefas
            if (window_Tarefas != null)
                window_Tarefas.Opacity = 1.0;
        }

        private void btn_check_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // filtro por dia
            if (rbtn_pordia.IsChecked == true)
            {
                Dia = data_dia.SelectedDate;
            }

            // filtro 7 dias
            else if (rbtn_porsemana.IsChecked == true)
            {
                PrimeiroDiaSemana = data_1dia.SelectedDate;
                UltimoDiaSemana = data_7dia.SelectedDate;
            }

            // filtro intervalo dias
            else if (rbtn_entredatas.IsChecked == true)
            {
                DataInicio = data_primeirodia.SelectedDate;
                DataFim = data_ultimodia.SelectedDate;
            }

            this.DialogResult = true;
            this.Close();

            // restaurar opacidade da janela tarefas
            if (window_Tarefas != null)
                window_Tarefas.Opacity = 1.0;
        }       
    }
}
