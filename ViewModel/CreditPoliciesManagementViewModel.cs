using Independiente.Commands;
using Independiente.DataAccess.Repositories;
using Independiente.Model;
using Independiente.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;

namespace Independiente.ViewModel
{
    public class CreditPoliciesManagementViewModel : ModificableViewModel
    {
        public PaginationViewModelBase Pagination { get; }
        public ObservableCollection<CreditPolicy> CreditPoliciesList { get; set; } = new ObservableCollection<CreditPolicy>();

        public CreditPolicy _selectedCreditPolicy;        
        public CreditPolicyQuery Query { get; set; } = new CreditPolicyQuery();

        private ICreditPolicyService _service;

        public IDialogService _dialogService;

        public INavigationService _navigationService;
        public ICommand GoToPageCommand { get; set; }
        public ICommand RegisterCommand { get; set; }          
        public ICommand SaveCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand RestoreCommand { get; set; }        
        public ICommand CancelCommand { get; set; }
        public ICommand GoBackCommand { get; set; }

        private bool _isOrderedAscendent = false;
        public bool _IsManagementVisible { get; set; } = true;

        public Dictionary<string, CreditPolicyStates?> StateFilterOptions { get; } = new Dictionary<string, CreditPolicyStates?>
        {
            { "Todos", null },
            { "Activo", CreditPolicyStates.Active },
            { "Inactivo", CreditPolicyStates.Inactive }            
        };
        public Dictionary<string, bool?> ValidityFilterOptions { get; } = new Dictionary<string, bool?>
        {
            { "Todos", null },
            { "Vigente", true },
            { "No vigente", false }
        };

        private KeyValuePair<string, CreditPolicyStates?> _selectedStateFilter;

        private KeyValuePair<string, bool?> _selectedValidityFilter;
        public CreditPoliciesManagementViewModel() { }
        public CreditPoliciesManagementViewModel(IDialogService dialogService, INavigationService navigationService, ICreditPolicyService creditPolicyService)
        {            
            _dialogService = dialogService;

            _navigationService = navigationService;

            _service = creditPolicyService;

            GoToPageCommand = new RelayCommand(ChangePage, CanDoIt);

            GoBackCommand = new RelayCommand(GoBack, CanDoIt);

            Pagination = new PaginationViewModelBase(_service.CountCreditPolicies(Query));

            Query = new CreditPolicyQuery { PageNumber = Pagination.PageNumber, PageSize = Pagination.PageSize };

            SelectedStateFilter = StateFilterOptions.First();

            SelectedValidityFilter = ValidityFilterOptions.First();

            EditCommand = new RelayCommand(Edit, CanDoIt);
            CancelCommand = new RelayCommand(Cancel, CanDoIt);
            SaveCommand = new RelayCommand(Save, CanDoIt);
            RegisterCommand = new RelayCommand(Register, CanDoIt);            

            SearchCommand = new RelayCommand(Search, CanDoIt);

            RestoreCommand = new RelayCommand(Restore, CanDoIt);
            
            Search(null);       

            _selectedCreditPolicy = new CreditPolicy();            
        }
        private void Save(object obj)
        {
            if (_service.AddCreditPolicy(_selectedCreditPolicy) > 0)
            {
                // mensaje de exito y clear a la ventana de registro
            } else
            {
                // Mensaje de falla y nose
            }
        }

        private void Edit(object obj)
        {
            _selectedCreditPolicy.IsEditable = true;
        }

        private void Cancel(object obj)
        {
            _selectedCreditPolicy.IsEditable = false;
        }
        
        private bool CanDoIt(object obj)
        {
            return true;
        }

        private void GoBack(object obj)
        {
            _navigationService.GoBack();
        }

        private void Restore(object obj)
        {
            Query.Name = null;            

            SelectedStateFilter = StateFilterOptions.First();
            SelectedValidityFilter = ValidityFilterOptions.First();

            Search(null);
        }

        private void Search(object obj)
        {
            try
            {
                CreditPoliciesList.Clear();

                Pagination.TotalItems = _service.CountCreditPolicies(Query);

                if (Pagination.TotalItems > 0)
                {

                    var creditPolicies = _service.GetCreditPolicies(Query);

                    if (creditPolicies != null && creditPolicies.Count > 0)
                    {
                        foreach (var c in creditPolicies)
                        {
                            CreditPoliciesList.Add(c);
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

        private void Register(object obj)
        {
            //CreditPolicy newCreditPolicy = new CreditPolicy { IsEditable = true, RegistrationDate = DateTime.Today };
            CreditPolicy newCreditPolicy = new CreditPolicy { IsEditable = true };
            CreditPoliciesList.Add(newCreditPolicy);
        }

        public ICommand OrderByCommand => new RelayCommand(param =>
        {
            switch (param as string)
            {
                case "Name":
                    OrderByProperty(x => x.Name);
                    break;                
                case "RegistrationDate":
                    OrderByProperty(x => x.RegistrationDate);
                    break;
                case "EndDate":
                    OrderByProperty(x => x.EndDate);
                    break;
                case "Status":
                    OrderByProperty(x => x.Status);
                    break;
            }
        }, CanDoIt);

        private void OrderByProperty(Func<CreditPolicy, object> keySelector)
        {
            IEnumerable<CreditPolicy> listaOrdenada;

            if (_isOrderedAscendent)
            {
                listaOrdenada = CreditPoliciesList.OrderByDescending(keySelector);
            }
            else
            {
                listaOrdenada = CreditPoliciesList.OrderBy(keySelector);
            }
            _isOrderedAscendent = !_isOrderedAscendent;
            CreditPoliciesList = new ObservableCollection<CreditPolicy>(listaOrdenada);
            OnPropertyChanged(nameof(CreditPoliciesList));
        }

        public CreditPolicy SelectedCreditPolicy
        {
            get => _selectedCreditPolicy;
            set
            {
                if (_selectedCreditPolicy != value)
                {
                    _selectedCreditPolicy.IsEditable = false;
                    _selectedCreditPolicy = value;
                    OnPropertyChanged(nameof(SelectedCreditPolicy));
                }
            }
        }

        private KeyValuePair<string, CreditPolicyStates?> SelectedStateFilter
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

        private KeyValuePair<string, bool?> SelectedValidityFilter
        {
            get => _selectedValidityFilter;
            set
            {
                if (_selectedValidityFilter.Equals(value))
                {
                    return;
                }
                _selectedValidityFilter = value;
                Query.Status = _selectedValidityFilter.Value.ToString();
                OnPropertyChanged(nameof(SelectedValidityFilter));
            }
        }
    }
}