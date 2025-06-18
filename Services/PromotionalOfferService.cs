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

        //provisional
        List<Independiente.Model.PromotionalOffer> GetAllPromotionalOffers();

    }

    internal class PromotionalOfferService : IPromotionalOfferService
    {
        private readonly IPromotionalOfferRepository _promotionalOfferRepository;

        public PromotionalOfferService(IPromotionalOfferRepository promotionalOfferRepository)
        {
            _promotionalOfferRepository = promotionalOfferRepository;
        }

        public int CountPromotionalOffers(PromotionalOfferQuery query)
        {
            return _promotionalOfferRepository.CountPromotionalOffers(query);
        }

        public List<Independiente.Model.PromotionalOffer> GetPromotionalOffers(PromotionalOfferQuery query)
        {
            List<Independiente.DataAccess.PromotionalOffer> promotionalOffersList = new List<Independiente.DataAccess.PromotionalOffer>();

            List<Independiente.Model.PromotionalOffer> promotionalOffers1 = new List<Model.PromotionalOffer>();

            promotionalOffersList = _promotionalOfferRepository.GetPromotionalOffers(query);
            foreach (var c in promotionalOffersList)
            {
                promotionalOffers1.Add(PromotionalOfferMapper.ToViewModel(c));

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
                id = _promotionalOfferRepository.AddPromotionalOffer(PromotionalOfferMapper.ToDataModel(promotionalOffer));

            }
            return id;
        }

        public int UpdatePromotionalOffer(Independiente.Model.PromotionalOffer promotionalOffer)
        {
            int affectedRows = 0;

            if (promotionalOffer != null)
            {
                affectedRows = _promotionalOfferRepository.UpdatePromotionalOffer(PromotionalOfferMapper.ToDataModel(promotionalOffer));
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

        //provisional
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
    }
}
