using Independiente.DataAccess;
using Independiente.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.Services
{
    public interface ICatalogService
    {
        List<Model.Bank> GetBanks();

    }

    public class CatalogManagerService : ICatalogService
    {
        public List<Model.Bank> GetBanks()
        {
            using (var context = new IndependienteEntities()) {
                var banks = context.Bank.Select(bank => new Model.Bank
                {
                    BankId = bank.BankId,
                    Name = bank.Name,
                }).ToList();
                return banks;
            }
        }
    }
}
