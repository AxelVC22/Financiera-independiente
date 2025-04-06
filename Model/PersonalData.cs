using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.Model
{
    public class PersonalData : INotifyPropertyChanged
    {

        private int _personalDataId;

        private string _name;

        private string _lastName;

        private string _surname;

        private DateTime? _birthDate;

        private string _rfc;

        private string _curp;

        private string _phoneNumber;

        private string _alternativePhoneNumber;

        private string _email;

        public int PersonalDataId
        {
            get => _personalDataId;
            set
            {
                if (_personalDataId != value)
                {
                    _personalDataId = value;
                    OnPropertyChanged(nameof(PersonalDataId));
                }
            }
        }

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

        public string LastName
        {
            get => _lastName;
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    OnPropertyChanged(nameof(LastName));
                }
            }
        }


        public string Surname
        {
            get => _surname;
            set
            {
                if (_surname != value)
                {
                    _surname = value;
                    OnPropertyChanged(nameof(Surname));
                }
            }
        }

        public DateTime? BirthDate
        {
            get => _birthDate;
            set
            {
                if (_birthDate != value)
                {
                    _birthDate=value?.Date;
                    OnPropertyChanged(nameof(BirthDate));
                }
            }
        }

        public string RFC
        {
            get => _rfc;
            set
            {
                if (_rfc != value)
                {
                    _rfc = value;
                    OnPropertyChanged(nameof(BirthDate));
                }
            }
        }

        public string CURP
        {
            get => _curp;
            set
            {
                if (_curp != value)
                {
                    _curp = value;
                    OnPropertyChanged(nameof(CURP));
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

        public string AlternativePhoneNumber
        {
            get => _alternativePhoneNumber;
            set
            {
                if (_alternativePhoneNumber != value)
                {
                    _alternativePhoneNumber = value;
                    OnPropertyChanged(nameof(AlternativePhoneNumber));
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"Name: {Name}, LastName: {LastName}, Surname: {Surname}, BirhDate: {BirthDate}";
        }
    }
}
