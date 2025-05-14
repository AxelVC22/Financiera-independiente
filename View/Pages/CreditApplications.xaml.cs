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

namespace Independiente.View
{
    /// <summary>
    /// Lógica de interacción para CreditApplications.xaml
    /// </summary>
    public partial class CreditApplications : Page
    {
        private CreditApplicationsViewModel _creditApplicationsViewModel;

        public CreditApplications()
        {
        }
        public CreditApplications(CreditApplicationsViewModel creditApplicationsViewModel)
        {
            
            InitializeComponent();

            _creditApplicationsViewModel = creditApplicationsViewModel;
            this.DataContext = _creditApplicationsViewModel;
        }
    }
}
