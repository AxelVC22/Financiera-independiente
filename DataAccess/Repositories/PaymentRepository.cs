using Independiente.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.DataAccess.Repositories
{
    public class PaymentQuery : INotifyPropertyChanged, IQueryObject<PaymentView>
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        private string _bankName;

        private string _status;


        private DateTime? _fromDate;

        private DateTime? _toDate;

        public Expression<Func<PaymentView, bool>> BuildExpression()
        {
            return c => (string.IsNullOrEmpty(Status) || (c.Status == Status)) &&
                ((string.IsNullOrEmpty(BankName)) || c.BankName == BankName) &&
                (!FromDate.HasValue || c.RegistrationDate >= FromDate) &&
                (!ToDate.HasValue || c.RegistrationDate <= ToDate);

        }

        public string BankName
        {
            get => _bankName;
            set
            {
                if (_bankName != value)
                {
                    _bankName = value;
                    OnPropertyChanged(nameof(_bankName));
                }
            }
        }


        public DateTime? FromDate
        {
            get => _fromDate;
            set
            {
                if (_fromDate != value)
                {
                    _fromDate = value;
                    OnPropertyChanged(nameof(FromDate));
                }
            }
        }

        public DateTime? ToDate
        {
            get => _toDate;
            set
            {
                if (_toDate != value)
                {
                    _toDate = value;
                    OnPropertyChanged(nameof(ToDate));
                }
            }
        }

        public string Status
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ChargeQuery : INotifyPropertyChanged, IQueryObject<ChargeView>
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        private string _bankName;

        private string _status = PaymentStatus.Pending.ToString();


        private DateTime? _fromDate;

        private DateTime? _toDate;

        public Expression<Func<ChargeView, bool>> BuildExpression()
        {
            //return c => 
            //    ( c.BankName == BankName) &&
            //    ( c.PaymentDate >= FromDate) &&
            //    ( c.PaymentDate <= ToDate) &&
            //    (c.Status == PaymentStatus.Pending.ToString());
            return c => c.CLABE == c.CLABE;
        }

        public string BankName
        {
            get => _bankName;
            set
            {
                if (_bankName != value)
                {
                    _bankName = value;
                    OnPropertyChanged(nameof(_bankName));
                }
            }
        }


        public DateTime? FromDate
        {
            get => _fromDate;
            set
            {
                if (_fromDate != value)
                {
                    _fromDate = value;
                    OnPropertyChanged(nameof(FromDate));
                }
            }
        }

        public DateTime? ToDate
        {
            get => _toDate;
            set
            {
                if (_toDate != value)
                {
                    _toDate = value;
                    OnPropertyChanged(nameof(ToDate));
                }
            }
        }

        public string Status
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public interface IPaymentRepository
    {
        int CountPayments(PaymentQuery query);
        List<PaymentView> GetPayments(PaymentQuery query);

        List<ChargeView> GetCharges(ChargeQuery query);
    }

    public class PaymentRepository : IPaymentRepository
    {
        public int CountPayments(PaymentQuery query)
        {
            int total = 0;

            var predicate = query.BuildExpression();

            try
            {
                using (var context = new IndependienteEntities())
                {

                    total = context.PaymentView.Count(predicate);
                }
            }
            catch (DbEntityValidationException ex)
            {

            }
            catch (EntityException ex)
            {

            }

            return total;
        }

        public List<ChargeView> GetCharges(ChargeQuery query)
        {
            List<ChargeView> charges = new List<ChargeView>();

            var predicate = query.BuildExpression();

            try
            {
                using (var context = new IndependienteEntities())
                {
                    var paymentsForSearch = context.ChargeView
                        .Where(predicate)
                        
                        .ToList();

                    if (paymentsForSearch != null)
                    {
                        charges = paymentsForSearch;
                    }
                }
            }
            catch (DbUpdateException ex)
            {

            }
            catch (EntityException ex)
            {
            }

            return charges;
        }

        public List<PaymentView> GetPayments(PaymentQuery query)
        {
            List<PaymentView> payments = new List<PaymentView>();

            var predicate = query.BuildExpression();

            try
            {
                using (var context = new IndependienteEntities())
                {
                    var paymentsForSearch = context.PaymentView
                        .Where(predicate)
                         .OrderBy(x => x.RegistrationDate)
                         .Skip((query.PageNumber - 1) * query.PageSize)
                         .Take(query.PageSize)
                        .ToList();

                    if (paymentsForSearch != null)
                    {
                        payments = paymentsForSearch;
                    }
                }
            }
            catch (DbUpdateException ex)
            {

            }
            catch (EntityException ex)
            {
            }

            return payments;
        }
    }
}
