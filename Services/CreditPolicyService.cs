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
    public interface ICreditPolicyService
    {
        int CountCreditPolicies(CreditPolicyQuery query);

        List<Independiente.Model.CreditPolicy> GetCreditPolicies(CreditPolicyQuery query);

        Independiente.Model.CreditPolicy GetCreditPolicy(int CreditPolicyId);

        int AddCreditPolicy(Independiente.Model.CreditPolicy creditPolicy);

        int UpdateCreditPolicy(Independiente.Model.CreditPolicy creditPolicy);

        int DeleteCreditPolicy(Independiente.Model.CreditPolicy creditPolicy);
    }
    public class CreditPolicyService : ICreditPolicyService
    {
        private readonly ICreditPolicyRepository _creditPolicyRepository;

        public CreditPolicyService(ICreditPolicyRepository creditPolicyRepository)
        {
            _creditPolicyRepository = creditPolicyRepository;
        }
        private bool ValidateQuery(CreditPolicyQuery query)
        {
            FieldValidator.IsValidName(query.Name);
            return true;
        }
        public int CountCreditPolicies(CreditPolicyQuery query)
        {
            int total = 0;
            if (ValidateQuery(query))
            {
                total = _creditPolicyRepository.CountCreditPolicies(query);
            }
            return total;
        }

        public List<Independiente.Model.CreditPolicy> GetCreditPolicies(CreditPolicyQuery query)
        {
            List<Independiente.DataAccess.CreditPolicy> creditPoliciesList = new List<Independiente.DataAccess.CreditPolicy>();

            List<Independiente.Model.CreditPolicy> creditPolicies1 = new List<Model.CreditPolicy>();

            if (ValidateQuery(query))
            {
                creditPoliciesList = _creditPolicyRepository.GetCreditPolicies(query);

                foreach (var c in creditPoliciesList)
                {
                    creditPolicies1.Add(CreditPolicyMapper.ToViewModel(c));
                }
            }
            return creditPolicies1;
        }

        public Independiente.Model.CreditPolicy GetCreditPolicy(int creditPolicyId)
        {
            Independiente.Model.CreditPolicy creditPolicy = null;

            if (creditPolicyId > 0)
            {
                var application = _creditPolicyRepository.GetCreditPolicy(creditPolicyId);
                creditPolicy = CreditPolicyMapper.ToViewModel(application);
            }
            return creditPolicy;
        }

        public int AddCreditPolicy(Independiente.Model.CreditPolicy creditPolicy)
        {
            int id = 0;

            if (creditPolicy != null)
            {
                id = _creditPolicyRepository.AddCreditPolicy(CreditPolicyMapper.ToDataModel(creditPolicy));

            }
            return id;
        }

        public int UpdateCreditPolicy(Independiente.Model.CreditPolicy creditPolicy)
        {
            int affectedRows = 0;

            if (creditPolicy != null)
            {
                affectedRows = _creditPolicyRepository.UpdateCreditPolicy(CreditPolicyMapper.ToDataModel(creditPolicy));
            }
            return affectedRows;
        }

        public int DeleteCreditPolicy(Independiente.Model.CreditPolicy creditPolicy)
        {
            int result = 0;

            if (creditPolicy != null)
            {
                result = _creditPolicyRepository.DeleteCreditPolicy(CreditPolicyMapper.ToDataModel(creditPolicy));
            }
            return result;
        }
    }
}