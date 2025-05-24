using Independiente.DataAccess;
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
        public int ClientId { get; set; }
        private PersonalData _personalData;
        private AddressData _addressData;
        private Reference _firstReference;
        private Reference _secondReference;
        private int EmployeeId { get; set; }
        private Account _depositAccount;
        private Account _paymentAccount;
        private WorkCenter _workCenter;

        public Client()
        {
            _personalData = new PersonalData();
            _addressData = new AddressData();
            _firstReference = new Reference();
            _secondReference = new Reference();
            _depositAccount = new Account();
            _paymentAccount = new Account();
            _workCenter = new WorkCenter();
        }

        public int Employee
        {
            get => EmployeeId;
            set
            {
                if (EmployeeId != value)
                {
                    EmployeeId = value;
                    OnPropertyChanged(nameof(EmployeeId));
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

        public Reference FirstReference
        {
            get => _firstReference;
            set
            {
                if (_firstReference != value)
                {
                    _firstReference = value;
                    OnPropertyChanged(nameof(FirstReference));
                }
            }
        }

        public Reference SecondReference
        {
            get => _secondReference;
            set
            {
                if (_secondReference != value)
                {
                    _secondReference = value;
                    OnPropertyChanged(nameof(SecondReference));
                }
            }
        }

        public Account DepositAccount
        {
            get => _depositAccount;
            set
            {
                if (_depositAccount != value)
                {
                    _depositAccount = value;
                    OnPropertyChanged(nameof(DepositAccount));
                }
            }
        }

        public Account PaymentAccount
        {
            get => _paymentAccount;
            set
            {
                if (_paymentAccount != value)
                {
                    _paymentAccount = value;
                    OnPropertyChanged(nameof(PaymentAccount));
                }
            }
        }

        public WorkCenter WorkCenter
        {
            get => _workCenter;
            set
            {
                if (_workCenter != value)
                {
                    _workCenter = value;
                    OnPropertyChanged(nameof(WorkCenter));
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
            return $"Personal data: {PersonalData.ToString()} Address data: {AddressData.ToString()}";
        }

        public static Client FromClientView(ClientView view)
        {
            if (view == null) return null;

            return new Client
            {
                PersonalData = new PersonalData
                {
                    PersonalDataId = view.PersonalDataId,
                    Name = view.ClientName,
                    LastName = view.ClientLastname,
                    Surname = view.ClientSurname,
                    BirthDate = view.BirthDate,
                    RFC = view.RFC,
                    CURP = view.CURP,
                    PhoneNumber = view.ClientPhoneNumber,
                    Email = view.ClientEmail
                },

                AddressData = new AddressData
                {
                    AddressDataId = view.AddressDataId,
                    Street = view.Street,
                    State = view.State,
                    NeighborHood = view.Neighborhood,
                    City = view.City
                },

                FirstReference = new Reference
                {
                    ReferenceId = (int)view.FirstReference,
                    Name = view.FirstReferenceName,
                    FullLastName = view.FirstReferenceLastname,
                    PhoneNumber = view.FirstReferencePhoneNumber,
                    Relationship = view.FirstReferenceRelationship,
                    Email = view.FirstReferenceEmail
                },

                SecondReference = new Reference
                {
                    ReferenceId = (int)view.SecondReference,
                    Name = view.SecondReferenceName,
                    FullLastName = view.FirstReferenceLastname,
                    PhoneNumber = view.SecondReferencePhoneNumber,
                    Relationship = view.SecondReferenceRelationship,
                    Email = view.SecondReferenceEmail
                },

                WorkCenter = new WorkCenter
                {
                    WorkCenterId = view.WorkCenterId,
                    Name = view.WorkCenterName,
                    Role = view.Role,
                    HiringDate = view.HiringDate,
                    MontlyIncome = view.MontlyIncome
                },

                DepositAccount = new Account
                {
                    AccountId = view.DepositAccountId,
                    CLABE = view.DepositCLABE,
                    Bank = view.DepositBank
                },

                PaymentAccount = new Account
                {
                    AccountId = view.PaymentAccountId,
                    CLABE = view.PaymentCLABE,
                    Bank = view.PaymentBank
                },

                EmployeeId = view.EmployeeId,
                ClientId = view.ClientId
            };
        }
    }
}