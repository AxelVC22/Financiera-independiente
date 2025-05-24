using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Independiente.Model
{
    public enum EmployeeStates
    {
        Active,
        Inactive
    }

    public class Employee : INotifyPropertyChanged, IPerson
    {
        private int _employeeId;

        private string _nss;        

        private DateTime _hireDate;

        private string _department;

        private EmployeeStates _status;

        private PersonalData _personalData;
        private AddressData _addressData;

        private AddressData _addressData;

        private User _user;

        public int EmployeeId
        {
            get => _employeeId;
            set
            {
                if (_employeeId != value)
                {
                    _employeeId = value;
                    OnPropertyChanged(nameof(EmployeeId));
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

        public DateTime HireDate
        {
            get => _hireDate;
            set
            {
                if (_hireDate != value)
                {
                    _hireDate = value;
                    OnPropertyChanged(nameof(HireDate));
                }
            }
        }

        public string Department
        {
            get => _department;
            set
            {
                if (_department != value)
                {
                    _department = value;
                    OnPropertyChanged(nameof(Department));
                }
            }
        }

        public EmployeeStates Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

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

        public User User
        {
            get => _user;
            set
            {
                if (_user != value)
                {
                    _user = value;
                    OnPropertyChanged(nameof(User));
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
