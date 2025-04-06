using Independiente.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.ViewModel
{
    public class CreditPoliciesManagementViewModel : ModificableViewModel
    {
        public IDialogService _dialogService;

        public INavigationService _navigationService;

      //  public List<>
        public CreditPoliciesManagementViewModel(IDialogService dialogService, INavigationService navigationService, PageMode mode)
        {

        }
    }
}
