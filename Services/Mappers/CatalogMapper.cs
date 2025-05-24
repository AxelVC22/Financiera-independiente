using Independiente.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.Services.Mappers
{
    public static class CatalogMapper
    {
        public static DataAccess.Bank ToDataModel(this Model.Bank source)
        {
            DataAccess.Bank bank = new DataAccess.Bank();

            if (source != null)
            {
                bank = new DataAccess.Bank
                {
                    BankId = source.BankId,
                    Name = source.Name,
                    Status = source.Status.ToString(),
                };
            }
            return bank;
        }

        public static Model.Bank ToViewModel(this DataAccess.Bank source)
        {
            Model.Bank bank = new Model.Bank();

            if (source != null)
            {
                bank = new Model.Bank
                {
                    BankId = source.BankId,
                    Name = source.Name,
                    Status = (BankStates)Enum.Parse(typeof(BankStates), source.Status)
                };
            }
            return bank;
        }
    }
}
