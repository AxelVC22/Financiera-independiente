using Independiente.Commands;
using Independiente.Model;
using Independiente.Properties;
using Independiente.Services;
using Independiente.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Independiente.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public User Session;
        public ICommand LogoutCommand { get; set; }

        public ICommand ShowAndHideMenuCommand { get; set; }

        private readonly IDialogService _dialogService;

        public User User { get; }

        public event EventHandler RequestClose;
        public double MenuWidth => IsMenuVisible ? 240 : 60;
        private bool _isMenuVisible { get; set; } 

        public MainWindowViewModel()
        {

        }

        public MainWindowViewModel(IDialogService dialogService)
        {
            LogoutCommand = new RelayCommand(Logout, CanLogout);
            ShowAndHideMenuCommand = new RelayCommand(ShowAndHideMenu, CanLogout);
            IsMenuVisible = false;
            _dialogService = dialogService;
            User = App.SessionService.CurrentUser;
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
                RequestClose?.Invoke(this, EventArgs.Empty);
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

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
