using Independiente.DataAccess;
using Independiente.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System;
using System.IO;
using System.ComponentModel;
using System.Windows.Documents;
using Independiente.Services.Mappers;
using System.Management.Instrumentation;
using System.Windows;
using System.Collections.ObjectModel;
using Independiente.Model;
using Independiente.DataAccess.Repositories;
using System.Windows.Input;
using Independiente.Commands;
using CreditPolicy = Independiente.Model.CreditPolicy;
using Report = Independiente.Model.Report;
using Independiente.View;
using Independiente.Properties;
using System.Resources;


namespace Independiente.ViewModel
{
    public class CreditApplicationValidationViewModel : BaseViewModel
    {

        private IDialogService _dialogService;

        private INavigationService _navigationService;

        private ICreditApplicationService _creditApplicationService;

        public ICommand CheckCommand { get; set; }

        public ICommand SubmitCommand { get; set; }

        public ICommand AmortizationCommmand { get; set; }

        private int _creditPoliciesTotal;

        private int _creditPoliciesPassed;

        public ObservableCollection<Model.CreditPolicy> CreditPoliciesList { get; set; } = new ObservableCollection<Model.CreditPolicy>();

        private Independiente.Model.CreditApplication _creditApplication;

        public Independiente.Model.CreditApplication CreditApplication { get; set; }

        public Report Report { get; set; } = new Report();

        public bool IsEditable { get; set; } = false;

        public bool IsAccepted { get; set; } = false;

        public bool IsRejected { get; set; } = false;

        private byte[] _pdfBytes;
        public byte[] PdfBytes
        {
            get => _pdfBytes;
            set
            {
                _pdfBytes = value;
                OnPropertyChanged(nameof(PdfBytes));
            }
        }

        public Dictionary<string, FileType> DocumentFilterOptions { get; } = new Dictionary<string, FileType>
        {
            {Resources.CreditApplicationLabelGlobal, FileType.CA },
            {Resources.INELabelGlobal,  FileType.INE},
            {Resources.ProofOfAddressLabelGlobal, FileType.POA },
              {"Caratula de estado de cuenta", FileType.ASCP }

        };

        private KeyValuePair<string, FileType> _selectedDocumentFilter;


        public CreditApplicationValidationViewModel(IDialogService dialogService, INavigationService navigationService, ICreditApplicationService creditApplicationService, Independiente.Model.CreditApplication creditApplication)
        {
            _dialogService = dialogService;
            _navigationService = navigationService;
            _creditApplicationService = creditApplicationService;
            CreditApplication = creditApplication;
            _creditApplication = CreditApplication;
            GoBackCommand = new RelayCommand(GoBack, CanDoIt);
            CheckCommand = new RelayCommand(Check, CanDoIt);
            SelectedDocumentFilter = DocumentFilterOptions.First();
            SubmitCommand = new RelayCommand(Submit, CanDoIt);
            AmortizationCommmand = new RelayCommand(GoToAmortization, CanDoIt);
            Report.CreditApplication = creditApplication;
            IsEditable = (creditApplication.Status == CreditApplicationStates.Pending) ? true : false;
            IsAccepted = (creditApplication.Status == CreditApplicationStates.Accepted) ? true : false;
            IsRejected = (creditApplication.Status == CreditApplicationStates.Rejected) ? true : false;


            
            try
            {
                if (!IsEditable)
                {
                    LoadReport();
                }
                else
                {
                    LoadCreditPolicies();
                }
            }
            catch (InvalidOperationException ex)
            {
                _dialogService.Dismiss(ex.Message, MessageBoxImage.Error);
            }

            if (App.SessionService.CurrentUser.UserRole != UserRole.Analyst)
            {
                IsEditable = false;
            }
        }

        private void Submit(object obj)
        {
            string message = null;

            if (CreditApplication.Status == CreditApplicationStates.Accepted)
            {
                message = Messages.ResourceManager.GetString("AcceptApplicationConfirmationMessage");
            }
            else if (CreditApplication.Status == CreditApplicationStates.Rejected)
            {
                message = Messages.ResourceManager.GetString("RejectApplicationConfirmationMessage");
            }


            if (CreditApplication.Status != CreditApplicationStates.Pending && _dialogService.Confirm(message))
            {
                if (Report.CreditApplication.Status == CreditApplicationStates.Rejected)
                {
                    foreach (CreditPolicy c in CreditPoliciesList)
                    {
                        if (!c.IsPassed)
                        {
                            Report.CreditPolicies.Add(c);
                        }
                    }
                }

                Report.ReviewingDate = DateTime.Now;

                try
                {
                    if (_creditApplicationService.SubmitDecision(Report) > 1)
                    {
                        _dialogService.Dismiss(Messages.ResourceManager.GetString("SentReportMessage"), MessageBoxImage.Information);
                        _navigationService.NavigateTo<CreditApplicationsViewModel>();
                    }
                }
                catch (ArgumentException e)
                {
                    _dialogService.Dismiss(e.Message, MessageBoxImage.Information);
                }
                catch (InvalidOperationException ex)
                {
                    _dialogService.Dismiss(ex.Message, MessageBoxImage.Error);
                }

            }
        }

        private void GoToAmortization(object obj)
        {
            _navigationService.NavigateTo<AmortizationScheduleViewModel>(CreditApplication);
        }


        private void SearchDocument(KeyValuePair<string, FileType> selectedDocumentFilter)
        {
            try
            {
                if (selectedDocumentFilter.Value == FileType.CA)
                {
                    if (CreditApplication.File.FileContent == null)
                    {
                        try
                        {
                            var file = _creditApplicationService.GetDocument(CreditApplication.Client.ClientId, selectedDocumentFilter.Value.ToString());
                            PdfBytes = file.FileContent;
                            CreditApplication.File = file;
                        }
                        catch (InvalidOperationException e)
                        {
                            _dialogService.Dismiss(e.Message, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        PdfBytes = CreditApplication.File.FileContent;
                    }
                }
                else
                {
                    var document = CreditApplication.Documents.FirstOrDefault(doc => doc.FileType == selectedDocumentFilter.Value);

                    if (document != null)
                    {
                        PdfBytes = document.FileContent;
                    }
                    else
                    {
                        try
                        {
                            var file = _creditApplicationService.GetDocument(CreditApplication.Client.ClientId, selectedDocumentFilter.Value.ToString());
                            PdfBytes = file.FileContent;
                            CreditApplication.Documents.Add(file);

                        }
                        catch (InvalidOperationException e)
                        {
                            _dialogService.Dismiss(e.Message, MessageBoxImage.Error);
                        }

                    }
                }
            }
            catch (KeyNotFoundException e)
            {
                PdfBytes = null;
                _dialogService.Dismiss(e.Message, MessageBoxImage.Information);
            }
        }
        private void Check(object obj)
        {
            if (obj is CreditPolicy c)
            {
                if (c.IsPassed)
                {
                    CreditPoliciesPassed = CreditPoliciesPassed + 1;
                }
                else
                {
                    CreditPoliciesPassed -= 1;
                }

                if (CreditPoliciesPassed == CreditPoliciesTotal)
                {
                    CreditApplication.Status = CreditApplicationStates.Accepted;
                }
                else
                {
                    CreditApplication.Status = CreditApplicationStates.Rejected;
                }
            }
        }

        private bool CanDoIt(object obj)
        {
            return true;
        }

        private void GoBack(object obj)
        {

            if (CreditApplication.Status == CreditApplicationStates.Pending)
            {
                if (_dialogService.Confirm("¿Estás seguro de querer salir? Los cambios de la validación no se guardarán"))
                {
                    CreditApplication = _creditApplication;
                    _navigationService.GoBack();
                }
            }
            else
            {
                _navigationService.GoBack();
            }

        }

        private void LoadCreditPolicies()
        {
            var creditPolicies = _creditApplicationService.GetCreditPolicies(new CreditPolicyQuery { Status = CreditPolicyStates.Active.ToString() , PageSize = 100,PageNumber=1 });

            foreach (Model.CreditPolicy c in creditPolicies)
            {
                CreditPoliciesList.Add(c);
            }

            CreditPoliciesTotal = creditPolicies.Count();
        }

        private void LoadReport()
        {
            Independiente.Model.Report report = null;
            try
            {
                report = _creditApplicationService.GetReport(CreditApplication.CreditApplicationId);
            }
            catch (ArgumentException e)
            {
                _dialogService.Dismiss(e.Message, MessageBoxImage.Error);
            }


            if (report != null)
            {
                Report = report;

                if (CreditApplication.Status == CreditApplicationStates.Rejected)
                {
                    CreditPoliciesList = new ObservableCollection<Model.CreditPolicy>(report.CreditPolicies);
                    CreditPoliciesTotal = report.CreditPolicies.Count();
                    CreditPoliciesPassed = report.CreditPolicies.Count(c => c.IsPassed);
                }
            }
        }


        public CreditApplicationValidationViewModel()
        {

        }

        public int CreditPoliciesTotal
        {
            get => _creditPoliciesTotal;
            set
            {
                _creditPoliciesTotal = value;
                OnPropertyChanged(nameof(CreditPoliciesTotal));
            }
        }

        public int CreditPoliciesPassed
        {
            get => _creditPoliciesPassed;
            set
            {
                _creditPoliciesPassed = value;
                OnPropertyChanged(nameof(CreditPoliciesPassed));
            }
        }

        public KeyValuePair<string, FileType> SelectedDocumentFilter
        {
            get => _selectedDocumentFilter;
            set
            {
                if (_selectedDocumentFilter.Equals(value))
                {
                    return;
                }
                _selectedDocumentFilter = value;
                OnPropertyChanged(nameof(SelectedDocumentFilter));
                SearchDocument(_selectedDocumentFilter);
            }
        }


    }
}
