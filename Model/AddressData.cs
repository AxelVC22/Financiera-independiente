using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.Model
{
    public class AddressData : INotifyPropertyChanged
    {
        private int _addressDateId;

        private string _street;

        private string _state;

        private string _neighboorhood;

        private string _city;


        public int AddressDataId
        {
            get => _addressDateId;
            set
            {
                if (_addressDateId != value)
                {
                    _addressDateId = value;
                    OnPropertyChanged(nameof(AddressDataId));
                }
            }
        }

        public string Street
        {
            get => _street;
            set
            {
                if (_street != value)
                {
                    _street = value;
                    OnPropertyChanged(nameof(Street));
                }
            }
        }

        public string State
        {
            get => _state;
            set
            {
                if (_state != value)
                {
                    _state = value;
                    OnPropertyChanged(nameof(State));
                }
            }
        }

        public string NeighborHood
        {
            get => _neighboorhood;
            set
            {
                if (_neighboorhood != value)
                {
                    _neighboorhood = value;
                    OnPropertyChanged(nameof(NeighborHood));
                }
            }
        }

        public string City
        {
            get => _city;
            set
            {
                if (_city != value)
                {
                    _city = value;
                    OnPropertyChanged(nameof(City));
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
            return $"Id: {AddressDataId}, Street: {Street}, State: {State}, Neighborhood: {NeighborHood}";
        }
    }
}
