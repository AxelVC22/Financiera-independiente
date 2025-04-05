using Independiente.Services;
using Independiente.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class PersonalData : Page
    {
        PersonalDataViewModel _personalDataViewModel;
        public PersonalData(PersonalDataViewModel personalDataViewModel)
        {
            InitializeComponent();


            _personalDataViewModel = personalDataViewModel;
           
            this.DataContext = _personalDataViewModel;
        }
      

       



    }
}
