using Independiente.Commands;
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
        public IDialogService _dialogService;

        public INavigationService _navigationService;

        public ICommand RegisterCommand { get; set; }

        public ICommand OrderByNameCommand { get; set; }

        public ICommand OrderByDescriptionCommand { get; set; }

        private bool _isOrderedAscendent = false;

        public List<CreditPolicyStates> StatesList { get; set; }

        public ObservableCollection<CreditPolicy> CreditPoliciesList { get; set; }

        public CreditPolicy _selectedCreditPolicy;

        public int _paginationIndex;


        public CreditPoliciesManagementViewModel() { }
        public CreditPoliciesManagementViewModel(IDialogService dialogService, INavigationService navigationService)
        {
            CreditPoliciesList = new ObservableCollection<CreditPolicy>
            {

            };
            _dialogService = dialogService;
            _navigationService = navigationService;
            EditCommand = new RelayCommand(Edit, CanDoIt);
            CancelCommand = new RelayCommand(Cancel, CanDoIt);
            SaveCommand = new RelayCommand(Save, CanDoIt);
            RegisterCommand = new RelayCommand(Register, CanDoIt);
            OrderByNameCommand = new RelayCommand(OrderByName, CanDoIt);
            StatesList = Enum.GetValues(typeof(CreditPolicyStates)).Cast<CreditPolicyStates>().ToList();
            _selectedCreditPolicy = new CreditPolicy();
            PaginationIndex = 1;

        }

        private void Edit(object obj)
        {
            _selectedCreditPolicy.IsEditable = true;
        }

        private void Cancel(object obj)
        {
            _selectedCreditPolicy.IsEditable = false;
        }

        private void Save(object obj)
        {
            if (obj is CreditPolicy creditPolicy)
            {
                creditPolicy.IsEditable = false;
            }
        }

        private bool CanDoIt(object obj)
        {
            return true;
        }

        private void Register(object obj)
        {
            CreditPolicy newCreditPolicy = new CreditPolicy { IsEditable = true, RegistrationDate = DateTime.Today };
            CreditPoliciesList.Add(newCreditPolicy);
        }

        private void OrderByName(object obj)
        {
            List<CreditPolicy> listaOrdenada = new List<CreditPolicy>();
            if (_isOrderedAscendent)
            {
                listaOrdenada = CreditPoliciesList.OrderByDescending(x => x.Name).ToList();
                _isOrderedAscendent = false;
            }
            else
            {
                listaOrdenada = CreditPoliciesList.OrderBy(x => x.Name).ToList();
                _isOrderedAscendent = true;
            }
            CreditPoliciesList = new ObservableCollection<CreditPolicy>(listaOrdenada);
            OnPropertyChanged(nameof(CreditPoliciesList));
        }

        public ICommand OrderByCommand => new RelayCommand(param =>
        {
            switch (param as string)
            {
                case "Name":
                    OrderByProperty(x => x.Name);
                    break;
                case "Description":
                    OrderByProperty(x => x.Description);
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

        public int PaginationIndex
        {
            get => _paginationIndex;
            set
            {
                if (_paginationIndex != value)
                {
                    _paginationIndex = value;
                    OnPropertyChanged(nameof(PaginationIndex));
                }
            }
        }

    }
}
