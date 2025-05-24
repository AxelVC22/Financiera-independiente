using Independiente.Model;
using Independiente.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.DataAccess.Repositories
{
    public class PromotionalOfferQuery : INotifyPropertyChanged, IQueryObject<PromotionalOffer>
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        private string _paymentFrecuency;
        
        private string _status;

        public string PaymentFrecuency
        {
            get => _paymentFrecuency;
            set
            {
                if (_paymentFrecuency != value)
                {
                    _paymentFrecuency = value;
                    OnPropertyChanged(nameof(PaymentFrecuency));
                }
            }
        }

        public string Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        public Expression<Func<PromotionalOffer, bool>> BuildExpression()
        {
            return c =>
                (string.IsNullOrEmpty(PaymentFrecuency) || c.PaymentFrecuency == PaymentFrecuency) &&
                (string.IsNullOrEmpty(Status) || c.Status == Status);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public interface IPromotionalOfferRepository
    {
        int CountPromotionalOffers(PromotionalOfferQuery query);

        List<PromotionalOffer> GetPromotionalOffers(PromotionalOfferQuery query);

        PromotionalOffer GetPromotionalOffer(int promotionalOfferId);

        int AddPromotionalOffer(PromotionalOffer promotionalOffer);

        int UpdatePromotionalOffer(PromotionalOffer promotionalOffer);

        int DeletePromotionalOffer(PromotionalOffer promotionalOffer);    
    }
    public class PromotionalOfferRepository : IPromotionalOfferRepository
    {
        public int CountPromotionalOffers(PromotionalOfferQuery query)
        {
            int total = 0;
            var predicate = query.BuildExpression();

            try
            {
                using (var context = new IndependienteEntities())
                {
                    total = context.PromotionalOffer.Count(predicate);
                }
            }
            catch (DbEntityValidationException ex)
            {

            }
            catch (EntityException ex)
            {

            }

            return total;
        }

        public List<PromotionalOffer> GetPromotionalOffers(PromotionalOfferQuery query)
        {
            List<PromotionalOffer> promotionalOffers = new List<PromotionalOffer>();

            var predicate = query.BuildExpression();

            try
            {
                using (var context = new IndependienteEntities())
                {
                    var promotionalOfferForSearch = context.PromotionalOffer
                        .Where(predicate)
                        .OrderBy(x => x.PromotionalOfferId)
                        .Skip((query.PageNumber - 1) * query.PageSize)
                        .Take(query.PageSize)
                        .ToList();

                    if (promotionalOfferForSearch != null)
                    {
                        promotionalOffers = promotionalOfferForSearch;
                    }
                }
            }
            catch (DbUpdateException ex)
            {
            }
            catch (EntityException ex)
            {
            }

            return promotionalOffers;
        }

        public PromotionalOffer GetPromotionalOffer(int promotionalOfferId)
        {
            PromotionalOffer promotionalOffer = new PromotionalOffer();

            try
            {
                using (var context = new IndependienteEntities())
                {
                    var promotionalOfferForSearch = context.PromotionalOffer.Find(promotionalOfferId);

                    if (promotionalOfferForSearch != null)
                    {
                        promotionalOffer = promotionalOfferForSearch;
                    }
                }
            }
            catch (DbUpdateException ex)
            {
            }
            catch (EntityException ex)
            {
            }

            return promotionalOffer;
        }

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

        public int UpdatePromotionalOffer(PromotionalOffer promotionalOffer)
        {
            int affectedRows = 0;

            try
            {
                using (var context = new IndependienteEntities())
                {
                    var existingPromotionalOffer = context.PromotionalOffer.Find(promotionalOffer.PromotionalOfferId);

                    if (existingPromotionalOffer != null)
                    {
                        existingPromotionalOffer.InterestRate = promotionalOffer.InterestRate;
                        existingPromotionalOffer.IVA = promotionalOffer.IVA;
                        existingPromotionalOffer.LoanTerm = promotionalOffer.LoanTerm;
                        existingPromotionalOffer.PaymentFrecuency = promotionalOffer.PaymentFrecuency;                        

                        affectedRows = context.SaveChanges();
                    }
                }
            }
            catch (DbUpdateException ex)
            {
            }
            catch (EntityException ex)
            {
            }

            return affectedRows;
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