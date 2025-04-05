using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Independiente.ViewModel
{
    public enum RegistrationType
    {
        Client,
        Employee
    }

    public enum PageMode
    {
        Registration,
        View,
        Update
    }

    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand GoBackCommand { get; set; }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
    public class ModificableViewModel : BaseViewModel
    {
        public ICommand NextCommand { get; set; }
        public ICommand EditCommand { get; set; }

        public ICommand CancelCommand { get; set; }
        public ICommand SaveCommand { get; set; }




        private bool _isReadOnlyMode { get; set; }

        private bool _isViewMode { get; set; }

        private bool _isUpdateMode { set; get; }

        public PageMode Mode { get; set; }


        public bool IsReadOnlyMode
        {
            get => _isReadOnlyMode;
            set
            {
                _isReadOnlyMode = value;
                OnPropertyChanged(nameof(IsReadOnlyMode));
            }
        }

        public bool IsViewMode
        {
            get => _isViewMode;
            set
            {
                _isViewMode = value;
                OnPropertyChanged(nameof(IsViewMode));
            }
        }

        public bool IsUpdateMode
        {
            get => _isUpdateMode;
            set
            {
                _isUpdateMode = value;
                OnPropertyChanged(nameof(IsUpdateMode));
            }
        }

        public void SwitchMode(PageMode mode)
        {
            switch (mode)
            {
                case PageMode.Registration:
                    IsReadOnlyMode = true;
                    IsUpdateMode = false;
                    IsViewMode = false;
                    break;
                case PageMode.Update:
                    IsReadOnlyMode = true;
                    IsUpdateMode = true;
                    IsViewMode = false;
                    break;
                case PageMode.View:
                    IsReadOnlyMode = true;
                    IsUpdateMode = false;
                    IsViewMode = true;
                    break;
            }
        }



    }
}
