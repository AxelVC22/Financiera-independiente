using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Independiente.Model
{

    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed
    }

    public class Payment : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Payment()
        {
            IsUploadEnabled = false;
        }
    
        public int PaymentId { get; set; }

        public Employee Employee { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal ActualAmount { get; set; }

        public DateTime RegistrationDate { get; set; }

        public Bank Bank { get; set; }

        public int TotalCredits { get; set; }

        public int ActualCredits { get; set; }

        private PaymentStatus _status;

        public DateTime? UploadDate { get; set; }


        private bool _isUploadEnabled = false;

        public bool IsUploadEnabled
        {
            get => _isUploadEnabled;
            set
            {
                if (_isUploadEnabled != value)
                {
                    _isUploadEnabled = value;
                    OnPropertyChanged(nameof(IsUploadEnabled));
                }
            }
        }

        public PaymentStatus Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    IsUploadEnabled = _status == PaymentStatus.Pending;

                    OnPropertyChanged(nameof(Status));
                }
                
            }
        }

    }
}
