using Independiente.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.Services.Mappers
{
    public static class PaymentMapper
    {
        public static Model.Payment ToViewModel(this DataAccess.PaymentView source)
        {
            Model.Payment payment = new Model.Payment();

            if (source != null)
            {
                return new Model.Payment
                {
                    PaymentId = source.PaymentId,
                    Employee = new Model.Employee { 
                    PersonalData = new PersonalData { Name = source.EmployeeName, LastName = source.Lastname, Surname = source.Surname}
                    },
                    TotalAmount = source.TotalAmount,
                    ActualAmount = source.ActualAmount,
                    RegistrationDate = source.RegistrationDate,
                    Bank = new Bank { Name = source.BankName },
                    TotalCredits = source.TotalCredits,
                    ActualCredits = source.ActualCredits,
                    Status = (PaymentStatus)Enum.Parse(typeof(PaymentStatus), source.Status),
                    IsUploadEnabled = (PaymentStatus)Enum.Parse(typeof(PaymentStatus), source.Status) == PaymentStatus.Pending,
                    UploadDate = source?.UploadDate
                };
            }

            return payment;
        }

    }
}
