using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Independiente.Converters
{
    public class CreditApplicationStateToSpanishConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CreditApplicationStates state)
            {
                switch (state)
                {
                    case CreditApplicationStates.Pending:
                        return "Pendiente";
                    case CreditApplicationStates.Accepted:
                        return "Aceptada";
                    case CreditApplicationStates.Rejected:
                        return "Rechazada";
                    default:
                        return "Desconocido";
                }
            }

            return "Desconocido";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // Solo lectura desde el ViewModel
        }
    }
}
