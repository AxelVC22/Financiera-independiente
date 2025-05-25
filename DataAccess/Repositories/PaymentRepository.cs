using Independiente.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
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
            return c =>
                (string.IsNullOrEmpty(BankName) || c.BankName == BankName) &&
                (!FromDate.HasValue || c.PaymentDate >= FromDate) &&
                (!ToDate.HasValue || c.PaymentDate <= ToDate) &&
                (Status == null || c.Status == PaymentStatus.Pending.ToString());
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

        int UploadCharges(List<ChargeView> charges, Payment payment);
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
            catch (Exception ex)
            {
                throw DbExceptionHandler.Handle(ex);
            }

            return total;
        }

        public List<ChargeView> GetCharges(ChargeQuery query)
        {
            List<ChargeView> charges = new List<ChargeView>();

            var predicate = query.BuildExpression();

            using (var context = new IndependienteEntities())
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {

                    var chargesForSearch = context.ChargeView
                        .Where(predicate)
                        .ToList();

                    if (chargesForSearch.Any())
                    {
                        decimal totalAmount = 0;

                        var bank = context.Bank
                            .FirstOrDefault(b => b.Name == query.BankName);

                        foreach (var charge in chargesForSearch)
                        {
                            totalAmount = totalAmount + charge.FixedPayment;
                            var chargeForUpdate = context.AmortizationSchedule
                                .FirstOrDefault(c => c.CreditId == charge.CreditId && c.PaymentNumber == charge.PaymentNumber);

                            if (chargeForUpdate != null)
                            {
                                chargeForUpdate.Status = PaymentStatus.InProgress.ToString();
                                context.Entry(chargeForUpdate).State = EntityState.Modified;
                            }
                        }

                        var payment = new Payment
                        {
                            TotalAmount = totalAmount,
                            ActualAmount = 0,
                            BankId = bank.BankId,
                            EmployeeId = App.SessionService.CurrentUser.EmployeeId,
                            RegistrationDate = DateTime.Now,
                            TotalCredits = chargesForSearch.Count,
                            ActualCredits = 0,
                            Status = PaymentStatus.Pending.ToString(),

                        };
                        context.Payment.Add(payment);

                        context.SaveChanges();
                        transaction.Commit();

                        charges = chargesForSearch;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw DbExceptionHandler.Handle(ex);
                }
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
            catch (Exception ex)
            {
                throw DbExceptionHandler.Handle(ex);
            }

            return payments;
        }

        public int UploadCharges(List<ChargeView> charges, Payment payment)
        {
            int result = 0;

            using (var context = new IndependienteEntities())
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var c in charges)
                    {
                        var amortization = context.AmortizationSchedule
                            .FirstOrDefault(a => a.PaymentDate == c.PaymentDate);

                        if (amortization == null)
                            continue;

                        if (c.Status == PaymentStatus.Failed.ToString())
                        {
                            int nextPaymentNumber = amortization.PaymentNumber + 1;

                            var nextCharge = context.AmortizationSchedule
                                .FirstOrDefault(a => a.CreditId == c.CreditId && a.PaymentNumber == nextPaymentNumber);

                            if (nextCharge != null)
                            {
                                nextCharge.FixedPayment += amortization.FixedPayment;
                                nextCharge.OutstandingBalance += amortization.FixedPayment;
                                context.Entry(nextCharge).State = EntityState.Modified;
                            }
                            else
                            {
                                var nextPaymentDate = c.PaymentDate.AddDays(15);

                                var newAmortization = new AmortizationSchedule
                                {
                                    CreditId = amortization.CreditId,
                                    FixedPayment = amortization.FixedPayment,
                                    Interest = 0,
                                    OutstandingBalance = amortization.FixedPayment,
                                    PaymentNumber = nextPaymentNumber,
                                    PaymentDate = nextPaymentDate,
                                    Status = PaymentStatus.Pending.ToString()
                                };

                                context.AmortizationSchedule.Add(newAmortization);
                            }
                        }

                        amortization.Status = c.Status;
                        context.Entry(amortization).State = EntityState.Modified;
                    }

                    var paymentForUpdate = context.Payment.Find(payment.PaymentId);

                    paymentForUpdate.Status = payment.Status;
                    paymentForUpdate.UploadDate = payment.UploadDate;
                    paymentForUpdate.ActualCredits = payment.ActualCredits;
                    paymentForUpdate.ActualAmount = payment.ActualAmount;

                    context.Entry(paymentForUpdate).State = EntityState.Modified;

                    result = context.SaveChanges();
                    transaction.Commit(); 
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw DbExceptionHandler.Handle(ex);
                }
            }

            return result;

        }


    }
}
