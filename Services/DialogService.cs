using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Independiente.Services
{
    public interface IDialogService
    {
        bool Confirm(string mensaje);

        void Dismiss(string mensaje);
    }

    public class DialogService : IDialogService
    {
        public bool Confirm(string mensaje)
        {
            return MessageBox.Show(mensaje, "", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }
        public void Dismiss(string mensaje)
        {
            MessageBox.Show(mensaje, "", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

}