using Independiente.DataAccess.Repositories;
using Independiente.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Tests
{
    [TestClass]
    public class CreditApplication
    {
        public ICreditApplicationRepository CreditApplicationRepository { get; set; } = new CreditApplicationRepository();

        [TestMethod]
        public void TestCountCreditApplicationsSuccess()
        {
            int result = 0;

            result = CreditApplicationRepository.CountCreditApplications(new CreditApplicationQuery
            {
                PageNumber = 1,
                PageSize = 10,
                RFC = "ARPS9288383",
                Status = "Accepted",
                FromDate = new DateTime(2025, 05, 07),
                ToDate = new DateTime(2025, 05, 07),
            });
            
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void TestCountCreditApplicationsSuccessWithoutRFC()
        {
            int result = 0;

            result = CreditApplicationRepository.CountCreditApplications(new CreditApplicationQuery
            {
                PageNumber = 1,
                PageSize = 10,
                Status = "Accepted",
                FromDate = new DateTime(2025, 05, 07),
                ToDate = new DateTime(2025, 05, 07),
            });

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void TestCountCreditApplicationsSuccessWithoutStatus()
        {
            int result = 0;

            result = CreditApplicationRepository.CountCreditApplications(new CreditApplicationQuery
            {
                PageNumber = 1,
                PageSize = 10,
                RFC = "ARPS9288383",
                FromDate = new DateTime(2025, 05, 07),
                ToDate = new DateTime(2025, 05, 07),
            });

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void TestCountCreditApplicationsSuccessWithoutDateRange()
        {
            int result = 0;

            result = CreditApplicationRepository.CountCreditApplications(new CreditApplicationQuery
            {
                PageNumber = 1,
                PageSize = 10,
                RFC = "ARPS9288383",
                Status = "Accepted",
            });

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void TestGetCreditApplicationsSuccess()
        {

            var result = CreditApplicationRepository.GetCreditApplications(new CreditApplicationQuery
            {
                PageNumber = 1,
                PageSize = 10,
                RFC = "ARPS9288383",
                Status = "Accepted",
                FromDate = new DateTime(2025, 05, 07),
                ToDate = new DateTime(2025, 05, 07),
            });

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void TestGetCreditApplicationsSuccessNotFound()
        {

            var result = CreditApplicationRepository.GetCreditApplications(new CreditApplicationQuery
            {
                PageNumber = 1,
                PageSize = 10,
                RFC = "InventedRFC",
                Status = "Accepted",
                FromDate = new DateTime(2025, 05, 07),
                ToDate = new DateTime(2025, 05, 07),
            });

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void TestAddCreditApplicationSuccess()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var newApp = new Independiente.DataAccess.CreditApplication
                {
                    LoanApplicationDate = DateTime.Now,
                    LoanAmount = 10000,
                    Status = "Pending",
                    ClientId = 18,
                    File = new Independiente.DataAccess.File
                    {
                        ClientId = 18,
                        FileId = 3,
                        Type = "CA",
                        File1 = new byte[2],
                    },
                    PromotionalOfferId = 32

                };

                CreditApplicationRepository.AddCreditApplication(newApp); 

                var count = CreditApplicationRepository.CountCreditApplications(new CreditApplicationQuery
                {
                   Status = "Pending",
                });

                Assert.AreEqual(4, count);

            }
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void TestAddCreditApplicationFailByNotFountClient()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var newApp = new Independiente.DataAccess.CreditApplication
                {
                    LoanApplicationDate = DateTime.Now,
                    LoanAmount = 10000,
                    Status = "Pending",
                    //invalid cient id
                    ClientId = 0,
                    File = new Independiente.DataAccess.File
                    {
                        ClientId = 0,
                        FileId = 3,
                        Type = "CA",
                        File1 = new byte[2],
                    },
                    PromotionalOfferId = 32

                };

                CreditApplicationRepository.AddCreditApplication(newApp);
            }
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void TestAddCreditApplicationFailByNotCorrectFile()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var newApp = new Independiente.DataAccess.CreditApplication
                {
                    LoanApplicationDate = DateTime.Now,
                    LoanAmount = 10000,
                    Status = "Pending",
                    ClientId = 3,
                    File = new Independiente.DataAccess.File
                    {
                      //nothing here
                    },
                    PromotionalOfferId = 32

                };

                CreditApplicationRepository.AddCreditApplication(newApp);
            }
        }

        [TestMethod]
        public void TestGetCreditApplicationSuccess()
        {

            var result = CreditApplicationRepository.GetCreditApplication(13
            );

            Assert.AreEqual(13,result.CreditApplicationId );
        }

        [TestMethod]
        public void TestGetDocumentsSuccess()
        {
            var result = CreditApplicationRepository.GetDocument(18, "CA");

            Assert.AreEqual(2, result.FileId);
        }

        [TestMethod]
        public void TestGetDocumentNotFound()
        {
            var result = CreditApplicationRepository.GetDocument(18, "NotFound");

            Assert.AreEqual(0, result.FileId);
        }

        [TestMethod]
        public void TestSubmitAcceptedDecisionSuccess()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var report = new Independiente.DataAccess.Report
                {
                    CreditApplicationId = 3,
                    CreditApplication = new Independiente.DataAccess.CreditApplication
                    {
                        CreditApplicationId = 3,
                        Status = "Accepted"
                    },
                    Notes = "nota",
                    ReviewingDate = DateTime.Now,

                };

                var Amortization = new Independiente.DataAccess.AmortizationSchedule
                {
                    PaymentNumber = 1,
                    PaymentDate = DateTime.Now,
                    FixedPayment = 1000,
                    Interest = 0.1m,
                    OutstandingBalance = 9000,
                    CreditId = 3,
                    Status = "Pending"
                };

                List<Independiente.DataAccess.AmortizationSchedule> list = new List<Independiente.DataAccess.AmortizationSchedule>();

                list.Add(Amortization);

                var result = CreditApplicationRepository.SubmitDecision(report, list);
                Console.WriteLine(result);
                Assert.IsTrue( result > 0);
            }
        }


        [TestMethod]
        public void TestSubmitRejectedDecisionSuccess()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var report = new Independiente.DataAccess.Report
                {
                    CreditApplicationId = 3,
                    CreditApplication = new Independiente.DataAccess.CreditApplication
                    {
                        CreditApplicationId = 3,
                        Status = "Rejected"
                    },
                    Notes = "nota",
                    ReviewingDate = DateTime.Now,

                };

                var creditPolicies = new Independiente.DataAccess.CreditPolicy
                {
                    Status = "Active",
                    CreditPolicyId = 3
                };

                report.CreditPolicy = new HashSet<Independiente.DataAccess.CreditPolicy>
                {
                    creditPolicies
                };

                var result = CreditApplicationRepository.SubmitDecision(report, null);
                Assert.IsTrue(result > 0);
            }
        }


        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void TestSubmitDecisionFailByInvalidStatus()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var report = new Independiente.DataAccess.Report
                {
                    //application accepted already
                    CreditApplicationId = 13,
                    CreditApplication = new Independiente.DataAccess.CreditApplication
                    {
                        CreditApplicationId = 13,
                        Status = "Rejected"
                    },
                    Notes = "nota",
                    ReviewingDate = DateTime.Now,

                };

                var creditPolicies = new Independiente.DataAccess.CreditPolicy
                {
                    Status = "Active",
                    CreditPolicyId = 3
                };

                report.CreditPolicy = new HashSet<Independiente.DataAccess.CreditPolicy>
                {
                    creditPolicies
                };

                var result = CreditApplicationRepository.SubmitDecision(report, null);
              
            }
        }

        [TestMethod]
        public void TestSubmitNotFoundApplication()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var report = new Independiente.DataAccess.Report
                {
                    CreditApplicationId = 0,
                    CreditApplication = new Independiente.DataAccess.CreditApplication
                    {
                        CreditApplicationId = 0,
                        Status = "Rejected"
                    },
                    Notes = "nota",
                    ReviewingDate = DateTime.Now,

                };

                var creditPolicies = new Independiente.DataAccess.CreditPolicy
                {
                    Status = "Active",
                    CreditPolicyId = 3
                };

                report.CreditPolicy = new HashSet<Independiente.DataAccess.CreditPolicy>
                {
                    creditPolicies
                };

                var result = CreditApplicationRepository.SubmitDecision(report, null);
                Assert.AreEqual(0, result);
            }
        }

       


    }
}
