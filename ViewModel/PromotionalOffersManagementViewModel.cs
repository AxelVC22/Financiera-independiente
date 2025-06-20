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
    public class PromotionalOffersManagementViewModel : ModificableViewModel
    {
        public PaginationViewModelBase Pagination { get; }
        public ObservableCollection<PromotionalOffer> PromotionalOffersList { get; set; } = new ObservableCollection<PromotionalOffer>();
        
        public PromotionalOffer _selectedPromotionalOffer;

        public PromotionalOffer _backUpPromotionalOffer;
        public PromotionalOfferQuery Query { get; set; } = new PromotionalOfferQuery();

        private IPromotionalOfferService _service;

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

        private bool _isOrderedAscendent = false;

        private bool _isAddButtonVisible = true;
        private bool _isSaveButtonVisible = false;
        private bool _isCancelButtonVisible = false;
        private bool _isEditButtonVisible = true;
        private bool _isSaveChangesVisible = false;
        private bool _isCancelEditButtonVisible = false;
        public ObservableCollection<PromotionalOfferStates> StatesList { get; set; } = new ObservableCollection<PromotionalOfferStates>(Enum.GetValues(typeof(PromotionalOfferStates)).Cast<PromotionalOfferStates>());
        public ObservableCollection<PromotionalOfferPaymentFrequencies> PaymentFrequenciesList { get; set; } = new ObservableCollection<PromotionalOfferPaymentFrequencies>(Enum.GetValues(typeof(PromotionalOfferPaymentFrequencies)).Cast<PromotionalOfferPaymentFrequencies>());
        public Dictionary<string, PromotionalOfferStates?> StateFilterOptions { get; } = new Dictionary<string, PromotionalOfferStates?>
        {
            { "Todos", null },
            { "Activo", PromotionalOfferStates.Active },
            { "Inactivo", PromotionalOfferStates.Inactive }
        };
        private KeyValuePair<string, PromotionalOfferStates?> _selectedStateFilter;
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
        public PromotionalOffersManagementViewModel() { }
        public PromotionalOffersManagementViewModel(IDialogService dialogService, INavigationService navigationService, IPromotionalOfferService promotionalOfferService)
        {            
            _dialogService = dialogService;
            _navigationService = navigationService;
            _service = promotionalOfferService;
            GoToPageCommand = new RelayCommand(ChangePage, CanDoIt);
            Pagination = new PaginationViewModelBase(_service.CountPromotionalOffers(Query));
            Query = new PromotionalOfferQuery { PageNumber = Pagination.PageNumber, PageSize = Pagination.PageSize };
            SelectedStateFilter = StateFilterOptions.First();
            EditCommand = new RelayCommand(Edit, CanDoIt);
            CancelCommand = new RelayCommand(Cancel, CanDoIt);
            SaveCommand = new RelayCommand(Save, CanDoIt);
            RegisterCommand = new RelayCommand(Register, CanDoIt);
            SaveChangesCommand = new RelayCommand(SaveChanges, CanDoIt);
            CancelEditCommand = new RelayCommand(CancelEdit, CanDoIt);
            SearchCommand = new RelayCommand(Search, CanDoIt);            
            Search(null);
            _selectedPromotionalOffer = new PromotionalOffer();
        }
        private void CloneCreditPolicy()
        {
            _backUpPromotionalOffer = new PromotionalOffer
            {
                PromotionalOfferId = _selectedPromotionalOffer.PromotionalOfferId,
                InteresRate = _selectedPromotionalOffer.InteresRate,
                LoanTerm = _selectedPromotionalOffer.LoanTerm,
                PaymenteFrecuency = _selectedPromotionalOffer.PaymenteFrecuency,
                IVA = _selectedPromotionalOffer.IVA,
                Status = _selectedPromotionalOffer.Status,
                Name = _selectedPromotionalOffer.Name,
            };
        }

        private void CopyFrom()
        {
            if (_backUpPromotionalOffer != null)
            {
                _selectedPromotionalOffer.PromotionalOfferId = _backUpPromotionalOffer.PromotionalOfferId;
                _selectedPromotionalOffer.InteresRate = _backUpPromotionalOffer.InteresRate;
                _selectedPromotionalOffer.LoanTerm = _backUpPromotionalOffer.LoanTerm;
                _selectedPromotionalOffer.PaymenteFrecuency = _backUpPromotionalOffer.PaymenteFrecuency;
                _selectedPromotionalOffer.IVA = _backUpPromotionalOffer.IVA;
                _selectedPromotionalOffer.Status = _backUpPromotionalOffer.Status;
                _selectedPromotionalOffer.Name = _backUpPromotionalOffer.Name;
            }
            _backUpPromotionalOffer = null;
        }
        private bool IsInputValid()
        {
            if (string.IsNullOrWhiteSpace(SelectedPromotionalOffer.Name))
            {
                MessageBox.Show("El nombre de la promoción es obligatorio.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }            
            if (!SelectedPromotionalOffer.LoanTerm.HasValue || SelectedPromotionalOffer.LoanTerm.Value <= 0 || SelectedPromotionalOffer.LoanTerm.Value > 99)
            {
                MessageBox.Show("El plazo del préstamo es obligatorio y debe ser un número entero positivo entre 1 y 99.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }            
            if (!SelectedPromotionalOffer.InteresRate.HasValue || SelectedPromotionalOffer.InteresRate.Value < 0 || SelectedPromotionalOffer.InteresRate.Value > 99)
            {
                MessageBox.Show("La tasa de interés es obligatoria y debe ser un número decimal entre 0 y 99.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (!SelectedPromotionalOffer.IVA.HasValue || SelectedPromotionalOffer.IVA.Value < 0 || SelectedPromotionalOffer.IVA.Value > 99)
            {
                MessageBox.Show("El IVA es obligatorio y debe ser un número decimal entre 0 y 99.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private void Register(object obj)
        {
            SelectedPromotionalOffer = new PromotionalOffer
            {                
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
                    if (_service.AddPromotionalOffer(_selectedPromotionalOffer) > 0)
                    {
                        MessageBox.Show("Política de crédito guardada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        IsAddButtonVisible = true;
                        IsEditButtonVisible = true;
                        IsSaveButtonVisible = false;
                        IsCancelButtonVisible = false;
                        SelectedPromotionalOffer = new PromotionalOffer();
                        Search(null);
                    }
                    else
                    {
                        MessageBox.Show("No se pudo guardar la promoción.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            SelectedPromotionalOffer = new PromotionalOffer
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
            if (_selectedPromotionalOffer.PromotionalOfferId > 0)
            {
                _selectedPromotionalOffer.IsEditable = true;
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
                    if (_service.UpdatePromotionalOffer(_selectedPromotionalOffer) > 0)
                    {
                        MessageBox.Show("Promoción modificada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        _selectedPromotionalOffer.IsEditable = false;
                        IsAddButtonVisible = true;
                        IsEditButtonVisible = true;
                        IsSaveChangesButtonVisible = false;
                        IsCancelEditButtonVisible = false;
                    }
                    else
                    {
                        MessageBox.Show("No se pudo modificar la promoción.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            if (_selectedPromotionalOffer != null)
            {
                _selectedPromotionalOffer.IsEditable = false;
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
        private void Search(object obj)
        {
            try
            {
                PromotionalOffersList.Clear();

                Pagination.TotalItems = _service.CountPromotionalOffers(Query);

                if (Pagination.TotalItems > 0)
                {

                    var promotionalOffers = _service.GetPromotionalOffers(Query);

                    if (promotionalOffers != null && promotionalOffers.Count > 0)
                    {
                        foreach (var c in promotionalOffers)
                        {
                            PromotionalOffersList.Add(c);
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
                case "Plazo":
                    OrderByProperty(x => x.LoanTerm);
                    break;                
                case "Status":
                    OrderByProperty(x => x.Status);
                    break;
            }
        }, CanDoIt);
        private void OrderByProperty(Func<PromotionalOffer, object> keySelector)
        {
            IEnumerable<PromotionalOffer> listaOrdenada;

            if (_isOrderedAscendent)
            {
                listaOrdenada = PromotionalOffersList.OrderByDescending(keySelector);
            }
            else
            {
                listaOrdenada = PromotionalOffersList.OrderBy(keySelector);
            }
            _isOrderedAscendent = !_isOrderedAscendent;
            PromotionalOffersList = new ObservableCollection<PromotionalOffer>(listaOrdenada);
            OnPropertyChanged(nameof(PromotionalOffersList));
        }
        public PromotionalOffer SelectedPromotionalOffer
        {
            get => _selectedPromotionalOffer;
            set
            {
                if (_selectedPromotionalOffer != value)
                {
                    if (_selectedPromotionalOffer != null)
                    {
                        _selectedPromotionalOffer.IsEditable = false;
                    }
                    _selectedPromotionalOffer = value;
                    OnPropertyChanged(nameof(SelectedPromotionalOffer));
                }
            }
        }
        public KeyValuePair<string, PromotionalOfferStates?> SelectedStateFilter
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
