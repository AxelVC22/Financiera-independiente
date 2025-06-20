using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.Model
{
    public enum PromotionalOfferStates
    {
        Active,
        Inactive
    }

    public enum PromotionalOfferPaymentFrequencies
    {
        Quincenal,
        Mensual
    }

    public class PromotionalOffer :INotifyPropertyChanged
    {
        private int _promotionalOfferId;

        private decimal? _interesRate;

        private int? _loanTerm;

        private PromotionalOfferPaymentFrequencies _paymenteFrecuency;

        private decimal? _iva;

        private PromotionalOfferStates _status;

        private string _name;

        private bool _isEditable;
        public override string ToString()
        {
            return $" PromotionalOfferId: {PromotionalOfferId}, InteresRate: {InteresRate}, LoanTerm: {LoanTerm}, PaymentFrecuency: {PaymenteFrecuency}, IVA: {IVA}";
        }

        public int PromotionalOfferId
        {
            get => _promotionalOfferId;
            set
            {
                if (_promotionalOfferId != value)
                {
                    _promotionalOfferId = value;
                    OnPropertyChanged(nameof(PromotionalOfferId));
                }
            }
        }

        public decimal? InteresRate
        {
            get => _interesRate;
            set
            {
                if (_interesRate != value)
                {
                    _interesRate = value;
                    OnPropertyChanged(nameof(InteresRate));
                }
            }
        }

        public int? LoanTerm
        {
            get => _loanTerm;
            set
            {
                if (_loanTerm != value)
                {
                    _loanTerm = value;
                    OnPropertyChanged(nameof(LoanTerm));
                }
            }
        }

        public PromotionalOfferPaymentFrequencies PaymenteFrecuency
        {
            get => _paymenteFrecuency;
            set
            {
                if (_paymenteFrecuency != value)
                {
                    _paymenteFrecuency = value;
                    OnPropertyChanged(nameof(PaymenteFrecuency));
                }
            }
        }

        public decimal? IVA
        {
            get => _iva;
            set
            {
                if (_iva != value)
                {
                    _iva = value;
                    OnPropertyChanged(nameof(IVA));
                }
            }
        }

        public PromotionalOfferStates Status
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

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(_name));
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


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
