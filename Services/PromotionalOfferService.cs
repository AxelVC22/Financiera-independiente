using Independiente.DataAccess;
using Independiente.DataAccess.Repositories;
using Independiente.Model;
using Independiente.Services.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.Services
{
    public interface IPromotionalOfferService
    {
        int CountPromotionalOffers(PromotionalOfferQuery query);

        List<Independiente.Model.PromotionalOffer> GetPromotionalOffers(PromotionalOfferQuery query);

        Independiente.Model.PromotionalOffer GetPromotionalOffer(int promotionalOfferId);

        int AddPromotionalOffer(Independiente.Model.PromotionalOffer promotionalOffer);

        int UpdatePromotionalOffer(Independiente.Model.PromotionalOffer promotionalOffer);

        int DeletePromotionalOffer(Independiente.Model.PromotionalOffer promotionalOffer);
        
        List<Independiente.Model.PromotionalOffer> GetAllPromotionalOffers();

    }

    internal class PromotionalOfferService : IPromotionalOfferService
    {
        private readonly IPromotionalOfferRepository _promotionalOfferRepository;

        public PromotionalOfferService(IPromotionalOfferRepository promotionalOfferRepository)
        {
            _promotionalOfferRepository = promotionalOfferRepository;
        }
        private bool ValidateQuery(PromotionalOfferQuery query)
        {
            if (!string.IsNullOrEmpty(query.Name))
            {
                FieldValidator.IsValidName(query.Name);
            }
            return true;
        }

        public int CountPromotionalOffers(PromotionalOfferQuery query)
        {
            int total = 0;
            if (ValidateQuery(query))
            {
                total = _promotionalOfferRepository.CountPromotionalOffers(query);
            }
            return total;
        }

        public List<Independiente.Model.PromotionalOffer> GetPromotionalOffers(PromotionalOfferQuery query)
        {
            List<Independiente.DataAccess.PromotionalOffer> promotionalOffersList = new List<Independiente.DataAccess.PromotionalOffer>();

            List<Independiente.Model.PromotionalOffer> promotionalOffers1 = new List<Model.PromotionalOffer>();

            if (ValidateQuery(query))
            {
                promotionalOffersList = _promotionalOfferRepository.GetPromotionalOffers(query);

                foreach (var c in promotionalOffersList)
                {
                    promotionalOffers1.Add(PromotionalOfferMapper.ToViewModel(c));

                }
            }            
            return promotionalOffers1;
        }

        public Independiente.Model.PromotionalOffer GetPromotionalOffer(int promotionalOfferId)
        {
            Independiente.Model.PromotionalOffer promotionalOffer = null;

            if (promotionalOfferId > 0)
            {
                var application = _promotionalOfferRepository.GetPromotionalOffer(promotionalOfferId);
                promotionalOffer = PromotionalOfferMapper.ToViewModel(application);
            }
            return promotionalOffer;
        }

        public int AddPromotionalOffer(Independiente.Model.PromotionalOffer promotionalOffer)
        {
            int id = 0;

            if (promotionalOffer != null)
            {
                try
                {
                    if (ValidatePromotionalOffer(promotionalOffer))
                    {
                        PromotionalOfferQuery query = new PromotionalOfferQuery();
                        query.Name = promotionalOffer.Name;
                        if (CountPromotionalOffers(query) == 0)
                        {
                            id = _promotionalOfferRepository.AddPromotionalOffer(PromotionalOfferMapper.ToDataModel(promotionalOffer));
                        }
                        else
                        {
                            throw new ArgumentException("El nombre de la promoción ya está en uso. Verifique e ingrese un nombre diferente.");
                        }
                    }
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException(ex.Message);
                }
            }
            return id;
        }

        public int UpdatePromotionalOffer(Independiente.Model.PromotionalOffer promotionalOffer)
        {
            int affectedRows = 0;

            if (promotionalOffer != null)
            {
                try
                {
                    if (ValidatePromotionalOffer(promotionalOffer))
                    {
                        var marches = _promotionalOfferRepository.GetPromotionalOfferByName(promotionalOffer.Name);
                        bool isDuplicateNameUsedByAnother = marches.Any(p => p.PromotionalOfferId != promotionalOffer.PromotionalOfferId);
                        if (!isDuplicateNameUsedByAnother)
                        {
                            affectedRows = _promotionalOfferRepository.UpdatePromotionalOffer(PromotionalOfferMapper.ToDataModel(promotionalOffer));
                        }
                        else
                        {
                            throw new ArgumentException("El nombre de la promoción ya está en uso. Verifique e ingrese un nombre diferente.");
                        }
                    }
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException(ex.Message);
                }
            }
            return affectedRows;
        }

        public int DeletePromotionalOffer(Independiente.Model.PromotionalOffer promotionalOffer)
        {
            int result = 0;

            if (promotionalOffer != null)
            {
                result = _promotionalOfferRepository.DeletePromotionalOffer(PromotionalOfferMapper.ToDataModel(promotionalOffer));
            }
            return result;
        }
        
        public List<Independiente.Model.PromotionalOffer> GetAllPromotionalOffers()
        {
            List<Independiente.Model.PromotionalOffer> promotionalOffers1 = new List<Independiente.Model.PromotionalOffer>();
            using (var context = new IndependienteEntities())
            {
                List<DataAccess.PromotionalOffer> promotionalOffers = new List<DataAccess.PromotionalOffer>();
                promotionalOffers = context.PromotionalOffer.ToList();

                foreach (var c in promotionalOffers)
                {
                    promotionalOffers1.Add(PromotionalOfferMapper.ToViewModel(c));
                }
                
            }
            return promotionalOffers1;
        }

        private bool ValidatePromotionalOffer(Independiente.Model.PromotionalOffer promotionalOffer)
        {
            return FieldValidator.IsValidName(promotionalOffer.Name) &&
                   FieldValidator.IsValidLoanTerm(promotionalOffer.LoanTerm) &&
                   FieldValidator.IsValidInterestRate(promotionalOffer.InteresRate) &&
                   FieldValidator.IsValidIVA(promotionalOffer.IVA);
        }
    }
}
