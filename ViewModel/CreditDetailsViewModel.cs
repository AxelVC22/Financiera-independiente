using Independiente.Commands;
using Independiente.Model;
using Independiente.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using System.Collections.ObjectModel;
using System.Collections;
using System.Resources;

namespace Independiente.ViewModel
{
    public class CreditDetailsViewModel : ModificableViewModel
    {
        public CreditApplication CreditApplication { get; set; }

        public ObservableCollection<PromotionalOffer> PromotionOffersList { get; set; }

        public List<string> PaymentFrecuenciesList { get; set; }


        private IDialogService _dialogService;
        private INavigationService _navigationService;
        private readonly IFilePickerService _filePickerService;

        public ICommand SelectINECommand { get; set; }

        public ICommand SelectProofOfAddressCommand { get; set; }

        public ICommand SelectAccountStatementCoverPageCommand { get; set; }

        public ICommand SelectCreditApplicationCommand { get; set; }

        public ICommand GenerateCreditApplication { get; set; }

        private string _inePath;

        private string _proofOfAddressPath;

        private string _accountStatementCoverPagePath;

        private string _creditApplicationPath;

        private PageMode _pageMode { get; set; }


        public CreditDetailsViewModel()
        {

        }

        public CreditDetailsViewModel(IDialogService dialogService, INavigationService navigationService, PageMode mode)
        {
            PromotionOffersList = new ObservableCollection<PromotionalOffer>
                {
                    new PromotionalOffer { InteresRate = 1m, LoanTerm=2 },
                    new PromotionalOffer { InteresRate = 3m, LoanTerm=4 },
                    new PromotionalOffer { InteresRate = 5m, LoanTerm=5 },
                };
            CreditApplication = new CreditApplication();
            NextCommand = new RelayCommand(Next, CanNext);
            EditCommand = new RelayCommand(Edit, CanNext);
            CancelCommand = new RelayCommand(Cancel, CanNext);
            SaveCommand = new RelayCommand(Save, CanNext);
            GoBackCommand = new RelayCommand(GoBack, CanNext);
            SelectINECommand = new RelayCommand(SelectINE, CanNext);
            SelectProofOfAddressCommand = new RelayCommand(SelectProofOfAddress, CanNext);
            SelectAccountStatementCoverPageCommand = new RelayCommand(SelectAccountStatementCoverPage, CanNext);
            SelectCreditApplicationCommand = new RelayCommand(SelectCreditApplication, CanNext);


            _dialogService = dialogService;
            _navigationService = navigationService;
            _filePickerService = new FilePickerService();
            SwitchMode(mode);
            LoadPaymentFrecuencies();

        }

        private void LoadPaymentFrecuencies()
        {
            PaymentFrecuenciesList = new List<string>();
            var resourceManager = new ResourceManager("Independiente.Properties.PaymentFrecuencies", typeof(PersonalDataViewModel).Assembly);

            var resourceSet = resourceManager.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, true, true);

            var states = resourceSet.Cast<DictionaryEntry>()
                                    .Where(entry => entry.Value is string)
                                    .Select(entry => entry.Value.ToString())
                                    .ToList();
            PaymentFrecuenciesList = states;
        }

        public string INEPath
        {
            get => _inePath;
            set
            {
                if (_inePath != value)
                {
                    _inePath = value;
                    OnPropertyChanged(nameof(INEPath));
                    OnPropertyChanged(nameof(INEFileName));
                }
            }
        }
        public string INEFileName
        {
            get => Path.GetFileName(INEPath);
        }

        public string ProofOfAddressPath
        {
            get => _proofOfAddressPath;
            set
            {
                if (_proofOfAddressPath != value)
                {
                    _proofOfAddressPath = value;
                    OnPropertyChanged(nameof(ProofOfAddressPath));
                    OnPropertyChanged(nameof(ProofOfAddressFileName));
                }
            }
        }

        public string ProofOfAddressFileName
        {
            get => Path.GetFileName(ProofOfAddressPath);
        }

        public string AccountStatementCoverPagePath
        {
            get => _accountStatementCoverPagePath;
            set
            {
                if (_accountStatementCoverPagePath != value)
                {
                    _accountStatementCoverPagePath = value;
                    OnPropertyChanged(nameof(AccountStatementCoverPagePath));
                    OnPropertyChanged(nameof(AccountStatementCoverPageFileName));
                }
            }
        }

        public string AccountStatementCoverPageFileName
        {
            get => Path.GetFileName(AccountStatementCoverPagePath);
        }

        public string CreditApplicationPath
        {
            get => _creditApplicationPath;
            set
            {
                if (_creditApplicationPath != value)
                {
                    _creditApplicationPath = value;
                    OnPropertyChanged(nameof(CreditApplicationPath));
                    OnPropertyChanged(nameof(CreditApplicationFileName));
                }
            }
        }

        public string CreditApplicationFileName
        {
            get => Path.GetFileName(CreditApplicationPath);
        }

        private void SelectINE(object obj)
        {
            INEPath = OpenFile();
        }

        private void SelectProofOfAddress(object obj)
        {
            ProofOfAddressPath = OpenFile();
        }

        private void SelectAccountStatementCoverPage(object obj)
        {
            AccountStatementCoverPagePath = OpenFile();
        }

        private void SelectCreditApplication(object obj)
        {
            CreditApplicationPath = OpenFile();
        }

        private string OpenFile()
        {
            string filePath = _filePickerService.PickFile();
            if (string.IsNullOrEmpty(filePath))
            {
                IDialogService dialogService = new DialogService();
                dialogService.Dismiss("gg");
            }
            return filePath;

        }

        private void Next(object obj)
        {
            _navigationService.NavigateTo<ReferencesViewModel>(new PersonDataParams(_pageMode));
        }

        private void GoBack(object obj)
        {
            _navigationService.GoBack();
        }


        private void Cancel(object obj)
        {
            SwitchMode(PageMode.View);
        }

        private void Save(object obj)
        {
            Console.WriteLine(CreditApplication.ToString());
            SwitchMode(PageMode.View);
        }

        private void Edit(object obj)
        {

            SwitchMode(PageMode.Update);
        }

        private bool CanNext(object obj)
        {
            return true;
        }



    }
}
