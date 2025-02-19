using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Utad.Lab.PL4.G01
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized; // full screen
        }

        private void botao_iniciar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) 
        {
            Window_perfil window_perfil = new Window_perfil();
            window_perfil.Show();
            this.Close();
        }

        private void menu_perfil_Click(object sender, RoutedEventArgs e)
        {
            Window_perfil window_perfil = new Window_perfil();
            window_perfil.Show();
            this.Close();
        }

        private void btn_exit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Tem a certeza que quer fechar a aplicação?", "Fechar Aplicação", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void menu_tarefas_Click(object sender, RoutedEventArgs e)
        {
            Window_Tarefas window_tarefas = new Window_Tarefas();
            window_tarefas.Show();
            this.Close();
        }
    }
}
