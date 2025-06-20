using Independiente.ViewModel;
using Independiente.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Independiente.View
{
  
    public partial class Login : Window
    {
        private readonly LoginViewModel _loginViewModel;

        public Login()
        {
            InitializeComponent();
            _loginViewModel = new LoginViewModel();
            this.DataContext = _loginViewModel;
            _loginViewModel.RequestClose += ViewModel_RequestClose;
            this.PasswordPasswordBox.PasswordChanged += PasswordPasswordBox_PasswordChanged;
        }

        private void PasswordPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                var placeholder = (TextBlock)passwordBox.Template.FindName("PlaceholderText", passwordBox);
                if (placeholder != null)
                {
                    placeholder.Visibility = string.IsNullOrEmpty(passwordBox.Password) ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            if (DataContext is LoginViewModel vm)
            {
                vm.Password = PasswordPasswordBox.Password;
            }
        }

        private void ViewModel_RequestClose(object sender, EventArgs e)
        {
            if (_loginViewModel.IsLoginSuccessful)
            {
                var main = new MainWindow();
                main.Show();
                this.Close();
            }
        }
    }
}
