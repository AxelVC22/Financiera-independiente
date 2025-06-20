using Independiente.Commands;
using Independiente.DataAccess.Repositories;
using Independiente.Model;
using Independiente.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace Independiente.ViewModel
{
    public class EmployeeViewModel : ModificableViewModel
    {
        public Employee Employee { get; set; }
        public IPerson OldPerson { get; set; }
        public Model.Employee EmployeeToUpdate { get; set; }
        private RegistrationType _registrationType { get; set; }
        private PageMode _pageMode { get; set; }

        public List<string> StatesList { get; set; }

        private IDialogService _dialogService { get; set; }

        private INavigationService _navigationService { get; set; }

        private IClientManagementService _clientManagementService { get; set; }

        private IEmployeeService _employeeService;

        public bool _isRegistrationMode { get; set; }
        public string Password { get; set; }

        public IEnumerable<UserRole> Roles => Enum.GetValues(typeof(UserRole)).Cast<UserRole>();
        public UserRole SelectedRole { get; set; }

        public ICommand RegisterCommand { get; set; }

        public EmployeeViewModel()
        {
        }

        public EmployeeViewModel(IDialogService dialogService, INavigationService navigationService, PageMode mode, RegistrationType type, IPerson person, IClientManagementService clientManagementService, IEmployeeService employeeService)
        {
            RegisterCommand = new RelayCommand(Register, CanNext);
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
            _employeeService = employeeService;
            Employee = person as Employee;

            if (_pageMode == PageMode.View)
            {
                EmployeeToUpdate = new Model.Employee();
                OldPerson = CreatePersonCopy(Employee);
                EmployeeToUpdate.PersonalData = person.PersonalData;
                EmployeeToUpdate.AddressData = person.AddressData;
            }

            IsRegistrationMode = mode == PageMode.Registration;

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


        //private void Next(object obj)
        //{
        //    string message = string.Empty;
        //    bool validation = false;
        //    Model.Client client = new Model.Client();
        //    client.PersonalData = Employee.PersonalData;
        //    client.AddressData = Employee.AddressData;

        //    if (_pageMode == PageMode.Registration)
        //    {
        //        switch (_registrationType)
        //        {
        //            case RegistrationType.Client:
        //                try
        //                {
        //                    if (_clientManagementService.ValidateAddressData(Employee.AddressData) &&
        //                        _clientManagementService.ValidatePersonalData(Employee.PersonalData))
        //                    {
        //                        if (!_clientManagementService.IsPhoneNumberRepeated(Employee.PersonalData.PhoneNumber, Employee.PersonalData.AlternativePhoneNumber, out message) &&
        //                            !_clientManagementService.IsRFCRegistered(Employee.PersonalData.RFC, Employee.PersonalData.PersonalDataId, out message) &&
        //                            _clientManagementService.IsValidAge(Employee.PersonalData.BirthDate.Value, out message))
        //                        {
        //                            validation = true;
        //                        }
        //                    }
        //                }
        //                catch (ArgumentException exception)
        //                {
        //                    message = exception.Message;
        //                }
        //                break;
        //            case RegistrationType.Employee:

        //                break;
        //        }

        //        if (validation)
        //        {
        //            _navigationService.NavigateTo<FinancialDataViewModel>(new ClientDataParams(_pageMode, client));
        //        }
        //        else
        //        {
        //            _dialogService.Dismiss(message, System.Windows.MessageBoxImage.Information);
        //        }
        //    }
        //    else if (_pageMode == PageMode.View)
        //    {

        //        _navigationService.NavigateTo<FinancialDataViewModel>(new ClientDataParams(_pageMode, client));
        //    }

        //}

        private void Cancel(object obj)
        {
            if (_dialogService.Confirm("¿Desea cancelar los cambios realizados?"))
            {
                if (OldPerson != null)
                {
                    Employee.PersonalData = OldPerson.PersonalData;
                    Employee.AddressData = OldPerson.AddressData;
                    OldPerson = CreatePersonCopy(Employee);

                }

                SwitchMode(PageMode.View);
            }
        }

        private void Register(object obj)
        {
            string message = string.Empty;
            int result = 0;
            try
            {
                if (_clientManagementService.ValidateAddressData(Employee.AddressData) &&
                    _clientManagementService.ValidatePersonalData(Employee.PersonalData))
                {
                    if (!_clientManagementService.IsPhoneNumberRepeated(Employee.PersonalData.PhoneNumber, Employee.PersonalData.AlternativePhoneNumber, out message) &&
                        !_clientManagementService.IsRFCRegistered(Employee.PersonalData.RFC, Employee.PersonalData.PersonalDataId, out message) &&
                        _clientManagementService.IsValidAge(Employee.PersonalData.BirthDate.Value, out message))
                    {



                        result = _employeeService.AddEmployee(Employee);




                        if (result > 0)
                        {
                            _dialogService.Dismiss("Los datos han sido registrados correctamente", System.Windows.MessageBoxImage.Information);
                            SwitchMode(PageMode.View);
                        }
                    }
                }
            }
            catch (ArgumentException exception)
            {
                _dialogService.Dismiss(exception.Message, System.Windows.MessageBoxImage.Information);
            }

            if (message != string.Empty)
            {
                _dialogService.Dismiss(message, System.Windows.MessageBoxImage.Information);
            }

        }

        private void Save(object obj)
        {
            string message = string.Empty;
            int result = 0;
            try
            {
                if (_clientManagementService.ValidateAddressData(Employee.AddressData) &&
                    _clientManagementService.ValidatePersonalData(Employee.PersonalData))
                {
                    if (!_clientManagementService.IsPhoneNumberRepeated(Employee.PersonalData.PhoneNumber, Employee.PersonalData.AlternativePhoneNumber, out message) &&
                       
                        _clientManagementService.IsValidAge(Employee.PersonalData.BirthDate.Value, out message))
                    {


                        result = _employeeService.UpdateEmployee(Employee);





                        if (result > 0)
                        {
                            _dialogService.Dismiss("Los datos han sido registrados correctamente", System.Windows.MessageBoxImage.Information);
                            SwitchMode(PageMode.View);
                        }
                    }
                }
            }
            catch (ArgumentException exception)
            {
                _dialogService.Dismiss(exception.Message, System.Windows.MessageBoxImage.Information);
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

        private Employee CreatePersonCopy(Employee original)
        {
            var copy = new Employee();

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

            copy.NSS = original.NSS;

            return copy;
        }


        public bool IsRegistrationMode
        {
            get => _isRegistrationMode;
            set
            {
                if (_isRegistrationMode != value)
                {
                    _isRegistrationMode = value;
                    OnPropertyChanged(nameof(IsRegistrationMode));
                }
            }
        }
    }
}
