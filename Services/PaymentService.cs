using Independiente.DataAccess.Repositories;
using Independiente.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.Services
{

    public interface IPaymentService
    {
        int CountPayments(PaymentQuery query);
        List<PaymentView> GetPayments(PaymentQuery query);
    }
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public int CountPayments(PaymentQuery query)
        {
            int result = 0;
            if (query != null)
            {
                result = _paymentRepository.CountPayments(query);
            }
            return result;
        }

        public List<PaymentView> GetPayments(PaymentQuery query)
        {
            List<PaymentView> result = new List<PaymentView>();

            if (query != null)
            {
                result = _paymentRepository.GetPayments(query);
            }
            return result;
        }
    }
}

