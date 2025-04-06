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
        bool Confirm(string message);

        void Dismiss(string message);
    }

    public class DialogService : IDialogService
    {
        public bool Confirm(string message)
        {
            return MessageBox.Show(message, "", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }
        public void Dismiss(string message)
        {
            MessageBox.Show(message, "", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

}