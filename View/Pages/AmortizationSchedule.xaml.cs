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
    /// Lógica de interacción para AmortizationSchedule.xaml
    /// </summary>
    public partial class AmortizationSchedule : Page
    {
        private AmortizationScheduleViewModel _amortizationScheduleViewModel;
        public AmortizationSchedule(AmortizationScheduleViewModel amortizationSchedule)
        {
            InitializeComponent();

            _amortizationScheduleViewModel = amortizationSchedule;
            this.DataContext = _amortizationScheduleViewModel;
        }
    }
}
