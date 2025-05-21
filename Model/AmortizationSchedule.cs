using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.Model
{
    public class AmortizationSchedule
    {
        public int PaymentNumber { get; set; }
        public System.DateTime PaymentDate { get; set; }
        public decimal FixedPayment { get; set; }
        public decimal Interest { get; set; }
        public decimal OutstandingBalance { get; set; }
        public string Status { get; set; }
        public CreditApplication CreditApplication { get; set; }
    }
}
