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
            switch ((string)value)
            {
                case "Comedia":
                    return "imagenes/comedia.png";
                case "Drama":
                    return "imagenes/drama.png";
                case "Acción":
                    return "imagenes/accion.png";
                case "Terror":
                    return "imagenes/terror.png";
                case "CienciaFicción":
                    return "imagenes/cienciaficcion.png";
                default:
                    return "imagenes/icono.ico";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
