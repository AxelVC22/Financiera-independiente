using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.Model
{
    public class WorkCenter :INotifyPropertyChanged
    {
        private string _name;

        private string _role;

        private DateTime? _hiringDate;

        private string _street;




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

        public DateTime? HiringDate
        {
            get => _hiringDate;
            set
            {
                if (_hiringDate != value)
                {
                    _hiringDate = value;
                    OnPropertyChanged(nameof(HiringDate));
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"Name: {Name}, Role: {Role}, HiringDate: {HiringDate}, Street: {Street}";
        }
    }
}
