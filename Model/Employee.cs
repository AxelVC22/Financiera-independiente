using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Independiente.Model
{
    public class Employee : INotifyPropertyChanged, IPerson
    {

        private PersonalData _personalData;
        private AddressData _addressData;

        private string _nss;

        private string _role;
        public PersonalData PersonalData
        {
            get => _personalData;
            set
            {
                if (_personalData != value)
                {
                    _personalData = value;
                    OnPropertyChanged(nameof(PersonalData));
                }
            }
        }
        public AddressData AddressData
            {
            get => _addressData;
            set
            {
                if (_addressData != value)
                {
                    _addressData = value;
                    OnPropertyChanged(nameof(AddressData));
                }
            }
        }

        public string NSS
        {
            get => _nss;
            set
            {
                if (_nss != value)
                {
                    _nss = value;
                    OnPropertyChanged(nameof(NSS));
                }
            }
        }

        public string Role
        {
            get => _role;
            set
            {
                if (_role != value)
                {
                    _role = value;
                    OnPropertyChanged(nameof(Role));
                }
            }
        }





        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"NSS: {NSS}";
        }

    }
}
