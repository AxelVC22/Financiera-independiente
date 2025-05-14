using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.Model
{

    public enum CreditPolicyStates
    {
        Active,
        Inactive,
    }

    public class CreditPolicy : INotifyPropertyChanged

    {
        private int _creditPolicyId;

        private string _name;

        private string _description;

        private DateTime _registrationDate;

        private DateTime _endDate;

        private CreditPolicyStates _status;

        private bool _isEditable;

        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isPassed;

        public int CredditPolicyId
        {
            get => _creditPolicyId;
            set
            {
                if (_creditPolicyId != value)
                {
                    _creditPolicyId = value;
                    OnPropertyChanged(nameof(CredditPolicyId));
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

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        public DateTime RegistrationDate
        {
            get => _registrationDate;
            set
            {
                if (_registrationDate != value)
                {
                    _registrationDate = value;
                    OnPropertyChanged(nameof(RegistrationDate));
                }
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }

        public CreditPolicyStates Status
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

        public bool IsEditable
        {
            get => _isEditable;
            set
            {
                if (_isEditable != value)
                {
                    _isEditable = value;
                    OnPropertyChanged(nameof(IsEditable));
                }
            }
        }

        public bool IsPassed
        {
            get => _isPassed;
            set
            {
                if (_isPassed != value)
                {
                    _isPassed = value;
                    OnPropertyChanged(nameof(IsPassed));
                }
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
