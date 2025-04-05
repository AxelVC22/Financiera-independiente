using Independiente.Commands;
using Independiente.Model;
using Independiente.Services;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Independiente.ViewModel
{
    public class PersonalDataViewModel : ModificableViewModel
    {
        public PersonalData PersonalData { get; set; }

        public AddressData AddressData { get; set; }
        private RegistrationType _registrationType { get; set; }
        private PageMode _pageMode { get; set; }

        private List<string> _statesList;

        private string _selectedState;

        public event EventHandler RequestClose;

        private IDialogService _dialogService { get; set; }

        private INavigationService _navigationService { get; set; }

        public PersonalDataViewModel()
        {
           
        }

        public PersonalDataViewModel(IDialogService dialogService, INavigationService navigationService, PageMode mode, RegistrationType type)
        {
            NextCommand = new RelayCommand(Next, CanNext);
            EditCommand = new RelayCommand(Edit, CanNext);
            CancelCommand = new RelayCommand(Cancel, CanNext);
            SaveCommand = new RelayCommand(Save, CanNext);
            GoBackCommand = new RelayCommand(GoBack, CanNext);
            PersonalData = new PersonalData();
            LoadStates();
            _dialogService = dialogService;
            _navigationService = navigationService;
            SwitchMode(mode);
            _registrationType = type;
            _pageMode = mode;
        }

        private void GoBack(object obj)
        {
            _navigationService.GoBack();
        }

        private void LoadStates()
        {
            var resourceManager = new ResourceManager("Independiente.Properties.States", typeof(PersonalDataViewModel).Assembly);

            var resourceSet = resourceManager.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, true, true);

            var states = resourceSet.Cast<DictionaryEntry>()
                                    .Where(entry => entry.Value is string)
                                    .Select(entry => entry.Value.ToString())
                                    .ToList();
            StatesList = states;
        }

        private void Next(object obj)
        {
            switch (_registrationType)
            {
                case RegistrationType.Client:
                    break;
                case RegistrationType.Employee:
                    break;
            }

            if (PersonalData.Name != null && PersonalData.Name.Length > 0)
            {
                _navigationService.NavigateTo<FinancialDataViewModel>(new PersonalDataParams(_pageMode));
            }
            else
            {
                IDialogService dialogService = new DialogService();
                dialogService.Dismiss("gg");
            }
        }

        private void Cancel(object obj)
        {
            SwitchMode(PageMode.View);
        }

        private void Save(object obj)
        {

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

        public List<string> StatesList
        {
            get => _statesList;
            set
            {
                _statesList = value;
                OnPropertyChanged(nameof(StatesList));
            }
        }


        public string SelectedState
        {
            get => _selectedState;
            set
            {
                if (_selectedState != value)
                {
                    _selectedState = value;
                    OnPropertyChanged(nameof(SelectedState));
                }
            }
        }
    }
}
