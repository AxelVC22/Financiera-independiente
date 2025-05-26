using Independiente.Commands;
using Independiente.Model;
using Independiente.Properties;
using Independiente.Services;
using Independiente.View.Pages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Independiente.ViewModel
{
    public class FinancialDataViewModel : ModificableViewModel
    {
        public ObservableCollection<Bank> BanksList { get; set; }
       
        private PageMode _pageMode;

        private INavigationService _navigationService { get; set; }
        private IDialogService _dialogService { get; set; }

        private IClientManagementService _clientManagementService { get; set; }

        private ICatalogService _catalogService { get; set; }


        public Account ChargeAccount { get; set; }

        public Account DepositAccount { get; set; }

        public Client Client { get; set; }
        public FinancialDataViewModel()
        {

        }
       

        private void GoBack(object obj)
        {
            _navigationService.GoBack();
        }

        public FinancialDataViewModel(IDialogService dialogService, INavigationService navigationService, PageMode mode, IClientManagementService clientManagementService, ICatalogService catalogManagerService, Client client)
        {
            NextCommand = new RelayCommand(Next, CanNext);
            EditCommand = new RelayCommand(Edit, CanNext);
            CancelCommand = new RelayCommand(Cancel, CanNext);
            SaveCommand = new RelayCommand(Save, CanNext);
            GoBackCommand = new RelayCommand(GoBack, CanNext);
            _navigationService = navigationService;
            _dialogService = dialogService;
            SwitchMode(mode);
            _pageMode = mode;
            ChargeAccount = new Account();
            DepositAccount = new Account();
            _clientManagementService = clientManagementService;
            _catalogService = catalogManagerService;
            BanksList = new ObservableCollection<Bank>(_catalogService.GetBanks(new DataAccess.Repositories.CatalogQuery()));
            Client = client;
        }
        private void Next(object obj)
        {
            string message = string.Empty;
            bool validation = false;

            try
            {
                if (FieldValidator.IsValidCLABE(ChargeAccount.CLABE) && FieldValidator.IsValidCLABE(DepositAccount.CLABE))
                {
                    if (ChargeAccount == null || DepositAccount == null)
                    {
                        message = Messages.NoBankSelectedMessage;
                    }
                    else
                    {
                        validation = true;
                    }
                }
            }
            catch (ArgumentException exception)
            {
                message = exception.Message;
            }

            if (validation)
            {
                Client.PaymentAccount = ChargeAccount;
                Client.DepositAccount = DepositAccount;
                _navigationService.NavigateTo<ReferencesViewModel>(new ClientDataParams(_pageMode, Client));
            } 
            else
            {
                _dialogService.Dismiss(message, MessageBoxImage.Exclamation);
            }
        }

        private void Cancel(object obj)
        {
            SwitchMode(PageMode.View);
        }

        private void Save(object obj)
        {
            Console.WriteLine(ChargeAccount.ToString());
            Console.WriteLine(DepositAccount.ToString());
            SwitchMode(PageMode.View);
        }

        private void Edit(object obj)
        {
            Console.WriteLine(ChargeAccount.ToString());
            Console.WriteLine(DepositAccount.ToString());

            SwitchMode(PageMode.Update);
        }

        private bool CanNext(object obj)
        {
            return true;
        }

        
    }
}
