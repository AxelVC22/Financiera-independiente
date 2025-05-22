using Independiente.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.DataAccess.Repositories
{

    public interface IPromotionalOfferRepository
    {
        int AddPromotionalOffer(PromotionalOffer promotionalOffer);

        int DeletePromotionalOffer(PromotionalOffer promotionalOffer);    
    }
    public class PromotionalOfferRepository : IPromotionalOfferRepository
    {
        public int AddPromotionalOffer(PromotionalOffer promotionalOffer)
        {
            int id = 0;

            try
            {
                using (var context = new IndependienteEntities())
                {
                    var newPromotionalOffer = new PromotionalOffer
                    {
                        InterestRate = promotionalOffer.InterestRate,
                        IVA = promotionalOffer.IVA,
                        LoanTerm = promotionalOffer.LoanTerm,
                        PaymentFrecuency = promotionalOffer.PaymentFrecuency
                    };
                    context.PromotionalOffer.Add(newPromotionalOffer);
                    context.SaveChanges();
                    id = newPromotionalOffer.PromotionalOfferId;
                }
            }
            catch (DbUpdateException ex)
            {

            }
            catch (EntityException ex)
            {

            }

            return id;
        }


        //Only for tests
        public int DeletePromotionalOffer(PromotionalOffer promotionalOffer)
        {
            int result = 0;

            try
            {
                using (var context = new IndependienteEntities())
                {
                    var promotionalOfferForRemove = context.PromotionalOffer.Find(promotionalOffer.PromotionalOfferId);
                    if (promotionalOfferForRemove != null)
                    {
                        context.PromotionalOffer.Remove(promotionalOfferForRemove);
                        result = context.SaveChanges();
                    }
                }
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine("Excepcion: " + e.Message);
            }

            return result;
        }
    }
}
