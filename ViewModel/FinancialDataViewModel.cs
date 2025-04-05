using Independiente.Commands;
using Independiente.Model;
using Independiente.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Independiente.ViewModel
{
    public class FinancialDataViewModel : ModificableViewModel
    {
        public ObservableCollection<Bank> BanksList { get; set; }
        private Bank _selectedBankCharge;

        private Bank _selectedBankDeposit;

        private PageMode _pageMode;


        public event EventHandler RequestClose;
        public bool IsNextSuccessful;

        private INavigationService _navigationService { get; set; }
        private IDialogService _dialogService { get; set; }

        public FinancialDataViewModel()
        {

        }

        private Account _chargeAccount;

        public Account ChargeAccount
        {
            get => _chargeAccount;
            set
            {
                if (_chargeAccount != value)
                {
                    _chargeAccount = value;
                    OnPropertyChanged(nameof(Account));
                }
            }
        }

        private Account _depositAccount;

        public Account DepositAccount
        {
            get => _depositAccount;
            set
            {
                if (_depositAccount != value)
                {
                    _depositAccount = value;
                    OnPropertyChanged(nameof(Account));  // Notificar a la vista cuando cambia PersonalData
                }
            }
        }

        private void GoBack(object obj)
        {
            _navigationService.GoBack();
        }

        public FinancialDataViewModel(IDialogService dialogService, INavigationService navigationService, PageMode mode)
        {
            BanksList = new ObservableCollection<Bank>
                {
                    new Bank { BankId = 1, Name = "Banco A" },
                    new Bank { BankId = 2, Name = "Banco B" },
                    new Bank { BankId = 3, Name = "Banco C" },
                };
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
        }
        private void Next(object obj)
        {
            _navigationService.NavigateTo<ReferencesViewModel>(new PersonalDataParams(_pageMode));

        }

        private void Cancel(object obj)
        {
            SwitchMode(PageMode.View);
        }

        private void Save(object obj)
        {
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

        public Bank SelectedBankCharge
        {
            get => _selectedBankCharge;
            set
            {
                _selectedBankCharge = value;
                OnPropertyChanged(nameof(SelectedBankCharge));
            }
        }

        public Bank SelectedBankDeposit
        {
            get => _selectedBankDeposit;
            set
            {
                _selectedBankDeposit = value;
                OnPropertyChanged(nameof(SelectedBankDeposit));
            }
        }
    }
}
