using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace Utad.Lab.PL4.G01
{ 
    public partial class Window_editar_fotografia : Window
    {
        //criar apontador para a a class app (camada de interligação)
        private App app;
        public BitmapImage Bitmap { get; private set; }

        public Window_editar_fotografia()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            app = App.Current as App; // obtem apontador para a app (camada de interligação)

            //subscrição de métodos da view em evento do Model
            app.Classes.PerfilFotoCarregada += Classes_PerfilFotoCarregada;
            app.Classes.PerfilFotoGuardada += Classes_PerfilFotoGuardada;
        }

        private void Classes_PerfilFotoGuardada()
        {
            MessageBox.Show("Fotografia atualizada com sucesso!", "Fotografia", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Classes_PerfilFotoCarregada()
        {
            imgFotografia.Source = app.Classes.MeuPerfilFoto.Fotografia;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //abre ambiente de trabalho
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "Ficheiros de imagem (*.png;*.jpg)|*.png;*.jpg|Todos os ficheiros (*.*)|*.*"
            };

            if (dlg.ShowDialog() == true)
            {               
                Bitmap = new BitmapImage(new Uri(dlg.FileName));
                imgFotografia.Source = Bitmap;
            }
        }

        private void menu_perfil_Click(object sender, RoutedEventArgs e)
        {
            Window_perfil window_perfil = new Window_perfil();
            window_perfil.Show();
            this.Close();
        }

        private void menu_inicio_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void botao_guardar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DialogResult = true;
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
