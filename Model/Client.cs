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

        private PersonalData _personalData;

        public Client() 
        {
            _personalData = new PersonalData();
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

       

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
