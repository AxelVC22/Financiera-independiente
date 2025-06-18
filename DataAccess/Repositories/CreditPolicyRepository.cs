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

    public class CreditPolicyQuery : INotifyPropertyChanged, IQueryObject<CreditPolicy>
    {
        private string _name;

        private string _status;

        private bool? _validity;

        public int PageSize { get; set; } = 10;

        public int PageCount { get; set; } = 1;

        public int PageNumber { get; set; }

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

        public bool? Validity
        {
            get => _validity;
            set
            {
                if (_validity != value)
                {
                    _validity = value;
                    OnPropertyChanged(nameof(Validity));
                }
            }
        }
        public Expression<Func<CreditPolicy, bool>> BuildExpression()
        {
            DateTime now = DateTime.Now;
            return c =>
                (string.IsNullOrEmpty(Name) || c.Name.Contains(Name)) &&
                (string.IsNullOrEmpty(Status) || c.Status == Status) &&
                (!Validity.HasValue ||
                (Validity.Value
                ? (c.RegistrationDate <= now && c.EndDate >= now)
                : (c.EndDate < now || c.RegistrationDate > now)));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }        

      
    }

    public interface ICreditPolicyRepository
    {
        int CountCreditPolicies(CreditPolicyQuery query);

        List<CreditPolicy> GetCreditPolicies(CreditPolicyQuery query);

        CreditPolicy GetCreditPolicy(int creditPolicyId);

        int AddCreditPolicy(CreditPolicy creditPolicy);

        int UpdateCreditPolicy(CreditPolicy creditPolicy);

        int DeleteCreditPolicy(CreditPolicy creditPolicy);
    }
    public class CreditPolicyRepository : ICreditPolicyRepository
    {
        public int CountCreditPolicies(CreditPolicyQuery query)
        {
            int total = 0;
            var predicate = query.BuildExpression();

            try
            {
                using (var context = new IndependienteEntities())
                {

                    total = context.CreditPolicy.Count(predicate);
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

        public List<CreditPolicy> GetCreditPolicies(CreditPolicyQuery query)
        {
            List<CreditPolicy> creditPolicies = new List<CreditPolicy>();

            var predicate = query.BuildExpression();

            try
            {
                using (var context = new IndependienteEntities())
                {
                    var creditPolicyForSearch = context.CreditPolicy
                        .Where(predicate)
                        .OrderBy(x => x.RegistrationDate)
                        .Skip((query.PageNumber - 1) * query.PageSize)
                        .Take(query.PageSize)
                        .ToList();

                    if (creditPolicyForSearch != null)
                    {
                        creditPolicies = creditPolicyForSearch;
                    }
                }
            }
            catch (DbUpdateException ex)
            {
            }
            catch (EntityException ex)
            {
            }

            return creditPolicies;
        }

        public CreditPolicy GetCreditPolicy(int creditPolicyId)
        {
            CreditPolicy creditPolicy = new CreditPolicy();

            try
            {
                using (var context = new IndependienteEntities())
                {
                    var creditPolicyForSearch = context.CreditPolicy.Find(creditPolicyId);

                    if (creditPolicyForSearch != null)
                    {
                        creditPolicy = creditPolicyForSearch;
                    }
                }
            }
            catch (DbUpdateException ex)
            {
            }
            catch (EntityException ex)
            {
            }

            return creditPolicy;
        }

        public int AddCreditPolicy(CreditPolicy creditPolicy)
        {
            int id = 0;

            try
            {
                using (var context = new IndependienteEntities())
                {
                    var newCreditPolicy = new CreditPolicy()
                    {                                                
                        Name = creditPolicy.Name,
                        Description = creditPolicy.Description,
                        RegistrationDate = creditPolicy.RegistrationDate,
                        EndDate = creditPolicy.EndDate,
                        Status = creditPolicy.Status

                    };
                    context.CreditPolicy.Add(newCreditPolicy);
                    context.SaveChanges();
                    id = newCreditPolicy.CreditPolicyId;
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

        public int UpdateCreditPolicy(CreditPolicy creditPolicy)
        {
            int affectedRows = 0;

            try
            {
                using (var context = new IndependienteEntities())
                {
                    var existingPolicy = context.CreditPolicy.Find(creditPolicy.CreditPolicyId);

                    if (existingPolicy != null)
                    {
                        existingPolicy.Name = creditPolicy.Name;
                        existingPolicy.Description = creditPolicy.Description;
                        existingPolicy.RegistrationDate = creditPolicy.RegistrationDate;
                        existingPolicy.EndDate = creditPolicy.EndDate;
                        existingPolicy.Status = creditPolicy.Status;

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

        public int DeleteCreditPolicy(CreditPolicy creditPolicy)
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
                            var creditPolicyForDelete = context.CreditPolicy.Find(creditPolicy.CreditPolicyId);

                            if (creditPolicyForDelete != null)
                            {
                                context.CreditPolicy.Remove(creditPolicyForDelete);

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