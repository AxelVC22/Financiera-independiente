using Independiente.Commands;
using Independiente.DataAccess;
using Independiente.DataAccess.Repositories;
using Independiente.Model;
using Independiente.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Independiente.ViewModel
{
    public class EmployeeAndClientConsultationViewModel : BaseViewModel
    {
        public ObservableCollection<IPerson> PeopleList { get; set; }
        public IPerson SelectedPerson {  get; set; }
        private IDialogService _dialogService { get; set; }
        private INavigationService _navigationService { get; set; }
        public IClientManagementService ClientManagementService { get; set; }

        public IEmployeeService EmployeeService { get; set; }
        public ICommand ShowSelectedCommand {  get; set; }
        public ICommand RegisterCommand { get; set; }

        public EmployeeQuery EmployeeQuery { get; set; }

        public RegistrationType RegistrationType { get; set; }

        public EmployeeAndClientConsultationViewModel() { }

        public EmployeeAndClientConsultationViewModel(IDialogService dialogService, INavigationService navigationService, RegistrationType registrationType, IClientManagementService clientManagementService, IEmployeeService employeeService)
        {
            ClientManagementService = clientManagementService;

            EmployeeService = employeeService;

            EmployeeQuery = new EmployeeQuery { PageNumber = 1 , PageSize = 50};

            RegistrationType = registrationType;
            if (registrationType == RegistrationType.Employee)
            {
                List<Model.Employee> employees = EmployeeService.GetEmployees(EmployeeQuery);
                PeopleList = new ObservableCollection<IPerson>(employees);
            } 
            else
            {
                List<Model.Client> clients = ClientManagementService.GetAllClientsByEmployeeId(App.SessionService.CurrentUser.EmployeeId);
                PeopleList = new ObservableCollection<IPerson>(clients);
            }

            ShowSelectedCommand = new RelayCommand (ShowSelected, CanDoIt);

            RegisterCommand = new RelayCommand(Register, CanDoIt);
            _navigationService = navigationService;
            _dialogService = dialogService;
        }

        private void ShowSelected(object obj)
        {
            if (obj is Model.Client client)
            {
                _navigationService.NavigateTo<PersonalDataViewModel>(new PersonDataParams(PageMode.View, RegistrationType.Client, client));
            }
            if (obj is Model.Employee employee)
            {
                _navigationService.NavigateTo<EmployeeViewModel>(new PersonDataParams(PageMode.View, RegistrationType.Client, employee));
            }
        }

        private void Register (object obj)
        {
            if (RegistrationType == RegistrationType.Client)
            {
                _navigationService.NavigateTo<PersonalDataViewModel>(new PersonDataParams(PageMode.Registration, RegistrationType.Client, new Model.Client()));

            }
            if (RegistrationType == RegistrationType.Employee)
            {
                _navigationService.NavigateTo<EmployeeViewModel>(new PersonDataParams(PageMode.Registration, RegistrationType.Employee, new Model.Employee()));

            }
        }

        private bool CanDoIt (object obj)
        {
            return true;
        }

    }
}
