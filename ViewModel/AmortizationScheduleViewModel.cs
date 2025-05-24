using Independiente.Commands;
using Independiente.DataAccess.Repositories;
using Independiente.Model;
using Independiente.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Independiente.ViewModel
{
    public class AmortizationScheduleViewModel : BaseViewModel
    {
        private IAmortizationScheduleService _amortizationScheduleService;

        private IDialogService _dialogService;

        private INavigationService _navigationService;

        public CreditApplication CreditApplication { get; set; } = new CreditApplication();
        public ObservableCollection<AmortizationSchedule> AmortizationSchedules { get; set; } = new ObservableCollection<AmortizationSchedule>();
        public AmortizationScheduleViewModel() { }

        public AmortizationScheduleViewModel(INavigationService navigationService, IDialogService dialogService, IAmortizationScheduleService amortizationScheduleService, CreditApplication creditApplication)
        {
            _amortizationScheduleService = amortizationScheduleService;
            _dialogService = dialogService;
            _navigationService = navigationService;
            CreditApplication = creditApplication;
            LoadAmortizationSchedules();
            GoBackCommand = new RelayCommand(GoBack, CanDoIt);
        }

        private bool CanDoIt(object obj)
        {
            return true;
        }

        private void GoBack(object obj)
        {
            _navigationService.GoBack();
        }
        private void LoadAmortizationSchedules()
        {
            var query = new AmortizationScheduleQuery
            {
                CreditApplicaitonId = CreditApplication.CreditApplicationId
            };

            var amortizationSchedules = _amortizationScheduleService.GetAmortizationSchedule(query);
            AmortizationSchedules.Clear();
            foreach (var amortizationSchedule in amortizationSchedules)
            {
                AmortizationSchedules.Add(amortizationSchedule);
            }


        }
    }
}
