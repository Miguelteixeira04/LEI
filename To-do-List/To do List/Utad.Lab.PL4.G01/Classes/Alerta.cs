using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Utad.Lab.PL4.G01.Classes
{
    public class Alerta
    {
        public int Id { get; set; }
        public string Mensagem { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public TimeSpan HoraInicial { get; set; }
        public TimeSpan HoraFinal { get; set; }
        public string TipoEnvio { get; set; }
        public string TipoAlerta { get; set; }
        public bool Estado { get; set; }

        public XElement ToXML()
        {
            XElement no = new XElement("alerta",
                new XElement("Id", Id),
                new XElement("Mensagem", Mensagem),
                new XElement("TipoAlerta", TipoAlerta),
                new XElement("TipoEnvio", TipoEnvio),
                new XElement("Estado", Estado)
            );

            if (TipoAlerta == "Antecipação")
            {
                no.Add(new XElement("DataAlarme", DataInicial.ToString("dd-MM-yyyy")),
                    new XElement("HoraAlarme", HoraInicial.ToString(@"hh\:mm")));
            }
            else if (TipoAlerta == "Execução")
            {
                no.Add(new XElement("DataAlarme", DataFinal.ToString("dd-MM-yyyy")),
                    new XElement("HoraAlarme", HoraFinal.ToString(@"hh\:mm")));
            }
            else if (TipoAlerta == "Antecipação e Execução")
            {
                no.Add(new XElement("DataInicial", DataInicial.ToString("dd-MM-yyyy")),
                    new XElement("DataFinal", DataFinal.ToString("dd-MM-yyyy")),
                    new XElement("HoraInicial", HoraInicial.ToString(@"hh\:mm")),
                    new XElement("HoraFinal", HoraFinal.ToString(@"hh\:mm")));
            }
            return no;
        }

        public static Alerta FromXML(XElement no)
        {
            Alerta alerta = new Alerta
            {
                Id = int.Parse(no.Element("Id").Value),
                Mensagem = no.Element("Mensagem").Value,
                TipoAlerta = no.Element("TipoAlerta").Value,
                TipoEnvio = no.Element("TipoEnvio").Value,
                Estado = bool.Parse(no.Element("Estado").Value)
            };

            if (no.Element("DataAlarme") != null && no.Element("HoraAlarme") != null)
            {
                DateTime dataTemp;
                TimeSpan horaTemp;

                if (alerta.TipoAlerta == "Antecipação")
                {
                    if (DateTime.TryParse(no.Element("DataAlarme").Value, out dataTemp))
                        alerta.DataInicial = dataTemp;

                    if (TimeSpan.TryParse(no.Element("HoraAlarme").Value, out horaTemp))
                        alerta.HoraInicial = horaTemp;
                }
                else if (alerta.TipoAlerta == "Execução")
                {
                    if (DateTime.TryParse(no.Element("DataAlarme").Value, out dataTemp))
                        alerta.DataFinal = dataTemp;

                    if (TimeSpan.TryParse(no.Element("HoraAlarme").Value, out horaTemp))
                        alerta.HoraFinal = horaTemp;
                }
                else if (alerta.TipoAlerta == "Antecipação e Execução")
                {
                    DateTime dataInicialTemp, dataFinalTemp;
                    TimeSpan horaInicialTemp, horaFinalTemp;

                    if (DateTime.TryParse(no.Element("DataInicial").Value, out dataInicialTemp))
                        alerta.DataInicial = dataInicialTemp;

                    if (DateTime.TryParse(no.Element("DataFinal").Value, out dataFinalTemp))
                        alerta.DataFinal = dataFinalTemp;

                    if (TimeSpan.TryParse(no.Element("HoraInicial").Value, out horaInicialTemp))
                        alerta.HoraInicial = horaInicialTemp;

                    if (TimeSpan.TryParse(no.Element("HoraFinal").Value, out horaFinalTemp))
                        alerta.HoraFinal = horaFinalTemp;
                }
            }
            return alerta;
        }
    }
}
