using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum CreditApplicationStates
{
    Pending,
}

namespace Independiente.Model
{
    public class CreditApplication : INotifyPropertyChanged
    {
        private int _creditApplicationId;

        private decimal _loanAmount;

        private DateTime _loanApplicationDate;

        private CreditApplicationStates _status;

        private Client _client;

        private PromotionalOffer _promotionalOffer;

        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            return $"CrediApplicationID: {CreditApplicationId}, LoanAmount: {_loanAmount}, LoanDate: {LoanApplicationDate}, Client: {Client?.ToString()}, PromotionalOffer: {PromotionalOffer?.ToString()}";
        }

        public int CreditApplicationId
        {
            get => _creditApplicationId;
            set
            {
                if (_creditApplicationId != value)
                {
                    _creditApplicationId = value;
                    OnPropertyChanged(nameof(CreditApplicationId));
                }
            }
        }

        public DateTime LoanApplicationDate
        {
            get => _loanApplicationDate;
            set
            {
                if (_loanApplicationDate != value)
                {
                    _loanApplicationDate = value;
                    OnPropertyChanged(nameof(LoanApplicationDate));
                }
            }
        }

        public CreditApplicationStates Status
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

        public Client Client
        {
            get => _client;
            set
            {
                if (_client != value)
                {
                    _client = value;
                    OnPropertyChanged(nameof(Client));
                }
            }
        }

        public PromotionalOffer PromotionalOffer
        {
            get => _promotionalOffer;
            set
            {
                if (_promotionalOffer != value)
                {
                    _promotionalOffer = value;
                    OnPropertyChanged(nameof(PromotionalOffer));
                }
            }
        }

        public decimal LoanAmount
        {
            get => _loanAmount;
            set
            {
                if (_loanAmount != value)
                {
                    _loanAmount = value;
                    OnPropertyChanged(nameof(LoanAmount));
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

       

    }

}
