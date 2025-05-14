using Independiente.View.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.Model
{
    public class Client : INotifyPropertyChanged, IPerson
    {
        public int ClientId {  get; set; }

        private PersonalData _personalData;

        private AddressData _addressData;
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
                    OnPropertyChanged(nameof(PersonalData));
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
            return $"Personal data: {PersonalData.ToString()}";
        }
    }
}
