using Independiente.DataAccess.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class AmortizationSchedule
    {
        public IAmortizationScheduleRepository AmortizationScheduleRepository { get; set; } = new AmortizationScheduleRepository();

        [TestMethod]
        public void GetAmortizationScheduleSuccess()
        {
            var query = new AmortizationScheduleQuery
            {
                CreditApplicaitonId = 13,
                Status = "Pending"
            };

            var result = AmortizationScheduleRepository.GetAmortizationSchedule(query);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetAmortizationScheduleNotFound()
        {
            var query = new AmortizationScheduleQuery
            {
                //invalid CreditApplicationId
                CreditApplicaitonId = 0,
                Status = "Pending"
            };

            var result = AmortizationScheduleRepository.GetAmortizationSchedule(query);

            Console.WriteLine($"Result Count: {result.Count}");
            Assert.IsTrue(result.Count == 0);
        }
    }
}
