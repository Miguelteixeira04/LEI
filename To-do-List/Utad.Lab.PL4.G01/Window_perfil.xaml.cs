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

namespace Utad.Lab.PL4.G01
{
    public partial class Window_perfil : Window
    {
        private App app; //criar apontador para a a class app (camada de interligação)

        public Window_perfil()
        {
            InitializeComponent();
            app = App.Current as App; //obtem apontador para a app (camada de interligação)
            this.WindowState = WindowState.Maximized;

            // Carregar e exibir dados do perfil
            if (app.MeuPerfil != null)
            {
                textbox_nome.Text = app.MeuPerfil.Nome;
                if (app.MeuPerfil.Fotografia != null)
                {
                    Imagem_editarfoto.Source = app.MeuPerfil.Fotografia;
                }
            }

            lv_printtarefas.ItemsSource = app.Classes.Tarefas; // atualiza o estado na ListView
        }

        public Window_perfil(string text, string email, BitmapImage bitmap) : this()
        {
            textbox_nome.Text = text; //coloca o nome da pagina Editar_perfil na pagina perfil
            if (bitmap != null)
            {
                Imagem_editarfoto.Source = bitmap; // Define a imagem no controle Image
            }
        }

        private void botao_editar_perfil_Click(object sender, RoutedEventArgs e)
        {
            Window_editar_perfil window_Editar_Perfil=new Window_editar_perfil();
            window_Editar_Perfil.Show();
            this.Close();
        }

        private void menu_inicio_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void botaoTarefas_Click(object sender, RoutedEventArgs e)
        {
            Window_Tarefas window_Tarefas = new Window_Tarefas();
            window_Tarefas.Show();
            this.Close();
        }

        private void menu_tarefas_Click(object sender, RoutedEventArgs e)
        {
            Window_Tarefas window_tarefas = new Window_Tarefas();
            window_tarefas.Show();
            this.Close();
        }        
    }
}
