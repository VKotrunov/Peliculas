using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Peliculas
{
    class GeneroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string ruta = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName; 
            switch ((string)value)
            {
                case "Comedia":
                    return Path.Combine(ruta, @"assets/comedia.png");
                case "Drama":
                    return Path.Combine(ruta, @"assets/drama.png");
                case "Acción":
                    return Path.Combine(ruta, @"assets/accion.png");
                case "Terror":
                    return Path.Combine(ruta, @"assets/terror.png");
                case "CienciaFicción":
                    return Path.Combine(ruta, @"assets/cienciaficcion.png");
                default:
                    return "icono.ico";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
