using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;
using System.Windows.Media.Imaging;
using System.Windows.Documents;
using System.Collections.ObjectModel;
using System.Windows;

namespace Utad.Lab.PL4.G01.Classes
{
    public class Model_ficheiro
    {
        public Perfil MeuPerfilFoto { get; set; }= new Perfil(); // armazena o perfil do utilizador
        public ObservableCollection<Tarefa> Tarefas {  get; set; } //coleciona de todas as tarefas
        public ObservableCollection<Alerta> Alertas { get; set; } //lista de alertas
        public ObservableCollection<Periodicidade> Periodicidades { get; set; }

        //declaração de eventos
        public event MetodosSemParametros PerfilFotoCarregada;
        public event MetodosSemParametros PerfilFotoGuardada;

        public Model_ficheiro()
        {
            Tarefas = new ObservableCollection<Tarefa>();
            Alertas = new ObservableCollection<Alerta>();
            Periodicidades = new ObservableCollection<Periodicidade>();
        }
    }
}
