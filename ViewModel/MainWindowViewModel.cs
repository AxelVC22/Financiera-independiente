using Independiente.Commands;
using Independiente.Model;
using Independiente.Properties;
using Independiente.Services;
using Independiente.View;
using Independiente.View.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Independiente.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<MenuOption> Options { get; } = new ObservableCollection<MenuOption>();
        public User Session;
        public ICommand LogoutCommand { get; set; }

        public ICommand ShowAndHideMenuCommand { get; set; }

        private readonly IDialogService _dialogService;

        private readonly INavigationService _navigationService;

        public User User { get; }

        public event EventHandler RequestClose;
        public double MenuWidth => IsMenuVisible ? 240 : 60;
        private bool _isMenuVisible { get; set; }

        public MainWindowViewModel()
        {

        }

        public MainWindowViewModel(IDialogService dialogService, INavigationService navigationService)
        {
            LogoutCommand = new RelayCommand(Logout, CanLogout);
            ShowAndHideMenuCommand = new RelayCommand(ShowAndHideMenu, CanLogout);
            IsMenuVisible = false;
            _dialogService = dialogService;
            User = App.SessionService.CurrentUser;
            _navigationService = navigationService;
            ChargeOptionsByRole();
        }

        public void ShowAndHideMenu(object obj)
        {
            IsMenuVisible = !IsMenuVisible;
        }


        public bool IsMenuVisible
        {
            get => _isMenuVisible;
            set
            {
                if (_isMenuVisible != value)
                {
                    _isMenuVisible = value;

                    OnPropertyChanged(nameof(MenuWidth));
                    OnPropertyChanged(nameof(IsMenuVisible));
                }
            }
        }

        private void Logout(object obj)
        {
            if (_dialogService.Confirm(Messages.LogOutMessage))
            {
                App.SessionService.LogOut();
                App.Current.Shutdown();
            }

        }

        private bool CanLogout(object obj)
        {
            return true;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ChargeOptionsByRole()
        {
            switch (User.UserRole)
            {
                case UserRole.Administrator:
                    ChargeAllOptions();
                    break;

                case UserRole.Advisor:
                    AddOption("Clientes", _ => _navigationService.NavigateTo<EmployeeAndClientConsultationViewModel>());
                    AddOption("Creditos", _ => _navigationService.NavigateTo<CreditApplicationsViewModel>());


                    break;

                case UserRole.Analyst:
                    AddOption("Clientes", _ => _navigationService.NavigateTo<EmployeeAndClientConsultationViewModel>());
                    AddOption("Creditos", _ => _navigationService.NavigateTo<CreditApplicationsViewModel>());


                    break;

                case UserRole.Collector:
                    AddOption("Pagos", _ => _navigationService.NavigateTo<PaymentsViewModel>());
                    AddOption("Creditos", _ => _navigationService.NavigateTo<CreditApplicationsViewModel>());


                    break;


            }
        }

        private void AddOption(string nombre, Action<object> comando)
        {
            Options.Add(new MenuOption { Name = nombre, Command = new RelayCommand(comando, _ => true) });
        }


        private void ChargeAllOptions()
        {
            AddOption("Politicas", _ => _navigationService.NavigateTo<CreditPoliciesManagementViewModel>());
            AddOption("Empleados", _ => _navigationService.NavigateTo<EmployeeAndClientConsultationViewModel>(new ConsultationParams(RegistrationType.Employee)));
            AddOption("Ofertas", _ => _navigationService.NavigateTo<PromotionalOffersManagementViewModel>());


        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
