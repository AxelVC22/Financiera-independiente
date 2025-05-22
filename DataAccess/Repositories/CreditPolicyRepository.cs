using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.DataAccess.Repositories
{

    public class CreditPolicyQuery : INotifyPropertyChanged, IQueryObject<CreditApplicationListView>
    {
        public string _status;

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
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Expression<Func<CreditApplicationListView, bool>> BuildExpression()
        {
            throw new NotImplementedException();
        }
    }

    public interface ICreditPolicyRepository
    {
        List<CreditPolicy> GetCreditPolicies(CreditPolicyQuery query);
    }
    public class CreditPolicyRepository : ICreditPolicyRepository
    {
        public List<CreditPolicy> GetCreditPolicies(CreditPolicyQuery query)
        {
            List<CreditPolicy> creditPolicies = new List<CreditPolicy>();

            try
            {
                using (var context = new IndependienteEntities())
                {
                    var creditApplicationForSearch = context.CreditPolicy.Where(c => c.Status == query.Status)
                        .ToList();

                    if (creditApplicationForSearch != null)
                    {
                        creditPolicies = creditApplicationForSearch;
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
    }
}
