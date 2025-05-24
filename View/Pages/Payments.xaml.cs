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

    public partial class Payments : Page
    {

        private PaymentsViewModel _paymentsViewModel;
        public Payments(PaymentsViewModel paymentsViewModel)
        {
            InitializeComponent();

            _paymentsViewModel = paymentsViewModel;
            this.DataContext = _paymentsViewModel;
        }
    }
}
