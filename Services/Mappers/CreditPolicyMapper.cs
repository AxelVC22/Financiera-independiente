using Independiente.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.Services.Mappers
{
    public static class CreditPolicyMapper
    {
        public static DataAccess.CreditPolicy ToDataModel(this Model.CreditPolicy source)
        {
            DataAccess.CreditPolicy creditPolicy = new DataAccess.CreditPolicy();

            if (source != null)
            {
                creditPolicy = new DataAccess.CreditPolicy
                {
                    CreditPolicyId = source.CredditPolicyId,
                    Description = source.Description,
                    EndDate = source.EndDate,
                    RegistrationDate = source.RegistrationDate,
                    Name = source.Name,
                    Status = source.Status.ToString(),
                };
            }

            return creditPolicy;
        }



        public static Model.CreditPolicy ToViewModel(this DataAccess.CreditPolicy source)
        {
            Model.CreditPolicy creditPolicy = new Model.CreditPolicy();

            if (source != null)
            {
                creditPolicy = new Model.CreditPolicy
                {
                    Description= source.Description,
                    RegistrationDate= source.RegistrationDate,
                    EndDate= source.EndDate,
                    Name = source.Name,
                    CredditPolicyId = source.CreditPolicyId,
                    IsEditable = false,
                    Status  = (CreditPolicyStates)Enum.Parse(typeof(CreditPolicyStates), source.Status)
                };
            }

            return creditPolicy;
        }
    }
}