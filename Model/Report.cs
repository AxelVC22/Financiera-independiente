using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.Model
{
    public class Report : INotifyPropertyChanged
    {
        public int MaxCharacters { get; set; } = 512;

        private int _currentCharacters;
        public int ReportId {  get; set; }

        private string _notes;

        public event PropertyChangedEventHandler PropertyChanged;

        private DateTime _reviewingDate;

        private CreditApplication _creditApplication;

        public List<CreditPolicy> CreditPolicies;

        public Report()
        {
            CreditPolicies = new List<CreditPolicy>();
        }

        public string Notes
        {
            get => _notes;
            set
            {
                if (_notes != value)
                {
                    _notes = value;
                    OnPropertyChanged(nameof(Notes));
                }
                CurrentCharacters = Notes.Length;

            }
        }

        public DateTime ReviewingDate
        {
            get => _reviewingDate;
            set
            {
                if (_reviewingDate != value)
                {
                    _reviewingDate = value;
                    OnPropertyChanged(nameof(ReviewingDate));
                }
            }
        }

        public CreditApplication CreditApplication
        {
            get => _creditApplication;
            set
            {
                if (value != _creditApplication)
                {
                    _creditApplication = value;
                    OnPropertyChanged(nameof(CreditApplication));
                }
            }
        }

        public int CurrentCharacters
        {
            get => _currentCharacters;
            set
            {
                if (value != _currentCharacters)
                {
                    _currentCharacters = value;
                    OnPropertyChanged(nameof(CurrentCharacters));
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

   
}
