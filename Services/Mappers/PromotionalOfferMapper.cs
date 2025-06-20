using Independiente.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.Services.Mappers
{
    public static class PromotionalOfferMapper
    {

        public static DataAccess.PromotionalOffer ToDataModel(this Model.PromotionalOffer source)
        {
            DataAccess.PromotionalOffer promotionalOffer = new DataAccess.PromotionalOffer();

            if (source != null)
            {
                promotionalOffer = new DataAccess.PromotionalOffer
                {
                    PromotionalOfferId = source.PromotionalOfferId,
                    InterestRate = (decimal)source.InteresRate,
                    LoanTerm = (int)source.LoanTerm,
                    PaymentFrecuency = source.PaymenteFrecuency.ToString(),
                    IVA = source.IVA,
                    Name = source.Name,
                    Status = source.Status.ToString()
                };
            }           
            return promotionalOffer;
        }
                    
        public static Model.PromotionalOffer ToViewModel(this DataAccess.PromotionalOffer source)
        {
            Model.PromotionalOffer promotionalOffer = new Model.PromotionalOffer();

            if ( source != null )
            {
                promotionalOffer = new Model.PromotionalOffer
                {
                    PromotionalOfferId = source.PromotionalOfferId,
                    InteresRate = (decimal)source.InterestRate,
                    LoanTerm = (int)source.LoanTerm,
                    PaymenteFrecuency = (PromotionalOfferPaymentFrequencies)Enum.Parse(typeof(PromotionalOfferPaymentFrequencies), source.PaymentFrecuency),
                    IVA = source.IVA,
                    Name = source.Name,
                    Status = (PromotionalOfferStates)Enum.Parse(typeof(PromotionalOfferStates), source.Status)
                };
            }           
            return promotionalOffer;
        }
    }
}
