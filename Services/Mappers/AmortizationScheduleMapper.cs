using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.Services.Mappers
{
    public static class AmortizationScheduleMapper
    {
        public static DataAccess.AmortizationSchedule ToDataModel(this Model.AmortizationSchedule source)
        {
            DataAccess.AmortizationSchedule amortizationSchedule = new DataAccess.AmortizationSchedule();

            if (source != null)
            {
                amortizationSchedule = new DataAccess.AmortizationSchedule
                {
                    PaymentNumber = source.PaymentNumber,
                    PaymentDate = source.PaymentDate,
                    FixedPayment = source.FixedPayment,
                    Interest = source.Interest,
                    OutstandingBalance = source.OutstandingBalance,
                    Status = source.Status == null? "Pending" : source.Status,
                    CreditId = source.CreditApplication.CreditApplicationId
                };
            }
            return amortizationSchedule;
        }

        public static Model.AmortizationSchedule ToViewModel(this DataAccess.AmortizationSchedule source)
        {
            Model.AmortizationSchedule amortizationSchedule = new Model.AmortizationSchedule();

            if (source != null)
            {
                return new Model.AmortizationSchedule
                {
                    PaymentNumber = source.PaymentNumber,
                    PaymentDate = source.PaymentDate,
                    FixedPayment = source.FixedPayment,
                    Interest = source.Interest,
                    OutstandingBalance = source.OutstandingBalance,
                    Status = source.Status,
                    // CreditApplication = CreditApplicationMapper.ToViewModel(source.CreditApplication)
                };
            }
            return amortizationSchedule;
        }
    }
}
