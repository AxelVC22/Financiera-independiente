using Independiente.Commands;
using Independiente.DataAccess;
using Independiente.Properties;
using Independiente.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Independiente.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly IDialogService _dialogService;

        public ICommand LoginCommand { get; set; }
        public ICommand ShowPasswordCommand {  get; set; }
        public ICommand HidePasswordCommand {  get; set; }

        public bool IsLoginSuccessful { get; private set; }

        public string EmailAddress { get; set; }

        private string _password;

        private bool _isPasswordVisible;
        public event EventHandler RequestClose;


        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login, CanLogin);
            ShowPasswordCommand = new RelayCommand(ShowPassword, CanLogin);
            HidePasswordCommand = new RelayCommand(HidePassword, CanLogin);
            IsLoginSuccessful = false;
        }

        private void Login(object obj)
        {
            //var (success, message) = App.SessionService.AuthEmployee(EmailAddress, Password);

            //if (success)
            //{
                IsLoginSuccessful = true;
                RequestClose?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                IDialogService dialogService = new DialogService();
                dialogService.Dismiss(message, MessageBoxImage.Information);
                IsLoginSuccessful = false;
            }
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

        private void ShowPassword(object obj)
        {
            IsPasswordVisible = true;
        }

        private void HidePassword(object obj)
        {
            IsPasswordVisible = false;
        }
       

        private bool CanLogin(object obj)
        {
            return true;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
