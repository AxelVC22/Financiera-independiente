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
                    PaymentFrecuency = source.PaymenteFrecuency,
                    IVA = source.IVA
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
                    PaymenteFrecuency = source.PaymentFrecuency,
                    IVA = source.IVA,
                    Description = source.Name
                };
            }           
            return promotionalOffer;
        }
    }
}
