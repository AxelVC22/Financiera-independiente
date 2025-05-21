using Independiente.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.ViewModel
{
    public class PaymentsViewModel
    {
        private IDialogService _dialogService;
        private INavigationService _navigationService;
        private IPaymentService _paymentService;

        public PaymentsViewModel()
        {
        }

        public PaymentsViewModel(IDialogService dialogService, INavigationService navigationService, IPaymentService paymentService)
        {
            _dialogService = dialogService;
            _navigationService = navigationService;
            _paymentService = paymentService;
        }
    }
}
