using Independiente.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Independiente.Converters
{
    public class UserRoleToSpanishConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is UserRole role)
            {
                switch (role)
                {
                    case UserRole.Advisor:
                        return "Asesor";
                    case UserRole.Analyst:
                        return "Analista";
                    case UserRole.Collector:
                        return "Cobrador";
                    case UserRole.Administrator:
                        return "Administrador";
                    default:
                        return role.ToString();
                }
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string roleText = value as string;
            if (roleText != null)
            {
                switch (roleText)
                {
                    case "Asesor":
                        return UserRole.Advisor;
                    case "Analista":
                        return UserRole.Analyst;
                    case "Cobrador":
                        return UserRole.Collector;
                    case "Administrador":
                        return UserRole.Administrator;
                }
            }

            return Binding.DoNothing;
        }
    }
}
