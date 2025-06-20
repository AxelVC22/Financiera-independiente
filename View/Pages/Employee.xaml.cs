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
    /// Lógica de interacción para Employee.xaml
    /// </summary>
    public partial class Employee : Page
    {
        private EmployeeViewModel viewModel;
        public Employee(EmployeeViewModel employeeViewModel)
        {
            InitializeComponent();
            viewModel = employeeViewModel;
            this.DataContext = employeeViewModel;
        }

        private void PasswordPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is EmployeeViewModel vm)
            {
                vm.Employee.User.Password = PasswordPasswordBox.Password;
            }
            if (sender is PasswordBox passwordBox)
            {
                var placeholder = (TextBlock)passwordBox.Template.FindName("PlaceholderText", passwordBox);
                if (placeholder != null)
                {
                    placeholder.Visibility = string.IsNullOrEmpty(passwordBox.Password) ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            if (DataContext is EmployeeViewModel vmm)
            {
                vmm.Password = PasswordPasswordBox.Password;
            }
        }
    }
}
