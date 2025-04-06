using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.Model
{
    public class Reference : INotifyPropertyChanged
    {
        private string _name;

        private string _fullLastName;

        private string _relationShip;

        private string _phoneNumber;

        private string _email;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string FullLastName
        {
            get => _fullLastName;
            set
            {
                if (_fullLastName != value)
                {
                    _fullLastName = value;
                    OnPropertyChanged(nameof(FullLastName));
                }
            }
        }
        public string Relationship
        {
            get => _relationShip;
            set
            {
                if (_relationShip != value)
                {
                    _relationShip = value;
                    OnPropertyChanged(nameof(Relationship));
                }
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"Name: {Name}, FullLastName: {FullLastName}, RelationShip: {Relationship}, PhoneNumber: {PhoneNumber}, Email {Email}";
        }
    }
}
