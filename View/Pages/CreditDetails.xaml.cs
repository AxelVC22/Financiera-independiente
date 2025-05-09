﻿using Independiente.ViewModel;
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
    /// Lógica de interacción para CreditDetails.xaml
    /// </summary>
    public partial class CreditDetails : Page
    {
        CreditDetailsViewModel _creditDetailsViewModel;
        public CreditDetails(CreditDetailsViewModel creditDetailsViewModel)
        {
            InitializeComponent();

            _creditDetailsViewModel = creditDetailsViewModel;

            this.DataContext = _creditDetailsViewModel;
            
        }

        private void ValidateDecimal(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !decimal.TryParse(e.Text, out _);
        }

        private void ValidatePasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var text = (string)e.DataObject.GetData(typeof(string));
                if (!decimal.TryParse(text, out _))
                {
                    e.CancelCommand();
                }
            }
        }

    }
}
