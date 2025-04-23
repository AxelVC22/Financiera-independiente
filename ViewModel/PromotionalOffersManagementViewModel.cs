using Independiente.Commands;
using Independiente.Model;
using Independiente.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Independiente.ViewModel
{
    public class PromotionalOffersManagementViewModel : ModificableViewModel
    {
        public IDialogService _dialogService;

        public INavigationService _navigationService;

        public ICommand RegisterCommand { get; set; }

        public List<PromotionalOfferState> StatesList { get; set; }

        public ObservableCollection<PromotionalOffer> PromotionalOffersList { get; set; }

        public PromotionalOffersManagementViewModel() { }
        public PromotionalOffersManagementViewModel(IDialogService dialogService, INavigationService navigationService)
        {
            PromotionalOffersList = new ObservableCollection<PromotionalOffer>
            {
                new PromotionalOffer { Description = "Cliente frecuente", InteresRate=1, IVA=1, LoanTerm=2, PaymenteFrecuency = "Mensual", Status=PromotionalOfferState.Active},
              
            };
            _dialogService = dialogService;
            _navigationService = navigationService;
            EditCommand = new RelayCommand(Edit, CanDoIt);
            CancelCommand = new RelayCommand(Cancel, CanDoIt);
            SaveCommand = new RelayCommand(Save, CanDoIt);
            RegisterCommand = new RelayCommand(Register, CanDoIt);
            StatesList = Enum.GetValues(typeof(PromotionalOfferState)).Cast<PromotionalOfferState>().ToList();

        }

        private void Edit(object obj)
        {
            if (obj is PromotionalOffer promotionalOffer)
            {
                promotionalOffer.IsEditable = true;
            }
        }

        private void Cancel(object obj)
        {
            if (obj is PromotionalOffer promotionalOffer)
            {
                promotionalOffer.IsEditable = false;
            }
        }

        private void Save(object obj)
        {
            if (obj is PromotionalOffer promotionalOffer)
            {
                promotionalOffer.IsEditable = false;
            }
        }

        private bool CanDoIt(object obj)
        {
            return true;
        }

        private void Register(object obj)
        {
            PromotionalOffersList.Add(new PromotionalOffer { IsEditable = true });
        }
    }
}
