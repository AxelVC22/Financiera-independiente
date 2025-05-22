using Independiente.DataAccess.Repositories;
using Independiente.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Independiente.Services.Mappers;

namespace Independiente.Services
{

    public interface IPaymentService
    {
        int CountPayments(PaymentQuery query);
        List<Model.Payment> GetPayments(PaymentQuery query);
    }
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        private bool ValidateQuery(PaymentQuery query)
        {

            if ((query.ToDate != null && query.FromDate != null) && (query.FromDate > query.ToDate))
            {
                throw new ArgumentException("Rango de fecha invalido");
            }

           

            return true;
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

        public List<Model.Payment> GetPayments(PaymentQuery query)
        {
            List<PaymentView> result = new List<PaymentView>();

            List<Model.Payment> payments = new List<Model.Payment>();

            if (ValidateQuery(query))
            {

                result = _paymentRepository.GetPayments(query);

                foreach (var p in result)
                {
                    payments.Add(PaymentMapper.ToViewModel(p));
                }
            }
            return payments;
        }
    }
}

