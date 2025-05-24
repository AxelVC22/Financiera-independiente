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
    public class CatalogQuery : INotifyPropertyChanged, IQueryObject<Bank>
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        private string _name;

        private string _status;
        
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
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

        public Expression<Func<Bank, bool>> BuildExpression()
        {
            return c =>
                string.IsNullOrEmpty(Name) || c.Name.Contains(Name) &&
                (string.IsNullOrEmpty(Status) || c.Status == Status);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public interface ICatalogRepository
    {
        int CountBanks(CatalogQuery query);

        List<Bank> GetBanks(CatalogQuery query);

        Bank GetBank(int bankId);

        int AddBank(Bank bank);

        int UpdateBank(Bank bank);

        int DeleteBank(Bank bank);
    }

    public class CatalogRepository : ICatalogRepository
    {
        public int CountBanks(CatalogQuery query)
        {
            int total = 0;
            var predicate = query.BuildExpression();

            try
            {
                using (var context = new IndependienteEntities())
                {

                    total = context.Bank.Count(predicate);
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
        
        public List<Bank> GetBanks(CatalogQuery query)
        {
            List<Bank> banks = new List<Bank>();

            var predicate = query.BuildExpression();

            try
            {
                using (var context = new IndependienteEntities())
                {
                    IQueryable<Bank> bankQuery = context.Bank
                        .Where(predicate)
                        .OrderBy(x => x.Name);

                    if (query.PageNumber > 0 && query.PageSize > 0)
                    {
                        bankQuery = bankQuery
                            .Skip((query.PageNumber - 1) * query.PageSize)
                            .Take(query.PageSize);
                    }

                    var bankForSearch = bankQuery.ToList();

                    if (bankForSearch != null)
                    {
                        banks = bankForSearch;
                    }
                }
            }
            catch (DbUpdateException ex)
            {
            }
            catch (EntityException ex)
            {
            }

            return banks;
        }

        public Bank GetBank(int bankId)
        {
            Bank bank = new Bank();

            try
            {
                using (var context = new IndependienteEntities())
                {
                    var bankForSearch = context.Bank.Find(bankId);

                    if (bankForSearch != null)
                    {
                        bank = bankForSearch;
                    }
                }
            }
            catch (DbUpdateException ex)
            {
            }
            catch (EntityException ex)
            {
            }

            return bank;
        }

        public int AddBank(Bank bank)
        {
            int id = 0;

            try
            {
                using (var context = new IndependienteEntities())
                {
                    var newBank = new Bank()
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
            catch (EntityException ex)
            {
            }
            return id;
        }

        public int UpdateBank(Bank bank)
        {
            int affectedRows = 0;

            try
            {
                using (var context = new IndependienteEntities())
                {
                    var existingBank = context.Bank.Find(bank.BankId);

                    if (existingBank != null)
                    {
                        existingBank.Name = bank.Name;                        
                        existingBank.Status = bank.Status;

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

        public int DeleteBank(Bank bank)
        {
            int result = 0;

            try
            {
                using (var context = new IndependienteEntities())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var bankForDelete = context.Bank.Find(bank.BankId);

                            if (bankForDelete != null)
                            {
                                context.Bank.Remove(bankForDelete);

                                result = context.SaveChanges();

                                transaction.Commit();
                            }

                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (DbUpdateException ex)
            {

            }
            catch (EntityException ex)
            {
            }
            return result;
        }
    }
}