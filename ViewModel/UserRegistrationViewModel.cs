using Independiente.Commands;
using Independiente.Model;
using Independiente.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Independiente.ViewModel
{
    public class UserRegistrationViewModel : ModificableViewModel
    {
        private  IDialogService _dialogService;

        private INavigationService _navigationService;

        private PageMode _pageMode;

        public Employee Employee { get; set; }

        public ICommand ShowPasswordCommand { get; set; }
        public ICommand HidePasswordCommand { get; set; }

        public ICommand ShowConfirmationPasswordCommand { get; set; }
        public ICommand HideConfirmationPasswordCommand { get; set; }

        private string _password;

        private bool _isPasswordVisible;

        private string _confirmationPassword;

        private bool _isConfirmationPasswordVisible;

        public List<string> RolesList { get; set; }


        public UserRegistrationViewModel()
        {

        }

        public UserRegistrationViewModel(IDialogService dialogService, INavigationService navigationService, PageMode mode, IPerson person)
        {
            NextCommand = new RelayCommand(Next, CanDoIt);
            EditCommand = new RelayCommand(Edit, CanDoIt);
            CancelCommand = new RelayCommand(Cancel, CanDoIt);
            SaveCommand = new RelayCommand(Save, CanDoIt);
            GoBackCommand = new RelayCommand(GoBack, CanDoIt);
            ShowPasswordCommand = new RelayCommand(ShowPassword, CanDoIt);
            HidePasswordCommand = new RelayCommand(HidePassword, CanDoIt);
            ShowConfirmationPasswordCommand = new RelayCommand(ShowConfirmationPassword, CanDoIt);
            HideConfirmationPasswordCommand = new RelayCommand(HideConfirmationPassword, CanDoIt);
            _dialogService = dialogService;
            _navigationService = navigationService;
            _pageMode = mode;
            Employee = (Employee)person;
            SwitchMode(mode);

        }

        public bool CanDoIt(object obj)
        {
            return true;
        }

        public bool IsPasswordVisible
        {
            get => _isPasswordVisible;
            set
            {
                _isPasswordVisible = value;
                OnPropertyChanged(nameof(IsPasswordVisible));
            }
        }

        public bool IsConfirmationPasswordVisible
        {
            get => _isConfirmationPasswordVisible;
            set
            {
                _isConfirmationPasswordVisible = value;
                OnPropertyChanged(nameof(IsConfirmationPasswordVisible));
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        public string ConfirmationPassword
        {
            get { return _confirmationPassword; }
            set
            {
                if (_confirmationPassword != value)
                {
                    _confirmationPassword = value;
                    OnPropertyChanged(nameof(ConfirmationPassword));
                }
            }
        }

        private void ShowPassword(object obj)
        {
            IsPasswordVisible = true;
        }

        private void HidePassword(object obj)
        {
            IsPasswordVisible = false;
        }

        private void ShowConfirmationPassword(object obj)
        {
            IsConfirmationPasswordVisible = true;
        }

        private void HideConfirmationPassword(object obj)
        {
            IsConfirmationPasswordVisible = false;
        }

        private void GoBack(object obj)
        {
            _navigationService.GoBack();
        }
        private void Next(object obj)
        {
            

        }

        private void Cancel(object obj)
        {
            SwitchMode(PageMode.View);
        }

        private void Save(object obj)
        {
           
            Console.WriteLine(Employee.ToString());
            Console.WriteLine(Password);
            Console.WriteLine(ConfirmationPassword);
            SwitchMode(PageMode.View);
        }

        private void Edit(object obj)
        {

            SwitchMode(PageMode.Update);
        }

    }
}
