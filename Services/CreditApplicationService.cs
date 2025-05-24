using Independiente.DataAccess;
using Independiente.DataAccess.Repositories;
using Independiente.Model;
using Independiente.Services.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.Services
{

    public interface ICreditApplicationService
    {
        int CountCreditApplications(CreditApplicationQuery query);

        int AddCreditApplication(Independiente.Model.CreditApplication creditApplication);

        List<Independiente.Model.CreditApplication> GetCreditApplications(CreditApplicationQuery query);

        Independiente.Model.CreditApplication GetCreditApplication(int creditApplicationId);

        Independiente.Model.File GetDocument(int clientId, string type);

        List<Independiente.Model.CreditPolicy> GetCreditPolicies(CreditPolicyQuery query);

        int SubmitDecision(Model.Report report);

        List<Model.AmortizationSchedule> GetAmortizationSchedule(Model.CreditApplication creditApplication);
    }
    public class CreditApplicationService : ICreditApplicationService
    {

        private readonly ICreditApplicationRepository _creditApplicationRepository;

        private readonly ICreditPolicyRepository _creditPolicyRepository;

        public CreditApplicationService(ICreditApplicationRepository creditApplicationRepository, ICreditPolicyRepository creditPolicyRepository)
        {
            _creditApplicationRepository = creditApplicationRepository;

            _creditPolicyRepository = creditPolicyRepository;
        }

        private bool ValidateQuery(CreditApplicationQuery query)
        {

            if ((query.ToDate != null && query.FromDate != null) && (query.FromDate > query.ToDate))
            {
                throw new ArgumentException("Rango de fecha invalido");
            }

            if (!string.IsNullOrEmpty(query.RFC))
            {
                FieldValidator.IsValidRFC(query.RFC);
            }

            return true;
        }

        public int AddCreditApplication(Independiente.Model.CreditApplication creditApplication)
        {
            int id = 0;

            if (creditApplication != null)
            {
                id = _creditApplicationRepository.AddCreditApplication(CreditApplicationMapper.ToDataModel(creditApplication));
            }

            return id;
        }

        public int CountCreditApplications(CreditApplicationQuery query)
        {
            int total = 0;

            if (ValidateQuery(query))
            {
                total = _creditApplicationRepository.CountCreditApplications(query);
            }


            return total;
        }

        public Independiente.Model.CreditApplication GetCreditApplication(int creditApplicationId)
        {
            Independiente.Model.CreditApplication creditApplication = null;

            if (creditApplicationId > 0)
            {
                var application = _creditApplicationRepository.GetCreditApplication(creditApplicationId);

                creditApplication = CreditApplicationMapper.ToViewModel(application);
            }

            return creditApplication;
        }

        public List<Independiente.Model.CreditApplication> GetCreditApplications(CreditApplicationQuery query)
        {
            List<CreditApplicationListView> creditApplicationList = new List<CreditApplicationListView>();

            List<Independiente.Model.CreditApplication> creditApplications1 = new List<Model.CreditApplication>();

            if (ValidateQuery(query))
            {
                creditApplicationList = _creditApplicationRepository.GetCreditApplications(query);


                foreach (var c in creditApplicationList)
                {
                    creditApplications1.Add(CreditApplicationMapper.ToViewModel(c));
                }

            }



            return creditApplications1;
        }


        public Independiente.Model.File GetDocument(int clientId, string type)
        {
            Independiente.Model.File file = null;

            if (clientId > 0 && !string.IsNullOrEmpty(type))
            {
                var documentationFile = _creditApplicationRepository.GetDocument(clientId, type);

                if (documentationFile.File1 == null)
                {
                    throw new KeyNotFoundException("Documento no encontrado");
                }

                file = FileMapper.ToViewModel(documentationFile);

            }

            return file;

        }

        public List<Independiente.Model.CreditPolicy> GetCreditPolicies(CreditPolicyQuery query)
        {
            List<Independiente.Model.CreditPolicy> creditPoliciesList = new List<Independiente.Model.CreditPolicy>();

            if (query != null && query.Status != null)
            {
                var creditPolicies = _creditPolicyRepository.GetCreditPolicies(query);

                foreach (var c in creditPolicies)
                {
                    creditPoliciesList.Add(CreditPolicyMapper.ToViewModel(c));
                }
            }

            return creditPoliciesList;


        }

        public int SubmitDecision(Model.Report report)
        {
            int result = 0;

            if (report != null)
            {
                result = _creditApplicationRepository.SubmitDecision(ReportMapper.ToDataModel(report));
            }

            return result;
        }

        public List<Model.AmortizationSchedule> GetAmortizationSchedule(Model.CreditApplication creditApplication)
        {
            var result = new List<Model.AmortizationSchedule>();

            if (!IsValidCreditApplication(creditApplication))
                return result;

            decimal totalAmountToPay = CalculateTotalAmountToPay(creditApplication);
            (int numberOfPayments, TimeSpan interval) = GetPaymentFrequencyDetails(
                creditApplication.PromotionalOffer.PaymenteFrecuency,
                creditApplication.PromotionalOffer.LoanTerm.Value
            );

            decimal fixedPayment = decimal.Round(totalAmountToPay / numberOfPayments, 2);
            DateTime startDate = creditApplication.LoanApplicationDate != default
                                 ? creditApplication.LoanApplicationDate
                                 : DateTime.Now;

            decimal remainingBalance = totalAmountToPay;

            for (int i = 1; i <= numberOfPayments; i++)
            {
                remainingBalance -= fixedPayment;
                if (remainingBalance < 0) remainingBalance = 0;

                result.Add(new Model.AmortizationSchedule
                {
                    PaymentNumber = i,
                    PaymentDate = startDate.AddDays(interval.TotalDays * (i - 1)),
                    FixedPayment = fixedPayment,
                    OutstandingBalance = decimal.Round(remainingBalance, 2),
                    Status = "Pending",
                    CreditApplication = creditApplication
                });
            }

            return result;
        }

        private bool IsValidCreditApplication(Model.CreditApplication creditApplication)
        {
            return creditApplication?.LoanAmount != null &&
                   creditApplication?.PromotionalOffer != null &&
                   creditApplication.PromotionalOffer.InteresRate != null &&
                   creditApplication.PromotionalOffer.LoanTerm != null &&
                   creditApplication.PromotionalOffer.IVA != null &&
                   !string.IsNullOrWhiteSpace(creditApplication.PromotionalOffer.PaymenteFrecuency);
        }
        private decimal CalculateTotalAmountToPay(Model.CreditApplication creditApplication)
        {
            decimal capital = creditApplication.LoanAmount.Value;
            decimal interesAnual = creditApplication.PromotionalOffer.InteresRate.Value;
            int plazoAnios = creditApplication.PromotionalOffer.LoanTerm.Value;
            decimal iva = creditApplication.PromotionalOffer.IVA.Value;

            decimal interesTotal = capital * interesAnual * plazoAnios;
            decimal interesConIVA = interesTotal * (1 + iva);

            return capital + interesConIVA;
        }

        private (int numberOfPayments, TimeSpan interval) GetPaymentFrequencyDetails(string paymentFrequency, int termInYears)
        {
            switch (paymentFrequency.ToLower())
            {
                case "quincenal":
                    return (termInYears * 24, TimeSpan.FromDays(15)); 
                case "mensual":
                default:
                    return (termInYears * 12, TimeSpan.FromDays(30)); 
            }
        }

    }
}
