using Independiente.DataAccess.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
    }
}
