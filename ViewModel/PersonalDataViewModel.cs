using Independiente.Commands;
using Independiente.Model;
using Independiente.Properties;
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
        public IPerson Person { get; set; }

        public AddressData AddressData { get; set; }
        private RegistrationType _registrationType { get; set; }
        private PageMode _pageMode { get; set; }

        public List<string> StatesList { get; set; }

        private IDialogService _dialogService { get; set; }

        private INavigationService _navigationService { get; set; }
        
        private IClientManagementService _clientManagementService { get; set; } 

        public PersonalDataViewModel()
        {

        }

        public PersonalDataViewModel(IDialogService dialogService, INavigationService navigationService, PageMode mode, RegistrationType type, IPerson person, IClientManagementService clientManagementService )
        {
            NextCommand = new RelayCommand(Next, CanNext);
            EditCommand = new RelayCommand(Edit, CanNext);
            CancelCommand = new RelayCommand(Cancel, CanNext);
            SaveCommand = new RelayCommand(Save, CanNext);
            GoBackCommand = new RelayCommand(GoBack, CanNext);
            AddressData = new AddressData();
            LoadStates();
            _dialogService = dialogService;
            _navigationService = navigationService;
            SwitchMode(mode);
            _registrationType = type;
            _pageMode = mode;
            _clientManagementService = clientManagementService;
            Person = person;
        }

        private void GoBack(object obj)
        {
            _navigationService.GoBack();
        }

        private void LoadStates()
        {
            StatesList = new List<string>();
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
            string message = string.Empty;
            bool validation = false;

            switch (_registrationType)
            {
                case RegistrationType.Client:
                    try
                    {
                        if (_clientManagementService.ValidateAddressData(AddressData) &&
                            _clientManagementService.ValidatePersonalData(Person.PersonalData))
                        {
                            if (!_clientManagementService.IsRFCRegistered(Person.PersonalData.RFC, out message) &&
                                !_clientManagementService.IsPhoneNumberRepeated(Person.PersonalData.PhoneNumber, Person.PersonalData.AlternativePhoneNumber, out message) &&
                                _clientManagementService.IsValidAge(Person.PersonalData.BirthDate.Value, out message))
                            {
                                validation = true;
                            }
                        }
                    }
                    catch (ArgumentException exception)
                    {
                        message = exception.Message;
                    }
                    break;
                case RegistrationType.Employee:
                    break;
            }

            Console.WriteLine(Person.PersonalData.Name);

            if (validation)
            {
                _navigationService.NavigateTo<FinancialDataViewModel>(new PersonDataParams(_pageMode, Person));
            }
            else
            {
                _dialogService.Dismiss(message);
            }
        }

        private void Cancel(object obj)
        {
            SwitchMode(PageMode.View);
        }

        private void Save(object obj)
        {
            Console.WriteLine(Person.PersonalData.ToString());
            Console.WriteLine(AddressData.ToString());

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
