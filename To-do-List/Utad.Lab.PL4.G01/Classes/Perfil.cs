using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace Utad.Lab.PL4.G01.Classes
{
    public class Perfil
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Ficheiro { get; set; }
        public BitmapImage Fotografia { get; set; }

        // Método para converter perfil em XML
        public XElement ToXML()
        {
            XElement no = new XElement("perfil",
                new XElement ("Nome",Nome),
                new XElement ("Email", Email),
                new XElement ("Ficheiro", Ficheiro)
            );

            if (Fotografia != null)
            {
                byte[] imageData;
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(Fotografia));
                using (MemoryStream stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    imageData = stream.ToArray();
                }
                no.Add(new XElement("Fotografia", Convert.ToBase64String(imageData)));
            }
            return no;
        }

        // Método para converter XML em perfil
        public static Perfil FromXML(XElement no)
        {
            Perfil perfil = new Perfil
            {
                Nome = no.Element("Nome")?.Value,
                Email = no.Element("Email")?.Value,
                Ficheiro = no.Element("Ficheiro")?.Value
            };

            string imageDataString = no.Element("Fotografia")?.Value;
            if (!string.IsNullOrEmpty(imageDataString))
            {
                byte[] imageData = Convert.FromBase64String(imageDataString);
                BitmapImage bitmap = new BitmapImage();
                using (MemoryStream stream = new MemoryStream(imageData))
                {
                    bitmap.BeginInit();
                    bitmap.StreamSource = stream;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                }
                perfil.Fotografia = bitmap;
            }
            return perfil;
        }
    }
}
