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
                    LoanAmount = source.LoanAmount.HasValue
                    ? source.LoanAmount.Value
                    : 0m,
                    LoanApplicationDate = source.LoanApplicationDate,
                    Status = source.Status.ToString(),
                    ClientId = source.Client.ClientId,
                    PromotionalOfferId = source.PromotionalOffer.PromotionalOfferId,
                    File = new DataAccess.File
                    {
                        ClientId = source.Client.ClientId,
                        Type = source.File.FileType.ToString(),
                        File1 = source.File.FileContent
                    }
                };
            }

            return creditApplication;
        }

        public static DataAccess.CreditApplication ToDataModelWithoutClient(this Model.CreditApplication source)
        {
            DataAccess.CreditApplication creditApplication = new DataAccess.CreditApplication();

            if (source != null)
            {
                creditApplication = new DataAccess.CreditApplication
                {
                    CreditApplicationId = source.CreditApplicationId,
                    LoanAmount = source.LoanAmount.HasValue
                    ? source.LoanAmount.Value
                    : 0m,
                    LoanApplicationDate = source.LoanApplicationDate,
                    Status = source.Status.ToString(),
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

        public static Model.CreditApplication ToViewModelWithFile(this DataAccess.CreditApplication source)
        {
            if (source == null) return null;

            var creditApplication = new Model.CreditApplication
            {
                CreditApplicationId = source.CreditApplicationId,
                LoanAmount =  (decimal)source.LoanAmount,
                LoanApplicationDate = source.LoanApplicationDate,
                Status = (CreditApplicationStates)Enum.Parse(typeof(CreditApplicationStates), source.Status),
                Client = source.Client != null ? ClientMapper.ToViewModel(source.Client) : null,
                PromotionalOffer = source.PromotionalOffer != null ? PromotionalOfferMapper.ToViewModel(source.PromotionalOffer) : null,
                File = source.File != null ? FileMapper.ToViewModel(source.File) : null
            };

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
