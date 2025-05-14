using Independiente.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.Services.Mappers
{
    public static class ReportMapper
    {
        public static DataAccess.Report ToDataModel(this Model.Report source)
        {
            DataAccess.Report report = new DataAccess.Report();

            if (source != null)
            {

                List<CreditPolicy> creditPolicies = new List<CreditPolicy>();

                foreach (var creditPolicy in source.CreditPolicies)
                {
                    creditPolicies.Add(CreditPolicyMapper.ToDataModel(creditPolicy));
                }

                report = new DataAccess.Report
                {
                    CreditApplicationId = source.CreditApplication.CreditApplicationId,
                    ReviewingDate = source.ReviewingDate,
                    Notes = source.Notes,
                    CreditApplication = CreditApplicationMapper.ToDataModel(source.CreditApplication),
                    CreditPolicy = creditPolicies
                }; 
            }

            return report;
        }

        public static Model.Client ToViewModel(this DataAccess.Client source)
        {
            Model.Client client = new Model.Client();

            if (source != null)
            {
                return new Model.Client
                {
                    ClientId = source.ClientId,
                    PersonalData = PersonalDataMapper.ToViewModel(source.PersonalData),
                    AddressData = AddressDataMapper.ToViewModel(source.AddressData),
                };
            }

            return client;
        }
    }
}
