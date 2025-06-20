using Independiente.DataAccess;
using Independiente.DataAccess.Repositories;
using Independiente.Model;
using Independiente.Services.Mappers;
using Org.BouncyCastle.Tls;
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

        Independiente.Model.CreditPolicy GetCreditPolicy(int creditPolicyId);

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
            if (!string.IsNullOrEmpty(query.Name))
            {
                FieldValidator.IsValidName(query.Name);
            }            
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
                try
                {
                    if (ValidateCreditPolicy(creditPolicy))
                    {
                        CreditPolicyQuery query = new CreditPolicyQuery();
                        query.Name = creditPolicy.Name;
                        if (CountCreditPolicies(query) == 0)
                        {
                            if (AreDatesValid(creditPolicy))
                            {
                                id = _creditPolicyRepository.AddCreditPolicy(CreditPolicyMapper.ToDataModel(creditPolicy));
                            }
                            else
                            {
                                throw new ArgumentException("Las fechas ingresadas no son válidas. La fecha de inicio debe ser igual o posterior a hoy, y la fecha de fin debe ser posterior a la de inicio.");
                            }
                        }
                        else
                        {
                            throw new ArgumentException("El nombre de la política de crédito ya está en uso. Verifique e ingrese un nombre diferente.");
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

        public int UpdateCreditPolicy(Independiente.Model.CreditPolicy creditPolicy)
        {
            int affectedRows = 0;

            if (creditPolicy != null)
            {
                try
                {
                    if (ValidateCreditPolicy(creditPolicy))
                    {
                        var matches = _creditPolicyRepository.GetCreditPolicyByName(creditPolicy.Name);
                        bool isDuplicateNameUsedByAnother = matches.Any(p => p.CreditPolicyId != creditPolicy.CreditPolicyId);
                        if (!isDuplicateNameUsedByAnother)
                        {
                            if (AreDatesValid(creditPolicy))
                            {
                                affectedRows = _creditPolicyRepository.UpdateCreditPolicy(CreditPolicyMapper.ToDataModel(creditPolicy));
                            }
                            else
                            {
                                throw new ArgumentException("Las fechas ingresadas no son válidas. La fecha de inicio debe ser igual o posterior a hoy, y la fecha de fin debe ser posterior a la de inicio.");
                            }
                        }
                        else
                        {
                            throw new ArgumentException("El nombre de la política de crédito ya está en uso. Verifique e ingrese un nombre diferente.");
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

        public int DeleteCreditPolicy(Independiente.Model.CreditPolicy creditPolicy)
        {
            int result = 0;

            if (creditPolicy != null)
            {
                result = _creditPolicyRepository.DeleteCreditPolicy(CreditPolicyMapper.ToDataModel(creditPolicy));
            }
            return result;
        }

        private bool AreDatesValid(Independiente.Model.CreditPolicy creditPolicy)
        {
            DateTime? startDate = creditPolicy.RegistrationDate;
            DateTime? endDate = creditPolicy.EndDate;
         
            if (!startDate.HasValue || !endDate.HasValue)
                return false;

            DateTime today = DateTime.Today;            
            if (startDate.Value.Date < today)
                return false;

            if (endDate.Value.Date <= startDate.Value.Date)
                return false;

            return true;
        }

        private bool ValidateCreditPolicy(Independiente.Model.CreditPolicy creditPolicy)
        {
            return FieldValidator.IsValidName(creditPolicy.Name) &&
                   FieldValidator.IsValidDescription(creditPolicy.Description);
        }
    }
}