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
                    CreditApplication = CreditApplicationMapper.ToDataModelWithoutClient(source.CreditApplication),
                    CreditPolicy = creditPolicies
                }; 
            }

            return report;
        }

        public static Model.Report ToViewModel(this DataAccess.Report source)
        {
            Model.Report report = new Model.Report();

            if (source != null)
            {
                return new Model.Report
                {
                    CreditApplication = CreditApplicationMapper.ToViewModel(source.CreditApplication),
                    ReviewingDate = source.ReviewingDate,
                    Notes = source.Notes,
                    CreditPolicies = source.CreditPolicy.Select(c => CreditPolicyMapper.ToViewModel(c)).ToList()
                };
            }

            return report;
        }
    }
}
