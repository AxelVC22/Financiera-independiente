using Independiente.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Independiente.View.Pages
{
    /// <summary>
    /// Lógica de interacción para UserRegistration.xaml
    /// </summary>
    public partial class UserRegistration : Page
    {

        private UserRegistrationViewModel _userRegistrationViewModel;
        public UserRegistration(UserRegistrationViewModel userRegistrationViewModel)
        {
            InitializeComponent();

            _userRegistrationViewModel = userRegistrationViewModel;
            this.DataContext = _userRegistrationViewModel;
            this.PasswordPasswordBox.PasswordChanged += PasswordPasswordBox_PasswordChanged;
            this.ConfirmationPasswordPasswordBox.PasswordChanged += PasswordPasswordBox_PasswordChanged;
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
            if (DataContext is UserRegistrationViewModel vm)
            {
                
                vm.Password = PasswordPasswordBox.Password;
            }
        }
    }
}
