using Independiente.DataAccess;
using Independiente.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = Independiente.Model.File;

namespace Independiente.Services.Mappers
{
    public static class CreditApplicationMapper
    {
        public static DataAccess.CreditApplication ToDataModel(this Model.CreditApplication source)
        {
            DataAccess.CreditApplication creditApplication = new DataAccess.CreditApplication();

            if (source != null)
            {
                creditApplication = new DataAccess.CreditApplication
                {
                    CreditApplicationId = source.CreditApplicationId,
                    LoanAmount = (decimal)source.LoanAmount,
                    LoanApplicationDate = source.LoanApplicationDate,
                    Status = source.Status.ToString(),
                    Client = source?.Client != null ? ClientMapper.ToDataModel(source.Client) : null,
                    PromotionalOffer = source?.PromotionalOffer != null ? PromotionalOfferMapper.ToDataModel(source.PromotionalOffer) : null,
                };
            }

            return creditApplication;
        }



        public static Model.CreditApplication ToViewModel(this DataAccess.CreditApplication source)
        {
            Model.CreditApplication creditApplication = new Model.CreditApplication();

            if (source != null)
            {
                creditApplication = new Model.CreditApplication
                {
                    CreditApplicationId = source.CreditApplicationId,
                    LoanAmount = (decimal)source.LoanAmount,
                    LoanApplicationDate = source.LoanApplicationDate,
                    Status = (CreditApplicationStates)Enum.Parse(typeof(CreditApplicationStates), source.Status),
                    Client = source?.Client != null ? ClientMapper.ToViewModel(source.Client) : null,
                    PromotionalOffer = source?.PromotionalOffer != null ? PromotionalOfferMapper.ToViewModel(source.PromotionalOffer) : null,
                };
            }
            return creditApplication;
        }

        public static Model.CreditApplication ToViewModel(this DataAccess.CreditApplicationListView source)
        {
            Model.CreditApplication creditApplication = new Model.CreditApplication();

            if (source != null)
            {
                creditApplication = new Model.CreditApplication
                {
                    CreditApplicationId = source.CreditApplicationId,
                    LoanAmount = source.LoanAmount,
                    LoanApplicationDate = source.LoanApplicationDate,
                    Status = (CreditApplicationStates)Enum.Parse(typeof(CreditApplicationStates), source.Status),
                    Client = new Independiente.Model.Client
                    {
                        ClientId = source.ClientId,
                        PersonalData = new Independiente.Model.PersonalData
                        {
                            Name = source.Name,
                            LastName = source.Lastname,
                            Surname = source.Surname,
                            RFC = source.RFC
                        }
                    }
                };
            }

            return creditApplication;
        }
    }
}
