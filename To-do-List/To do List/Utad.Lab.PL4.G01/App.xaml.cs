using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using Utad.Lab.PL4.G01.Classes;

namespace Utad.Lab.PL4.G01
{
    public partial class App : Application
    {
        public Model_ficheiro Classes { get; private set; }
        public static string nome { get; internal set; }
        public Perfil MeuPerfil { get; private set; }
        public List<Tarefa> Tarefas { get; private set; }
        public static int NextTaskId = 0;
        public List<Alerta> Alertas { get; private set; }

        public App()
        {
            Classes = new Model_ficheiro(); // inicializar a instancia da classe model (model_ficheiro)
            MeuPerfil = CarregarPerfil();

            Classes.Tarefas = new ObservableCollection<Tarefa>(CarregarTarefas());
            Classes.Alertas = new ObservableCollection<Alerta>(CarregarAlertas());

            // define o NextTaskId de acordo com os Ids das tarefas que já existem na lista
            if (Classes.Tarefas.Any())
            {
                NextTaskId = 1;
                foreach (Tarefa tarefa in Classes.Tarefas)
                {
                    if (tarefa.Id >= NextTaskId)
                        NextTaskId = tarefa.Id + 1;
                }
            }
            else
                NextTaskId = 1; // se não houver tarefas na lista, começa no 1

        }
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            GuardarPerfil(MeuPerfil);
            GuardarTarefas(new List<Tarefa>(Classes.Tarefas));
            GuardarAlertas(new List<Alerta>(Classes.Alertas));
        }

        // Metodo para carregar perfil do utilizador
        private Perfil CarregarPerfil()
        {
            try
            {
                if (File.Exists("dadosPerfil.xml"))
                {
                    XElement perfilXML = XElement.Load("dadosPerfil.xml");
                    return Perfil.FromXML(perfilXML);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar o perfil: " + ex.Message);
            }
            return new Perfil();
        }

        // Metodo para guardar perfil do utilizador
        private void GuardarPerfil(Perfil perfil)
        {
            try
            {
                XElement perfilXML = perfil.ToXML();
                perfilXML.Save("dadosPerfil.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar o perfil: " + ex.Message);
            }
        }

        // Metodo para carregar tarefas
        private List<Tarefa> CarregarTarefas()
        {
            try
            {
                if (File.Exists("dadosTarefas.xml"))
                {
                    XElement tarefasXML = XElement.Load("dadosTarefas.xml");
                    return tarefasXML.Elements("tarefa").Select(Tarefa.FromXML).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar a lista de tarefas: " + ex.Message);
            }
            return new List<Tarefa>();
        }

        // Metodo para guardar tarefas
        private void GuardarTarefas(List<Tarefa> tarefas)
        {
            try
            {
                XElement tarefasXML = new XElement("tarefas");
                tarefasXML.Add(new XElement("NextTaskId", App.NextTaskId)); // Salva o NextTaskId

                foreach (Tarefa tarefa in tarefas)
                {
                    tarefasXML.Add(tarefa.ToXML());
                }
                tarefasXML.Save("dadosTarefas.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar a lista de tarefas: " + ex.Message);
            }
        }

        // Metodo para carregar alertas
        public List<Alerta> CarregarAlertas()
        {
            List<Alerta> alertas = new List<Alerta>();
            try
            {
                if (File.Exists("dadosAlertas.xml"))
                {
                    XElement alertasXML = XElement.Load("dadosAlertas.xml");
                    return alertasXML.Elements("alerta").Select(Alerta.FromXML).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar a lista de alertas: " + ex.Message);
            }
            return new List<Alerta>();
        }

        // Método para guardar alertas
        public void GuardarAlertas(List<Alerta> alertas)
        {
            try
            {
                XElement alertasXML = new XElement("alertas");

                // Filtra os alertas que estão associados às tarefas existentes
                foreach (Alerta alerta in alertas.Where(a => Classes.Tarefas.Any(t => t.Id == a.Id)))
                {
                    XElement alertaExistente = alertasXML.Elements("alerta").FirstOrDefault(e => e.Element("Id").Value == alerta.Id.ToString());
                    if (alertaExistente != null)
                    {
                        alertaExistente.ReplaceWith(alerta.ToXML());
                    }
                    else
                    {
                        alertasXML.Add(alerta.ToXML());
                    }
                }

                alertasXML.Save("dadosAlertas.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar a lista de alertas: " + ex.Message);
            }
        }
    }
}