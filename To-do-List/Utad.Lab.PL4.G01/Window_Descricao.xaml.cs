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
    public partial class Window_Descricao : Window
    {
        private Window_Tarefas window_Tarefas;
        public string Descricao {  get; set; }
        public Window_Descricao() {}
        public Window_Descricao(Window_Tarefas windowTarefas, string descricao)
        {
            Descricao = descricao;
            InitializeComponent();
            DataContext = this; // define o contexto dos dados para esta instância
            window_Tarefas = windowTarefas;
        }

        private void btn_check_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Restaurar a opacidade da janela Window_Tarefas
            if (window_Tarefas != null)
            {
                window_Tarefas.Opacity = 1.0;
            }

            DialogResult = true;
            this.Close();
        }
    }
}
