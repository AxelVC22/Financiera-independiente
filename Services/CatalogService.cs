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
    public interface ICatalogService
    {
        int CountBanks(CatalogQuery query);

        List<Independiente.Model.Bank> GetBanks(CatalogQuery query);

        Independiente.Model.Bank GetBank(int bankId);

        int AddBank(Independiente.Model.Bank bank);

        int UpdateBank(Independiente.Model.Bank bank);

        int DeleteBank(Independiente.Model.Bank bank);
    }
    public class CatalogService : ICatalogService
    {
        private readonly ICatalogRepository _catalogRepository;

        public CatalogService(ICatalogRepository catalogRepository)
        {
            _catalogRepository = catalogRepository;
        }

        private bool ValidateQuery(CatalogQuery query)
        {
            if (string.IsNullOrEmpty(query.Name))
            {
                FieldValidator.IsValidName(query.Name);
            }            
            return true;
        }

        public int CountBanks(CatalogQuery query)
        {
            int total = 0;
            if (ValidateQuery(query))
            {
                total = _catalogRepository.CountBanks(query);
            }
            return total;
        }

        public List<Independiente.Model.Bank> GetBanks(CatalogQuery query)
        {
            List<Independiente.DataAccess.Bank> banksList = new List<Independiente.DataAccess.Bank>();

            List<Independiente.Model.Bank> banks1 = new List<Model.Bank>();

            if (ValidateQuery(query))
            {
                banksList = _catalogRepository.GetBanks(query);

                foreach (var c in banksList)
                {
                    banks1.Add(CatalogMapper.ToViewModel(c));
                }
            }
            return banks1;
        }

        public Independiente.Model.Bank GetBank(int bankId)
        {
            Independiente.Model.Bank bank = null;

            if (bankId > 0)
            {
                var application = _catalogRepository.GetBank(bankId);
                bank = CatalogMapper.ToViewModel(application);
            }
            return bank;
        }

        public int AddBank(Independiente.Model.Bank bank)
        {
            int id = 0;

            if (bank != null)
            {
                id = _catalogRepository.AddBank(CatalogMapper.ToDataModel(bank));

            }
            return id;
        }

        public int UpdateBank(Independiente.Model.Bank bank)
        {
            int affectedRows = 0;

            if (bank != null)
            {
                affectedRows = _catalogRepository.UpdateBank(CatalogMapper.ToDataModel(bank));
            }
            return affectedRows;
        }

        public int DeleteBank(Independiente.Model.Bank bank)
        {
            int result = 0;

            if (bank != null)
            {
                result = _catalogRepository.DeleteBank(CatalogMapper.ToDataModel(bank));
            }
            return result;
        }
    }
}
