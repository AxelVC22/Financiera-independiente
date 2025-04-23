using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.Model
{
    public class Bank : INotifyPropertyChanged, ICatalog
    {
        private bool _isEditable;
        private int _bankId { get; set; }

        private string _name;

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

        public int BankId
        {
            get => _bankId;
            set
            {
                if (_bankId != value)
                {
                    _bankId = value;
                    OnPropertyChanged(nameof(BankId));
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

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
