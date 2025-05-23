﻿using Independiente.Commands;
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
        public ICommand ShowSelectedCommand {  get; set; }
        public ICommand RegisterCommand { get; set; }

        public EmployeeAndClientConsultationViewModel() { }

        public EmployeeAndClientConsultationViewModel(IDialogService dialogService, INavigationService navigationService, RegistrationType registrationType, IClientManagementService clientManagementService)
        {
            ClientManagementService = clientManagementService;

            if (registrationType == RegistrationType.Employee)
            {
                //TODO aqui va la logica de consulta de empleados
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

            Console.WriteLine(PeopleList.ToString());
        }

        private void ShowSelected(object obj)
        {
            if (obj is Client client)
            {
                _navigationService.NavigateTo<PersonalDataViewModel>(new PersonDataParams(PageMode.View, RegistrationType.Client, client));
            }
        }

        private void Register (object obj)
        {
            _navigationService.NavigateTo<PersonalDataViewModel>(new PersonDataParams(PageMode.Registration, RegistrationType.Client, new Client()));
        }

        private bool CanDoIt (object obj)
        {
            return true;
        }

    }
}
