using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Utad.Lab.PL4.G01.Classes
{
    public class Periodicidade
    {
        public int Id { get; set; }
        public string Tipo { get; set; } // fixa ou semanal
        public string DiasSemana { get; set; }
    }
}
