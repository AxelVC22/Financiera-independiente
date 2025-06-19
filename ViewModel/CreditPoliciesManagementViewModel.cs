using Independiente.Commands;
using Independiente.DataAccess.Repositories;
using Independiente.Model;
using Independiente.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Independiente.ViewModel
{
    public class CreditPoliciesManagementViewModel : ModificableViewModel
    {
        public PaginationViewModelBase Pagination { get; }
        public ObservableCollection<CreditPolicy> CreditPoliciesList { get; set; } = new ObservableCollection<CreditPolicy>();

        public CreditPolicy _selectedCreditPolicy;

        public CreditPolicy _backUpCreditPolicy;
        public CreditPolicyQuery Query { get; set; } = new CreditPolicyQuery();

        private ICreditPolicyService _service;

        public IDialogService _dialogService;

        public INavigationService _navigationService;
        public ICommand GoToPageCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand SaveChangesCommand { get; set; }
        public ICommand CancelEditCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand RestoreCommand { get; set; }                
        
        private bool _isOrderedAscendent = false;        

        private bool _isAddButtonVisible = true;
        private bool _isSaveButtonVisible = false;
        private bool _isCancelButtonVisible = false;
        private bool _isEditButtonVisible = true;
        private bool _isSaveChangesVisible = false;
        private bool _isCancelEditButtonVisible = false;

        public ObservableCollection<CreditPolicyStates> StatesList { get; set; } = new ObservableCollection<CreditPolicyStates>(Enum.GetValues(typeof(CreditPolicyStates)).Cast<CreditPolicyStates>());
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
        public bool IsAddButtonVisible
        {
            get => _isAddButtonVisible;
            set
            {
                if (_isAddButtonVisible != value)
                {
                    _isAddButtonVisible = value;
                    OnPropertyChanged(nameof(IsAddButtonVisible));
                }
            }
        }
        public bool IsSaveButtonVisible
        {
            get => _isSaveButtonVisible;
            set
            {
                if (_isSaveButtonVisible != value)
                {
                    _isSaveButtonVisible = value;
                    OnPropertyChanged(nameof(IsSaveButtonVisible));
                }
            }
        }
        public bool IsCancelButtonVisible
        {
            get => _isCancelButtonVisible;
            set
            {
                if (_isCancelButtonVisible != value)
                {
                    _isCancelButtonVisible = value;
                    OnPropertyChanged(nameof(IsCancelButtonVisible));
                }
            }
        }

        public bool IsEditButtonVisible
        {
            get => _isEditButtonVisible;
            set
            {
                if (_isEditButtonVisible != value)
                {
                    _isEditButtonVisible = value;
                    OnPropertyChanged(nameof(IsEditButtonVisible));
                }
            }
        }

        public bool IsSaveChangesButtonVisible
        {
            get => _isSaveChangesVisible;
            set
            {
                if (_isSaveChangesVisible != value)
                {
                    _isSaveChangesVisible = value;
                    OnPropertyChanged(nameof(IsSaveChangesButtonVisible));
                }
            }
        }

        public bool IsCancelEditButtonVisible
        {
            get => _isCancelEditButtonVisible;
            set
            {
                if (_isCancelEditButtonVisible != value)
                {
                    _isCancelEditButtonVisible = value;
                    OnPropertyChanged(nameof(IsCancelEditButtonVisible));
                }
            }
        }

        private KeyValuePair<string, CreditPolicyStates?> _selectedStateFilter;

        private KeyValuePair<string, bool?> _selectedValidityFilter;
        public CreditPoliciesManagementViewModel() { }
        public CreditPoliciesManagementViewModel(IDialogService dialogService, INavigationService navigationService, ICreditPolicyService creditPolicyService)
        {            
            _dialogService = dialogService;

            _navigationService = navigationService;

            _service = creditPolicyService;

            GoToPageCommand = new RelayCommand(ChangePage, CanDoIt);
            
            Pagination = new PaginationViewModelBase(_service.CountCreditPolicies(Query));

            Query = new CreditPolicyQuery { PageNumber = Pagination.PageNumber, PageSize = Pagination.PageSize };

            SelectedStateFilter = StateFilterOptions.First();

            SelectedValidityFilter = ValidityFilterOptions.First();

            EditCommand = new RelayCommand(Edit, CanDoIt);
            CancelCommand = new RelayCommand(Cancel, CanDoIt);
            SaveCommand = new RelayCommand(Save, CanDoIt);
            RegisterCommand = new RelayCommand(Register, CanDoIt);
            SaveChangesCommand = new RelayCommand(SaveChanges, CanDoIt);
            CancelEditCommand = new RelayCommand(CancelEdit, CanDoIt);

            SearchCommand = new RelayCommand(Search, CanDoIt);

            RestoreCommand = new RelayCommand(Restore, CanDoIt);
            
            Search(null);       

            _selectedCreditPolicy = new CreditPolicy();
        }

        private void CloneCreditPolicy()
        {
            _backUpCreditPolicy = new CreditPolicy
            {
                CreditPolicyId = _selectedCreditPolicy.CreditPolicyId,
                Name = _selectedCreditPolicy.Name,
                Description = _selectedCreditPolicy.Description,
                RegistrationDate = _selectedCreditPolicy.RegistrationDate,
                EndDate = _selectedCreditPolicy.EndDate,
                Status = _selectedCreditPolicy.Status,                
            };
        }

        private void CopyFrom()
        {
            if (_backUpCreditPolicy != null)
            {
                _selectedCreditPolicy.CreditPolicyId = _backUpCreditPolicy.CreditPolicyId;
                _selectedCreditPolicy.Name = _backUpCreditPolicy.Name;
                _selectedCreditPolicy.Description = _backUpCreditPolicy.Description;
                _selectedCreditPolicy.RegistrationDate = _backUpCreditPolicy.RegistrationDate;
                _selectedCreditPolicy.EndDate = _backUpCreditPolicy.EndDate;
                _selectedCreditPolicy.Status = _backUpCreditPolicy.Status;
            }
            _backUpCreditPolicy = null;
        }

        private bool IsInputValid()
        {
            if (string.IsNullOrWhiteSpace(SelectedCreditPolicy.Name))
            {
                MessageBox.Show("El nombre de la política es obligatorio.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(SelectedCreditPolicy.Description))
            {
                MessageBox.Show("La descripción de la política es obligatoria.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }           
            return true;
        }

        private void Register(object obj)
        {
            SelectedCreditPolicy = new CreditPolicy
            {
                RegistrationDate = DateTime.Today,
                EndDate = DateTime.Today,
                IsEditable = true
            };

            IsAddButtonVisible = false;
            IsSaveButtonVisible = true;
            IsCancelButtonVisible = true;
            IsEditButtonVisible = false;            
        }

        private void Save(object obj)
        {
            try
            {
                if (IsInputValid())
                {
                    if (_service.AddCreditPolicy(_selectedCreditPolicy) > 0)
                    {
                        MessageBox.Show("Política de crédito guardada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        IsAddButtonVisible = true;
                        IsEditButtonVisible = true;
                        IsSaveButtonVisible = false;
                        IsCancelButtonVisible = false;
                        SelectedCreditPolicy = new CreditPolicy();
                        Search(null);
                    }
                    else
                    {
                        MessageBox.Show("No se pudo guardar la política de crédito.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }                
            }
            catch (ArgumentException exception)
            {
                _dialogService.Dismiss(exception.Message, System.Windows.MessageBoxImage.Exclamation);
            }
        }

        private void Cancel(object obj)
        {            
            SelectedCreditPolicy = new CreditPolicy
            {
                IsEditable = false
            };
            IsAddButtonVisible = true;
            IsSaveButtonVisible = false;
            IsCancelButtonVisible = false;
            IsEditButtonVisible = true;
        }

        private void Edit(object obj)
        {
            if (_selectedCreditPolicy.CreditPolicyId > 0)
            {
                _selectedCreditPolicy.IsEditable = true;
                IsAddButtonVisible = false;
                IsEditButtonVisible = false;
                IsSaveChangesButtonVisible = true;
                IsCancelEditButtonVisible = true;
                CloneCreditPolicy();
            }                               
        }

        private void SaveChanges(object obj)
        {            
            try
            {
                if (IsInputValid())
                {
                    if (_service.UpdateCreditPolicy(_selectedCreditPolicy) > 0)
                    {
                        MessageBox.Show("Política de crédito modificada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        _selectedCreditPolicy.IsEditable = false;
                        IsAddButtonVisible = true;
                        IsEditButtonVisible = true;
                        IsSaveChangesButtonVisible = false;
                        IsCancelEditButtonVisible = false;
                    }
                    else
                    {
                        MessageBox.Show("No se pudo modificar la política de crédito.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }                
            }
            catch (ArgumentException exception)
            {
                _dialogService.Dismiss(exception.Message, System.Windows.MessageBoxImage.Exclamation);
            }

        }

        private void CancelEdit(object obj)
        {            
            if (_selectedCreditPolicy != null)
            {
                _selectedCreditPolicy.IsEditable = false;
                IsAddButtonVisible = true;
                IsEditButtonVisible = true;
                IsSaveChangesButtonVisible = false;
                IsCancelEditButtonVisible = false;
                CopyFrom();
            }
        }

        private bool CanDoIt(object obj)
        {
            return true;
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
                Query.Name = string.Empty;
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
                    if (_selectedCreditPolicy != null)
                    {
                        _selectedCreditPolicy.IsEditable = false;
                    }                        
                    _selectedCreditPolicy = value;
                    OnPropertyChanged(nameof(SelectedCreditPolicy));
                }
            }
        }

        public KeyValuePair<string, CreditPolicyStates?> SelectedStateFilter
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

        public KeyValuePair<string, bool?> SelectedValidityFilter
        {
            get => _selectedValidityFilter;
            set
            {
                if (_selectedValidityFilter.Equals(value))
                {
                    return;
                }
                _selectedValidityFilter = value;
                Query.Validity = _selectedValidityFilter.Value;
                OnPropertyChanged(nameof(SelectedValidityFilter));
            }
        }
    }
}