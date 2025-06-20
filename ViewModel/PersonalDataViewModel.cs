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
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Independiente.ViewModel
{
    public class PersonalDataViewModel : ModificableViewModel
    {
        public IPerson Person { get; set; }
        public IPerson OldPerson { get; set; }
        public Model.Client ClientToUpdate { get; set; }
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
            LoadStates();
            _dialogService = dialogService;
            _navigationService = navigationService;
            SwitchMode(mode);
            _registrationType = type;
            _pageMode = mode;
            _clientManagementService = clientManagementService;
            Person = person;
            if (_pageMode == PageMode.View)
            {
                ClientToUpdate = new Model.Client();
                OldPerson = CreatePersonCopy(Person);
                ClientToUpdate.PersonalData = person.PersonalData;
                ClientToUpdate.AddressData = person.AddressData;

                if (person is Client client)
                {
                    ClientToUpdate.FirstReference = client.FirstReference;
                    ClientToUpdate.SecondReference = client.SecondReference;
                    ClientToUpdate.WorkCenter = client.WorkCenter;
                    ClientToUpdate.DepositAccount = client.DepositAccount;
                    ClientToUpdate.PaymentAccount = client.PaymentAccount;
                    ClientToUpdate.Employee = client.Employee;
                    ClientToUpdate.ClientId = client.ClientId;
                }
            }

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
                                    .OrderBy(s => s)
                                    .ToList();

            StatesList = states;
        }


        private void Next(object obj)
        {
            string message = string.Empty;
            bool validation = false;
            Model.Client client = new Model.Client();
            client.PersonalData = Person.PersonalData;
            client.AddressData = Person.AddressData;

            if (_pageMode == PageMode.Registration)
            {
                switch (_registrationType)
                {
                    case RegistrationType.Client:
                        try
                        {
                            if (_clientManagementService.ValidateAddressData(Person.AddressData) &&
                                _clientManagementService.ValidatePersonalData(Person.PersonalData))
                            {
                                if (!_clientManagementService.IsPhoneNumberRepeated(Person.PersonalData.PhoneNumber, Person.PersonalData.AlternativePhoneNumber, out message) &&
                                    !_clientManagementService.IsRFCRegistered(Person.PersonalData.RFC, Person.PersonalData.PersonalDataId, out message) &&
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
                        /* Logica para empleados 
                         ////////
                         ///////
                         //////
                         /////
                         ///
                         //
                         */
                        break;
                }

                if (validation)
                {
                    _navigationService.NavigateTo<FinancialDataViewModel>(new ClientDataParams(_pageMode, client));
                }
                else
                {
                    _dialogService.Dismiss(message, System.Windows.MessageBoxImage.Information);
                }
            }
            else if (_pageMode == PageMode.View)
            {
                if (Person is Client originalClient)
                {
                    client.FirstReference = originalClient.FirstReference;
                    client.SecondReference = originalClient.SecondReference;
                    client.WorkCenter = originalClient.WorkCenter;
                    client.DepositAccount = originalClient.DepositAccount;
                    client.PaymentAccount = originalClient.PaymentAccount;
                    client.Employee = originalClient.Employee;
                    client.ClientId = originalClient.ClientId;
                }

                _navigationService.NavigateTo<FinancialDataViewModel>(new ClientDataParams(_pageMode, client));
            }

        }

        private void Cancel(object obj)
        {
            if (_dialogService.Confirm("¿Desea cancelar los cambios realizados?"))
            {
                if (OldPerson != null)
                {
                    Person.PersonalData = OldPerson.PersonalData;
                    Person.AddressData = OldPerson.AddressData;
                }

                SwitchMode(PageMode.View);
            }
        }

        private void Save(object obj)
        {
            string message = string.Empty;

            try
            {
                if (_clientManagementService.ValidateAddressData(Person.AddressData) &&
                    _clientManagementService.ValidatePersonalData(Person.PersonalData))
                {
                    if (!_clientManagementService.IsPhoneNumberRepeated(Person.PersonalData.PhoneNumber, Person.PersonalData.AlternativePhoneNumber, out message) &&
                        !_clientManagementService.IsRFCRegistered(Person.PersonalData.RFC, Person.PersonalData.PersonalDataId, out message) &&
                        _clientManagementService.IsValidAge(Person.PersonalData.BirthDate.Value, out message))
                    {
                        var (updateMessage, updatedPersonalData) = _clientManagementService.UpdatePersonalData(Person.PersonalData);
                        var (updateAddressMessage, updatedAddressData) = _clientManagementService.UpdateAddressData(Person.AddressData);

                        message = updateMessage;

                        if (updatedPersonalData != null && updateAddressMessage != null)
                        {
                            Person.PersonalData = updatedPersonalData;
                            Person.AddressData = updatedAddressData;
                            SwitchMode(PageMode.View);
                        }
                    }
                }
            }
            catch (ArgumentException exception)
            {
                message = exception.Message;
            }

            if (message != string.Empty)
            {
                _dialogService.Dismiss(message, System.Windows.MessageBoxImage.Information);
            }            
        }

        private void Edit(object obj)
        {
            SwitchMode(PageMode.Update);
        }

        private bool CanNext(object obj)
        {
            return true;
        }

        private IPerson CreatePersonCopy(IPerson original)
        {
            var copy = new Client();

            copy.PersonalData = new PersonalData
            {
                Name = original.PersonalData.Name,
                LastName = original.PersonalData.LastName,
                Surname = original.PersonalData.Surname,
                BirthDate = original.PersonalData.BirthDate,
                RFC = original.PersonalData.RFC,
                CURP = original.PersonalData.CURP,
                PhoneNumber = original.PersonalData.PhoneNumber,
                AlternativePhoneNumber = original.PersonalData.AlternativePhoneNumber,
                Email = original.PersonalData.Email
            };

            copy.AddressData = new AddressData
            {
                Street = original.AddressData.Street,
                NeighborHood = original.AddressData.NeighborHood,
                City = original.AddressData.City,
                State = original.AddressData.State
            };

            return copy;
        }
    }
}
