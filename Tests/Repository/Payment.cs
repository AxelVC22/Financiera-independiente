using Independiente;
using Independiente.DataAccess;
using Independiente.DataAccess.Repositories;
using Independiente.Services;
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
        public IUserRepository userRepository { get; set; } = new Independiente.DataAccess.Repositories.UserRepository();

        [TestMethod]
        public void CountPaymentsSuccess()
        {
            var paymentQuery = new PaymentQuery
            {
                PageNumber = 1,
                PageSize = 10,
                BankName = "Santander",
                Status = "Pending",
                FromDate = new DateTime(2025, 05, 24),
                ToDate = new DateTime(2025, 05, 24),
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
                BankName = "Santander",
                Status = "Pending",
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
                Status = "Pending",
                FromDate = new DateTime(2025, 05, 24),
                ToDate = new DateTime(2025, 05, 24),
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
                BankName = "Santander",
                FromDate = new DateTime(2025, 05, 24),
                ToDate = new DateTime(2025, 05, 24),
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
                BankName = "Santander",
                Status = "Pending",
                FromDate = new DateTime(2025, 05, 24),
                ToDate = new DateTime(2025, 05, 24),
            };

            var result = paymentRepository.GetPayments(paymentQuery);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(2, result.First().PaymentId);

        }

        [TestMethod]
        public void GetChargesSuccessWithoutStatus()
        {
            App.SessionService = new Independiente.Services.SessionService(new UserRepository());
            App.SessionService.CurrentUser = new Independiente.Model.User
            {
                EmployeeId = 3
              
            };


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


        [TestMethod]
        public void TestUploadChargesSuccess()
        {
            int result = 0;

            var charge = new ChargeView
            {
                CLABE = "002180340118359714",
                ClientName = "Mauricio Diaz Tapia",
                BankName = "Santander",
                PaymentNumber = 5,
                PaymentDate = new DateTime(2025, 09, 04),
                FixedPayment = 464.67m,
                CreditId = 13,
                Status = "Completed"
            };

            List<ChargeView> list = new List<ChargeView>();
            list.Add(charge);

            var payment = new Independiente.DataAccess.Payment
            {
                PaymentId = 2
            };

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                result = paymentRepository.UploadCharges(list, payment);
                Assert.IsTrue(result > 0);
            }
        }

        [ExpectedException(typeof(InvalidOperationException))]


        [TestMethod]
        public void TestUploadChargesFailByNotSpecifiedStatus()
        {
            int result = 0;

            var charge = new ChargeView
            {
                CLABE = "002180340118359714",
                ClientName = "Mauricio Diaz Tapia",
                BankName = "Santander",
                PaymentNumber = 5,
                PaymentDate = new DateTime(2025, 09, 04),
                FixedPayment = 464.67m,
                CreditId = 13,
               // without status
            };

            List<ChargeView> list = new List<ChargeView>();
            list.Add(charge);

            var payment = new Independiente.DataAccess.Payment
            {
                PaymentId = 2
            };

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                result = paymentRepository.UploadCharges(list, payment);
            }
        }

    }
}
