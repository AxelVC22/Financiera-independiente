using Independiente.Commands;
using Independiente.DataAccess.Repositories;
using Independiente.Model;
using Independiente.Properties;
using Independiente.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Independiente.ViewModel
{
    public class PaymentsViewModel : BaseViewModel
    {
        public PaginationViewModelBase Pagination { get; }

        public ObservableCollection<Bank> BanksList { get; set; }

        private Bank _selectedBank;

        private Bank _selectedBankForLayout;


        public ObservableCollection<Payment> PaymentsList { get; set; } = new ObservableCollection<Payment>();

        public IDialogService _dialogService;

        public ICatalogService _catalogService;

        public INavigationService _navigationService;
        public ICommand GoToPageCommand { get; set; }

        public ICommand OrderByNameCommand { get; set; }

        public ICommand SearchCommand { get; set; }

        public ICommand RestoreCommand { get; set; }

        public ICommand CheckCommand { get; set; }

        public ICommand GenerateCommand { get; set; }

        private IPaymentService _paymentService;

        private IFilePickerService _filePickerService;

        public PaymentQuery Query { get; set; } = new PaymentQuery();

        public ChargeQuery ChargeQuery { get; set; } = new ChargeQuery();

        public CatalogQuery CatalogQuery { get; set; } = new CatalogQuery();

        private bool _isOrderedAscendent = false;

        public ICommand SaveCommand { get; set; }

        public Dictionary<string, PaymentStatus?> StateFilterOptions { get; } = new Dictionary<string, PaymentStatus?>
        {
            { "Todos", null },
            { "Pendiente", PaymentStatus.Pending },
            { "Completado", PaymentStatus.Completed },
        };

        private KeyValuePair<string, PaymentStatus?> _selectedStateFilter;


        public PaymentsViewModel()
        {

        }

        public PaymentsViewModel(IDialogService dialogService, INavigationService navigationService, IPaymentService paymentService, ICatalogService catalogService, IFilePickerService filePickerService)
        {
            _dialogService = dialogService;
            _navigationService = navigationService;
            _paymentService = paymentService;
            _catalogService = catalogService;
            _filePickerService = filePickerService;

            GoToPageCommand = new RelayCommand(ChangePage, CanDoIt);

            GoBackCommand = new RelayCommand(GoBack, CanDoIt);

            GenerateCommand = new RelayCommand(Generate, CanDoIt);

            SaveCommand = new RelayCommand(Save, CanDoIt);

            Pagination = new PaginationViewModelBase(_paymentService.CountPayments(Query));

            Query = new PaymentQuery { PageNumber = Pagination.PageNumber, PageSize = Pagination.PageSize };

            SearchCommand = new RelayCommand(Search, CanDoIt);

            RestoreCommand = new RelayCommand(Restore, CanDoIt);

            SelectedStateFilter = StateFilterOptions.First();

            BanksList = new ObservableCollection<Bank>(_catalogService.GetBanks(new CatalogQuery()));

            BanksList.Insert(0, new Bank { });

            Search(null);

        }


        private void Save(object obj)
        {
            if (obj is Payment payment)
            {
                string path = _filePickerService.PickFile();

                if (!string.IsNullOrEmpty(path) && Path.GetExtension(path).Equals(".csv", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        if (_paymentService.UploadCharges(path, payment) > 0)
                        {
                            _dialogService.Dismiss("El pago de los creditos ha sido subido correctamente", System.Windows.MessageBoxImage.Information);
                            Search(null);
                        }
                    }
                    catch (ArgumentException ex)
                    {
                        _dialogService.Dismiss(ex.Message, System.Windows.MessageBoxImage.Information);
                    }
                    catch (InvalidOperationException ex)
                    {
                        _dialogService.Dismiss(ex.Message, System.Windows.MessageBoxImage.Warning);
                    }
                }
                else
                {
                    _dialogService.Dismiss("Solo se permiten archivos .CSV", System.Windows.MessageBoxImage.Information);
                }
            }
        }

        private bool CanDoIt(object obj)
        {
            return true;
        }

        private void GoBack(object obj)
        {
            _navigationService.GoBack();
        }


        private void Generate(object obj)
        {

            Console.WriteLine(Messages.ResourceManager.GetString("LayoutGeneratedMessage"));
            string ruta = _filePickerService.SelectPath();

            if (!string.IsNullOrEmpty(ruta))
            {
                try
                {
                    if (_paymentService.GenerateLayout(ruta, ChargeQuery))
                    {
                        Search(null);
                        _dialogService.Dismiss("El layout de cobro fue generado correctamente", System.Windows.MessageBoxImage.Information);
                    }
                }
                catch (ArgumentException e)
                {
                    _dialogService.Dismiss(e.Message, System.Windows.MessageBoxImage.Information);
                }
                catch (InvalidOperationException e)
                {
                    _dialogService.Dismiss(e.Message, System.Windows.MessageBoxImage.Warning);
                }
            }

        }

        private void Restore(object obj)
        {
            Query.FromDate = null;
            Query.ToDate = null;
            Query.BankName = null;

            SelectedStateFilter = StateFilterOptions.First();

            Search(null);
        }

        private void Search(object obj)
        {
            try
            {
                PaymentsList.Clear();

                Pagination.TotalItems = _paymentService.CountPayments(Query);

                if (Pagination.TotalItems > 0)
                {

                    var payments = _paymentService.GetPayments(Query);

                    if (payments != null && payments.Count > 0)
                    {
                        foreach (var c in payments)
                        {
                            PaymentsList.Add(c);
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
                case "Name":
                    OrderByProperty(x => x.Employee.PersonalData.Name);
                    break;
                case "TotalAmount":
                    OrderByProperty(x => x.TotalAmount);
                    break;

                case "ActualAmount":
                    OrderByProperty(x => x.ActualAmount);
                    break;
                case "RegistrationDate":
                    OrderByProperty(x => x.RegistrationDate);
                    break;
                case "Status":
                    OrderByProperty(x => x.Status);
                    break;
                case "Bank":
                    OrderByProperty(x => x.Bank.Name);
                    break;
                case "UploadDate":
                    OrderByProperty(x => x.UploadDate);
                    break;

                case "TotalCredits":
                    OrderByProperty(x => x.TotalCredits);
                    break;
                case "ActualCredits":
                    OrderByProperty(x => x.ActualCredits);
                    break;
            }
        }, CanDoIt);

        private void OrderByProperty(Func<Payment, object> keySelector)
        {
            IEnumerable<Payment> orderedList;

            if (_isOrderedAscendent)
            {
                orderedList = PaymentsList.OrderByDescending(keySelector);
            }
            else
            {
                orderedList = PaymentsList.OrderBy(keySelector);
            }
            _isOrderedAscendent = !_isOrderedAscendent;
            PaymentsList = new ObservableCollection<Payment>(orderedList);
            OnPropertyChanged(nameof(PaymentsList));
        }

        public KeyValuePair<string, PaymentStatus?> SelectedStateFilter
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

        public Bank SelectedBank
        {
            get => _selectedBank;
            set
            {
                if (_selectedBank != value)
                {
                    _selectedBank = value;
                    Query.BankName = _selectedBank?.Name;
                    OnPropertyChanged(nameof(SelectedBank));
                }
            }
        }

        public Bank SelectedBankForLayout
        {
            get => _selectedBankForLayout;
            set
            {
                if (_selectedBankForLayout != value)
                {
                    _selectedBankForLayout = value;
                    ChargeQuery.BankName = _selectedBankForLayout?.Name;
                    OnPropertyChanged(nameof(SelectedBankForLayout));
                }
            }
        }

    }
}
