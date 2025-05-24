using Independiente.Commands;
using Independiente.DataAccess.Repositories;
using Independiente.Model;
using Independiente.Services;
using Independiente.Services.Mappers;
using Independiente.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using CreditApplication = Independiente.Model.CreditApplication;

namespace Independiente.ViewModel
{
    public class CreditApplicationsViewModel : BaseViewModel
    {
        public PaginationViewModelBase Pagination { get; }

        public ObservableCollection<CreditApplication> CreditApplicationsList { get; set; } = new ObservableCollection<CreditApplication>();

        public IDialogService _dialogService;

        public INavigationService _navigationService;
        public ICommand GoToPageCommand { get; set; }

        public ICommand OrderByNameCommand { get; set; }

        public ICommand SearchCommand { get; set; }

        public ICommand RestoreCommand { get; set; }

        public ICommand CheckCommand { get; set; }


        private ICreditApplicationService _service;

        private CreditApplication _selectedCreditApplication;

        private bool _isOrderedAscendent = false;

        public CreditApplicationQuery Query { get; set; } = new CreditApplicationQuery();

        public Dictionary<string, CreditApplicationStates?> StateFilterOptions { get; } = new Dictionary<string, CreditApplicationStates?>
        {
            { "Todos", null },
            { "Pendiente", CreditApplicationStates.Pending },
            { "Aceptado", CreditApplicationStates.Accepted },
            { "Rechazado", CreditApplicationStates.Rejected }
        };

        

        private KeyValuePair<string, CreditApplicationStates?> _selectedStateFilter;
        
        public CreditApplicationsViewModel() { }

        public CreditApplicationsViewModel(IDialogService dialogService, INavigationService navigationService, ICreditApplicationService creditApplicationService)
        {
            _dialogService = dialogService;

            _navigationService = navigationService;

            _service = creditApplicationService;

            GoToPageCommand = new RelayCommand(ChangePage, CanDoIt);

            GoBackCommand = new RelayCommand(GoBack, CanDoIt);


            Pagination = new PaginationViewModelBase(_service.CountCreditApplications(Query));

            Query = new CreditApplicationQuery { PageNumber = Pagination.PageNumber, PageSize = Pagination.PageSize };


            SelectedStateFilter = StateFilterOptions.First();

            SearchCommand = new RelayCommand(Search, CanDoIt);

            RestoreCommand = new RelayCommand(Restore, CanDoIt);

            CheckCommand = new RelayCommand(Check, CanDoIt);

            Search(null);

        }

        private bool CanDoIt(object obj)
        {
            return true;
        }

        private void GoBack(object obj)
        {
            _navigationService.GoBack();
        }

        private void Check(object obj)
        {
            Independiente.Model.CreditApplication creditApplication;
            
            if (obj is Independiente.Model.CreditApplication application)
            {
                creditApplication = _service.GetCreditApplication(application.CreditApplicationId);

                if (creditApplication != null)
                {
                    _navigationService.NavigateTo<CreditApplicationValidationViewModel>(creditApplication);
                }
            }
        }

        private void Restore(object obj)
        {
            Query.FromDate = null;
            Query.ToDate = null;
            Query.RFC = null;

            SelectedStateFilter = StateFilterOptions.First();

            Search(null);
        }

        private void Search(object obj)
        {
            try
            {
                CreditApplicationsList.Clear();

                Pagination.TotalItems = _service.CountCreditApplications(Query);

                if (Pagination.TotalItems > 0)
                {

                    var creditApplications = _service.GetCreditApplications(Query);

                    if (creditApplications != null && creditApplications.Count > 0)
                    {
                        foreach (var c in creditApplications)
                        {
                            CreditApplicationsList.Add(c);
                        }
                    }
                    else
                    {
                        ChangePage(1);
                    }
                }

                Pagination.Refresh();
            }
            catch (ArgumentException e)
            {
                _dialogService.Dismiss(e.Message, System.Windows.MessageBoxImage.Information);
            }
        }

        private void ChangePage(object obj)
        {
            int pageNumber = 1;

            if (obj is PageLink link && link.PageNumber.HasValue)
            {
                pageNumber = link.PageNumber.Value;
            }
            else if (obj is int i)
            {
                pageNumber = i;
            }
            else if (obj is string s && int.TryParse(s, out int result))
            {
                pageNumber = result;
            }

            Pagination.PageNumber = pageNumber;
            Query.PageNumber = pageNumber;
            Search(obj);
        }

        public ICommand OrderByCommand => new RelayCommand(param =>
        {
            switch (param as string)
            {
                case "FullName":
                    OrderByProperty(x => x.Client.PersonalData.FullName);
                    break;
                case "RFC":
                    OrderByProperty(x => x.Client.PersonalData.RFC);
                    break;

                case "LoanAmount":
                    OrderByProperty(x => x.LoanAmount);
                    break;
                case "LoanApplicationDate":
                    OrderByProperty(x => x.LoanApplicationDate);
                    break;
                case "Status":
                    OrderByProperty(x => x.Status);
                    break;
            }
        }, CanDoIt);

        private void OrderByProperty(Func<CreditApplication, object> keySelector)
        {
            IEnumerable<CreditApplication> listaOrdenada;

            if (_isOrderedAscendent)
            {
                listaOrdenada = CreditApplicationsList.OrderByDescending(keySelector);
            }
            else
            {
                listaOrdenada = CreditApplicationsList.OrderBy(keySelector);
            }
            _isOrderedAscendent = !_isOrderedAscendent;
            CreditApplicationsList = new ObservableCollection<CreditApplication>(listaOrdenada);
            OnPropertyChanged(nameof(CreditApplicationsList));
        }


        public CreditApplication SelectedCreditApplication
        {
            get => _selectedCreditApplication;
            set
            {
                if (_selectedCreditApplication != value)
                {
                    _selectedCreditApplication = value;
                    OnPropertyChanged(nameof(SelectedCreditApplication));
                }
            }
        }



        public KeyValuePair<string, CreditApplicationStates?> SelectedStateFilter
        {
            get => _selectedStateFilter;
            set
            {
                if (_selectedStateFilter.Equals(value))
                {
                    return;
                }
                _selectedStateFilter = value;
                Query.Status = _selectedStateFilter.Value.ToString();
                OnPropertyChanged(nameof(SelectedStateFilter));
            }
        }


    }
}
