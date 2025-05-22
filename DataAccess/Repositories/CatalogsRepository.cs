using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.DataAccess.Repositories
{
    public interface ICatalogsRepository
    {
        int AddBank(Bank bank);

        int DeleteBank(Bank bank);
    }

    public class CatalogsRepository : ICatalogsRepository
    {
        public int AddBank(Bank bank)
        {
            int id = 0;

            try
            {
                using (var context = new IndependienteEntities())
                {
                    var newBank = new Bank
                    {
                        Name = bank.Name,
                        Status = bank.Status
                    };
                    context.Bank.Add(newBank);
                    context.SaveChanges();
                    id = newBank.BankId;
                }
            }
            catch (DbUpdateException ex)
            {
               
            }
            catch(EntityException ex)
            {

            }

            return id;
        }


        //Only for tests
        public int DeleteBank(Bank bank)
        {
            int result = 0;

            try
            {
                using (var context = new IndependienteEntities())
                {
                    var bankForRemove = context.Bank.Find(bank.BankId);
                    if (bankForRemove != null)
                    {
                        context.Bank.Remove(bankForRemove);
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
