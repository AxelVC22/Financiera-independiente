using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.Model
{
    public class Account : INotifyPropertyChanged
    {
        private int _accountId;
        private string _clabe;
        private int _bank;

        public int AccountId
        {
            get => _accountId;
            set
            {
                if (_accountId != value)
                {
                    _accountId = value;
                    OnPropertyChanged(nameof(AccountId));
                }
            }
        }

        public string CLABE
        {
            get => _clabe;
            set
            {
                if (_clabe != value)
                {
                    _clabe = value;
                    OnPropertyChanged(nameof(CLABE));
                }
            }
        }

        public int Bank
        {
            get => _bank;
            set
            {
                if (_bank != value)
                {
                    _bank = value;
                    OnPropertyChanged(nameof(Bank));
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
            return $"AccountId: {AccountId}, CLABE: {CLABE}";
        }
    }
}
