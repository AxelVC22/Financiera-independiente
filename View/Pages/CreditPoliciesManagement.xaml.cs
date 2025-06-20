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
  
    public partial class CreditPoliciesManagement : Page
    {

        private CreditPoliciesManagementViewModel _creditPoliciesManagement;

        public CreditPoliciesManagement() { }
        public CreditPoliciesManagement(CreditPoliciesManagementViewModel creditPoliciesManagement)
        {
            InitializeComponent();

            _creditPoliciesManagement = creditPoliciesManagement;

            this. DataContext = _creditPoliciesManagement;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ListView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
