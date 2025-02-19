using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xml.Linq;

namespace Utad.Lab.PL4.G01.Classes
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }

        // horas e minutos (formato hh:mm)
        public string HoraInicio { get; set; }
        public string HoraFim { get; set; }

        public string NivelImportancia { get; set; } // Pouco importante, Normal, Importante e Prioritária
        public string Estado { get; set; } // por iniciar, em execucao, por terminar
        public string Periodicidade { get; set; }
        public string Alerta { get; set; }

        // horas e minutos separados
        public string HoraInicial { get; set; }
        public string HoraFinal { get; set; }
        public string MinInicial { get; set; }
        public string MinFinal { get; set; }

        public string Tipo { get; set; } // Fixa ou personalizada
        public bool d1 { get; set; }
        public bool d2 { get; set; }
        public bool d3 { get; set; }
        public bool d4 { get; set; }
        public bool d5 { get; set; }
        public bool d6 { get; set; }
        public bool d7 { get; set; }

        // Método para converter tarefa em XML
        public XElement ToXML()
        {
            XElement no = new XElement("tarefa",
                new XElement("Id", Id),
                new XElement("Titulo", Titulo),
                new XElement("Descricao", Descricao),
                new XElement("DataInicio", DataInicio),
                new XElement("DataTermino", DataTermino),
                new XElement("HoraInicio", HoraInicio),
                new XElement("HoraFim", HoraFim),
                new XElement("NivelImportancia", NivelImportancia),
                new XElement("Estado", Estado),
                new XElement("Periodicidade", Periodicidade),
                new XElement("Alerta", Alerta),
                new XElement("HoraInicial", HoraInicial),
                new XElement("HoraFinal", HoraFinal),
                new XElement("MinInicial", MinInicial),
                new XElement("MinFinal", MinFinal),

                new XElement("Tipo", Tipo),
                new XElement("d1", d1),
                new XElement("d2", d2),
                new XElement("d3", d3),
                new XElement("d4", d4),
                new XElement("d5", d5),
                new XElement("d6", d6),
                new XElement("d7", d7)
            );
            return no;
        }

        // Método para converter XML em tarefa
        public static Tarefa FromXML(XElement no)
        {
            Tarefa tarefa = new Tarefa
            {
                Id = int.Parse(no.Element("Id").Value),
                Titulo = no.Element("Titulo").Value,
                Descricao = no.Element("Descricao").Value,
                DataInicio = DateTime.Parse(no.Element("DataInicio").Value),
                DataTermino = DateTime.Parse(no.Element("DataTermino").Value),
                HoraInicio = no.Element("HoraInicio").Value,
                HoraFim = no.Element("HoraFim").Value,
                NivelImportancia = no.Element("NivelImportancia").Value,
                Estado = no.Element("Estado").Value,
                Periodicidade = no.Element("Periodicidade").Value,
                Alerta = no.Element("Alerta").Value,
                HoraInicial = no.Element("HoraInicial").Value,
                HoraFinal = no.Element("HoraFinal").Value,
                MinInicial = no.Element("MinInicial").Value,
                MinFinal = no.Element("MinFinal").Value,

                Tipo = no.Element("Tipo").Value,
                d1 = bool.Parse(no.Element("d1").Value),
                d2 = bool.Parse(no.Element("d2").Value),
                d3 = bool.Parse(no.Element("d3").Value),
                d4 = bool.Parse(no.Element("d4").Value),
                d5 = bool.Parse(no.Element("d5").Value),
                d6 = bool.Parse(no.Element("d6").Value),
                d7 = bool.Parse(no.Element("d7").Value)
            };
            return tarefa;
        }
    }
}
