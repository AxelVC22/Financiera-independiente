using Independiente.Commands;
using Independiente.Model;
using Independiente.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Controls;
using System.Windows.Input;

namespace Independiente.ViewModel
{

    enum CatalogType
    {
        Bank,
    }

    public class CatalogsViewModel : ModificableViewModel
    {
        public ObservableCollection<ICatalog> CatalogsList { get; set; }

        public List<string> CatalogsTypesList { get; set; }

        private INavigationService _navigationService { get; set; }
        private IDialogService _dialogService { get; set; }

        private ICatalog _selectedCatalog;

        private string _selectedCatalogType;

        public ICommand RegisterCommand { get; set; }
        public ICatalog SelectedCatalog
        {
            get => _selectedCatalog;
            set
            {
                if (_selectedCatalog != value)
                {
                    _selectedCatalog = value;
                    OnPropertyChanged(nameof(SelectedCatalog));
                }
            }
        }

        public string SelectedCatalogType
        {
            get => _selectedCatalogType;
            set
            {
                if (_selectedCatalogType != value)
                {
                    _selectedCatalogType = value;
                    OnPropertyChanged(nameof(SelectedCatalogType));
                }
            }
        }

        private void GoBack(object obj)
        {
            _navigationService.GoBack();
        }

        public CatalogsViewModel() { }
        public CatalogsViewModel(IDialogService dialogService, INavigationService navigationService)
        {
            CatalogsList = new ObservableCollection<ICatalog>
                {
                    new Bank { BankId = 1, Name = "Banco A" },
                new Bank { BankId = 2, Name = "Banco B" },
                    new Bank { BankId = 3, Name = "Banco C" },
                };

            EditCommand = new RelayCommand(Edit, CanDoIt);
            CancelCommand = new RelayCommand(Cancel, CanDoIt);
            SaveCommand = new RelayCommand(Save, CanDoIt);
            GoBackCommand = new RelayCommand(GoBack, CanDoIt);
            RegisterCommand = new RelayCommand(Register, CanDoIt);
            _navigationService = navigationService;
            _dialogService = dialogService;

            CatalogsTypesList = Enum.GetNames(typeof(CatalogType)).ToList();

        }

        private void Cancel(object obj)
        {
            if (obj is Bank bank)
            {

                bank.IsEditable = false;

            }

        }

        private void Save(object obj)
        {
            if (obj is Bank bank)
            {
                if (!string.IsNullOrEmpty(bank.Name))
                {
                    if (bank.BankId >= 0)
                    {
                        bank.IsEditable = false;
                    }
                }
            }
        }

        private void Edit(object obj)
        {
            if (obj is Bank bank)
            {
                bank.IsEditable = true;

            }

        }

        private void Register(object obj)
        {
            ICatalog catalog = CatalogsList.LastOrDefault();

            if (catalog is Bank bank)
            {
                if (bank.BankId != 0 || !string.IsNullOrEmpty(bank.Name))
                {
                    CatalogsList.Add(new Bank { IsEditable = true });
                }
            }
        }

        private bool CanDoIt(object obj)
        {
            return true;
        }
    }
}
