using Independiente.DataAccess.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Tests
{
    [TestClass]
    public class Payment
    {
        public IPaymentRepository paymentRepository { get; set; } = new Independiente.DataAccess.Repositories.PaymentRepository();

        [TestMethod]
        public void CountPaymentsSuccess()
        {
            var paymentQuery = new PaymentQuery
            {
                PageNumber = 1,
                PageSize = 10,
                BankName = "BBVA",
                Status = "Completed",
                FromDate = new DateTime(2025, 05, 18),
                ToDate = new DateTime(2025, 05, 18),
            };

            var result = paymentRepository.CountPayments(paymentQuery);

            Assert.AreEqual(1, result);

        }

        [TestMethod]
        public void CountPaymentsSuccessWithoutDates()
        {
            var paymentQuery = new PaymentQuery
            {
                PageNumber = 1,
                PageSize = 10,
                BankName = "BBVA",
                Status = "Completed",
            };

            var result = paymentRepository.CountPayments(paymentQuery);

            Assert.AreEqual(1, result);

        }


        [TestMethod]
        public void CountPaymentsSuccessWithoutBank()
        {
            var paymentQuery = new PaymentQuery
            {
                PageNumber = 1,
                PageSize = 10,
                Status = "Completed",
                FromDate = new DateTime(2025, 05, 18),
                ToDate = new DateTime(2025, 05, 18),
            };

            var result = paymentRepository.CountPayments(paymentQuery);

            Assert.AreEqual(1, result);

        }

        [TestMethod]
        public void CountPaymentsSuccessWithoutStatus()
        {
            var paymentQuery = new PaymentQuery
            {
                PageNumber = 1,
                PageSize = 10,
                BankName = "BBVA",
                FromDate = new DateTime(2025, 05, 18),
                ToDate = new DateTime(2025, 05, 18),
            };

            var result = paymentRepository.CountPayments(paymentQuery);

            Assert.AreEqual(1, result);

        }

        [TestMethod]
        public void GetPaymentsSuccess()
        {
            var paymentQuery = new PaymentQuery
            {
                PageNumber = 1,
                PageSize = 10,
                BankName = "BBVA",
                Status = "Completed",
                FromDate = new DateTime(2025, 05, 18),
                ToDate = new DateTime(2025, 05, 18),
            };

            var result = paymentRepository.GetPayments(paymentQuery);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result.First().PaymentId);

        }

        [TestMethod]
        public void GetChargesSuccessWithoutStatus()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var chargeQuery = new ChargeQuery
                {
                    PageNumber = 1,
                    PageSize = 10,
                    BankName = "Santander",
                    FromDate = new DateTime(2025, 05, 07),
                    ToDate = new DateTime(2025, 08, 05),
                };

                var result = paymentRepository.GetCharges(chargeQuery);

                Console.WriteLine(result);
                Assert.AreEqual(4, result.Count);
            }
        }
    }
}
