using Independiente.Commands;
using Independiente.DataAccess.Repositories;
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
        private Account OldDepositAccount { get; set; }
        private Account OldPaymentAccount { get; set; }
        private Bank OldDepositBank { get; set; }
        private Bank OldChargeBank { get; set; }
        public Account ChargeAccount { get; set; }
        public Account DepositAccount { get; set; }
        public Client Client { get; set; }

        private Bank _depositBank;
        public Bank DepositBank
        {
            get => _depositBank;
            set
            {
                if (_depositBank != value)
                {
                    _depositBank = value;
                    OnPropertyChanged(nameof(DepositBank));
                }
            }
        }

        private Bank _chargeBank;
        public Bank ChargeBank
        {
            get => _chargeBank;
            set
            {
                if (_chargeBank != value)
                {
                    _chargeBank = value;
                    OnPropertyChanged(nameof(ChargeBank));
                }
            }
        }

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

            _clientManagementService = clientManagementService;
            _catalogService = catalogManagerService;

            BanksList = new ObservableCollection<Bank>(_catalogService.GetBanks(new CatalogQuery()));

            Client = client;

            if (client?.DepositAccount != null)
            {
                DepositAccount = client.DepositAccount;
                DepositBank = BanksList.FirstOrDefault(b => b.BankId == client.DepositAccount.Bank);
            }
            else
            {
                DepositAccount = new Account();
            }

            if (client?.PaymentAccount != null)
            {
                ChargeAccount = client.PaymentAccount;
                ChargeBank = BanksList.FirstOrDefault(b => b.BankId == client.PaymentAccount.Bank);
            }
            else
            {
                ChargeAccount = new Account();
            }

            if (_pageMode == PageMode.View)
            {
                OldDepositAccount = CreateAccountCopy(DepositAccount);
                OldPaymentAccount = CreateAccountCopy(ChargeAccount);
                OldDepositBank = DepositBank;
                OldChargeBank = ChargeBank;
            }
        }

        private void Next(object obj)
        {
            string message = string.Empty;
            bool validation = false;

            try
            {
                if (FieldValidator.IsValidCLABE(ChargeAccount.CLABE) && FieldValidator.IsValidCLABE(DepositAccount.CLABE))
                {
                    if (ChargeBank == null || DepositBank == null)
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
                ChargeAccount.Bank = ChargeBank.BankId;
                DepositAccount.Bank = DepositBank.BankId;
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
            if (_dialogService.Confirm("¿Desea cancelar los cambios realizados?"))
            {
                if (OldDepositAccount != null)
                {
                    DepositAccount.CLABE = OldDepositAccount.CLABE;
                    DepositAccount.Bank = OldDepositAccount.Bank;
                    DepositBank = OldDepositBank;
                }

                if (OldPaymentAccount != null)
                {
                    ChargeAccount.CLABE = OldPaymentAccount.CLABE;
                    ChargeAccount.Bank = OldPaymentAccount.Bank;
                    ChargeBank = OldChargeBank;
                }

                SwitchMode(PageMode.View);
            }
        }

        private void Save(object obj)
        {
            string message = string.Empty;
            bool validation = false;

            try
            {
                if (FieldValidator.IsValidCLABE(ChargeAccount.CLABE) && FieldValidator.IsValidCLABE(DepositAccount.CLABE))
                {
                    if (ChargeBank == null || DepositBank == null)
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
                ChargeAccount.Bank = ChargeBank.BankId;
                DepositAccount.Bank = DepositBank.BankId;

                var (updateMessage, updatedDeposit, updatedCharge) = _clientManagementService.UpdateAccounts(DepositAccount, ChargeAccount);

                Client.DepositAccount = updatedDeposit;
                Client.PaymentAccount = updatedCharge;

                _dialogService.Dismiss(updateMessage, MessageBoxImage.Information);

                SwitchMode(PageMode.View);
            }
            else
            {
                _dialogService.Dismiss(message, MessageBoxImage.Exclamation);
            }
        }

        private void Edit(object obj)
        {
            SwitchMode(PageMode.Update);
        }

        private bool CanNext(object obj)
        {
            return true;
        }

        private Account CreateAccountCopy(Account original)
        {
            if (original == null) return null;

            return new Account
            {
                AccountId = original.AccountId,
                CLABE = original.CLABE,
                Bank = original.Bank
            };
        }

    }
}