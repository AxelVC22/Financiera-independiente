﻿using Independiente.Commands;
using Independiente.Model;
using Independiente.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace Independiente.ViewModel
{
    public class CreditDetailsViewModel : ModificableViewModel
    {
        public CreditApplication CreditApplication { get; set; }

        public ObservableCollection<PromotionalOffer> PromotionOffersList { get; set; }

        //public List<string> PaymentFrecuenciesList { get; set; }


        private IDialogService _dialogService;
        private INavigationService _navigationService;
        private readonly IFilePickerService _filePickerService;
        private ICreditApplicationService _creditApplicationService;
        private IPromotionalOfferService _promotionalOfferService;
        private ICreditApplicationGeneratorService _creditApplicationGeneratorService;
        private IClientManagementService _clientManagementService;

        public ICommand SelectINECommand { get; set; }

        public ICommand SelectProofOfAddressCommand { get; set; }

        public ICommand SelectAccountStatementCoverPageCommand { get; set; }

        public ICommand SelectCreditApplicationCommand { get; set; }

        public ICommand GenerateCreditApplicationCommand { get; set; }

        private string _inePath;

        private string _proofOfAddressPath;

        private string _accountStatementCoverPagePath;

        private string _creditApplicationPath;
        private PromotionalOffer _selectedPromotion;
        private Client _client;

        private PageMode _pageMode { get; set; }


        public CreditDetailsViewModel()
        {

        }

        public CreditDetailsViewModel(IDialogService dialogService, INavigationService navigationService, PageMode mode, Client client, ICreditApplicationService creditApplicationService,
            IPromotionalOfferService promotionalOfferService, ICreditApplicationGeneratorService creditApplicationGeneratorService, IClientManagementService clientManagementService)
        {
            _promotionalOfferService = promotionalOfferService;
            List<PromotionalOffer> promotionOffersList = _promotionalOfferService.GetAllPromotionalOffers();
            PromotionOffersList = new ObservableCollection<PromotionalOffer>(promotionOffersList);


            NextCommand = new RelayCommand(Next, CanNext);
            EditCommand = new RelayCommand(Edit, CanNext);
            CancelCommand = new RelayCommand(Cancel, CanNext);
            SaveCommand = new RelayCommand(Save, CanNext);
            GoBackCommand = new RelayCommand(GoBack, CanNext);
            SelectINECommand = new RelayCommand(SelectINE, CanNext);
            SelectProofOfAddressCommand = new RelayCommand(SelectProofOfAddress, CanNext);
            SelectAccountStatementCoverPageCommand = new RelayCommand(SelectAccountStatementCoverPage, CanNext);
            SelectCreditApplicationCommand = new RelayCommand(SelectCreditApplication, CanNext);
            GenerateCreditApplicationCommand = new RelayCommand(GenerateCreditApplication, CanNext);

            _clientManagementService = clientManagementService;
            CreditApplication = new CreditApplication();
            _dialogService = dialogService;
            _navigationService = navigationService;
            _filePickerService = new FilePickerService();
            _creditApplicationService = creditApplicationService;
            _creditApplicationGeneratorService = creditApplicationGeneratorService;
            _client = client;
            SwitchMode(mode);
            CreditApplication.LoanApplicationDate = DateTime.Now;

            if (CreditApplication != null)
            {
                CreditApplication.PropertyChanged += CreditApplication_PropertyChanged;
            }

        }

        public PromotionalOffer SelectedPromotion
        {
            get => _selectedPromotion;
            set
            {
                if (_selectedPromotion != value)
                {
                    _selectedPromotion = value;
                    OnPropertyChanged(nameof(SelectedPromotion));

                    if (_selectedPromotion != null)
                    {
                        CreditApplication.PromotionalOffer = _selectedPromotion;
                    }

                    ClearCreditApplicationFile();
                }
            }
        }

        private void CreditApplication_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CreditApplication.LoanAmount))
            {
                ClearCreditApplicationFile();
            }
        }

        private void ClearCreditApplicationFile()
        {
            CreditApplicationPath = null;  
            OnPropertyChanged(nameof(CreditApplicationPath));
            OnPropertyChanged(nameof(CreditApplicationFileName)); 
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
            if (_inePath == null || _proofOfAddressPath == null || _accountStatementCoverPagePath == null)
            {
                _dialogService.Dismiss(Properties.Messages.IncompleteDocumentationMessage, System.Windows.MessageBoxImage.Information);
            }
            else
            {
                CreditApplicationPath = OpenFile();
            }

        }

        private void GenerateCreditApplication(object obj)
        {
            if (_inePath == null || _proofOfAddressPath == null || _accountStatementCoverPagePath == null)
            {
                _dialogService.Dismiss(Properties.Messages.IncompleteDocumentationMessage, System.Windows.MessageBoxImage.Information);
            }
            else
            {
                CreditApplicationPath = SaveFolder();
                List<Model.AmortizationSchedule> amortizationSchedules = _creditApplicationService.GetAmortizationSchedule(CreditApplication);
                _creditApplicationGeneratorService.GenerateCompleteReport(CreditApplicationPath, _client, CreditApplication, amortizationSchedules);
            }
        }

        private string OpenFile()
        {
            string filePath = _filePickerService.PickFile();
            if (string.IsNullOrEmpty(filePath))
            {
                IDialogService dialogService = new DialogService();
            }
            return filePath;

        }

        private string SaveFolder()
        {
            string folderPath = _filePickerService.SaveFile($"SolicitudCredito_{_client.PersonalData.RFC}");
            if (string.IsNullOrEmpty(folderPath))
            {
                IDialogService dialogService = new DialogService();
            }
            return folderPath;
        }
        private void Next(object obj)
        {
            try
            {
                if (ValidateInput())
                {
                    _client.Employee = App.SessionService.CurrentUser.EmployeeId;
                    _client.ClientId = _clientManagementService.AddClient(_client);

                    if (_client.ClientId > 0)
                    {
                        CreditApplication.PromotionalOffer = SelectedPromotion;
                        CreditApplication.Client = _client;

                        var files = new List<Model.File>
                        {
                            CreateFileModel(INEPath, FileType.INE),
                            CreateFileModel(ProofOfAddressPath, FileType.POA),
                            CreateFileModel(AccountStatementCoverPagePath, FileType.ASCP),
                            CreateFileModel(CreditApplicationPath, FileType.CA)
                        };

                        var resultId = _creditApplicationService.AddCreditApplicationWithFiles(CreditApplication, files);

                        if (resultId > 0)
                        {
                            _navigationService.NavigateTo<EmployeeAndClientConsultationViewModel>();
                        }
                    }
                }
                else
                {
                    _dialogService.Dismiss(Properties.Messages.IncompleteDocumentationMessage, System.Windows.MessageBoxImage.Information);
                }
            }
            catch (ArgumentException ex)
            {
                _dialogService.Dismiss(ex.Message, System.Windows.MessageBoxImage.Exclamation);
            }
        }
        private Model.File CreateFileModel(string path, FileType fileType)
        {
            return new Model.File
            {
                FileType = fileType,
                FileContent = System.IO.File.ReadAllBytes(path),
                Client = _client
            };
        }
        private bool ValidateInput()
        {
            return !string.IsNullOrEmpty(INEPath) &&
                   !string.IsNullOrEmpty(ProofOfAddressPath) &&
                   !string.IsNullOrEmpty(AccountStatementCoverPagePath) &&
                   !string.IsNullOrEmpty(CreditApplicationPath) &&
                   FieldValidator.IsValidMoney(CreditApplication.LoanAmount);
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