using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class Window_editar_perfil : Window
    {
        private App app;
        public Window_editar_perfil()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            app = App.Current as App;

            // Carregar e exibir dados do perfil
            if (app.MeuPerfil != null)
            {
                textbox_editar_nome.Text = app.MeuPerfil.Nome;
                textbox_editar_email.Text = app.MeuPerfil.Email;
                if (app.MeuPerfil.Fotografia != null)
                {
                    Imagem_editar_fotografia.Source = app.MeuPerfil.Fotografia;
                }
            }
        }

        public Window_editar_perfil(BitmapImage bitmap) : this()
        {
            if(bitmap != null) 
            {
                Imagem_editar_fotografia.Source = bitmap;
            }
        }

        private void botao_editar_nome_Click(object sender, RoutedEventArgs e)
        {
            textbox_editar_nome.IsEnabled = true;
        }

        private void botao_editar_email_Click(object sender, RoutedEventArgs e)
        {
            textbox_editar_email.IsEnabled = true;  
        }

        private void botao_editar_fotografia_Click(object sender, RoutedEventArgs e)
        {
            Window_editar_fotografia window_Editar_Fotografia = new Window_editar_fotografia();
            window_Editar_Fotografia.ShowDialog();

            if (window_Editar_Fotografia.DialogResult == true)
            {
                Imagem_editar_fotografia.Source = window_Editar_Fotografia.Bitmap;
            }
        }

        private void textbox_editar_nome_TextChanged(object sender, TextChangedEventArgs e)
        {
            App.nome = textbox_editar_nome.Text;
        }

        private void menu_inicio_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void menu_perfil_Click(object sender, RoutedEventArgs e)
        {
            Window_perfil window_perfil = new Window_perfil();
            window_perfil.Show();
            this.Close();
        }

        private void botao_guardar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string text = textbox_editar_nome.Text;
            string email = textbox_editar_email.Text;
            BitmapImage bitmap = Imagem_editar_fotografia.Source as BitmapImage;

            // Validação do email
            if (!ValidarEmail(email))
            {
                MessageBox.Show("O email inserido não é válido.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Encerra a operação se o email não for válido
            }
            app.MeuPerfil.Nome = text;
            app.MeuPerfil.Email = email;
            app.MeuPerfil.Fotografia = bitmap;

            Window_perfil window_Perfil = new Window_perfil(text, email, bitmap);
            window_Perfil.Show();
            this.Close();
        }

        // Método para validar o formato do email usando expressão regular
        private bool ValidarEmail(string email)
        {
            // Expressão regular para validar o formato do email
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            // Verifica se o email corresponde ao padrão especificado
            return Regex.IsMatch(email, pattern);
        }

        public string GetEmail()
        {
            return textbox_editar_email.Text;
        }

        private void menu_tarefas_Click(object sender, RoutedEventArgs e)
        {
            Window_Tarefas window_Tarefas = new Window_Tarefas();
            window_Tarefas.Show();
            this.Close();
        }
    }
}
