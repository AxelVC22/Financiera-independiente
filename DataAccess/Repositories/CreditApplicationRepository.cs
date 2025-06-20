using Independiente.Model;
using Independiente.Services.Mappers;
using Independiente.View;
using Independiente.View.Pages;
using iTextSharp.text;
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
using System.Windows;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace Independiente.DataAccess.Repositories
{

    public interface IQueryObject<T>
    {
        Expression<Func<T, bool>> BuildExpression();
    }

    public class CreditApplicationQuery : INotifyPropertyChanged, IQueryObject<CreditApplicationListView>
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        private string _rfc;

        private string _status;

        private DateTime? _fromDate;

        private DateTime? _toDate;

        public Expression<Func<CreditApplicationListView, bool>> BuildExpression()
        {
            return c =>
                (string.IsNullOrEmpty(RFC) || c.RFC == RFC) &&
                (!FromDate.HasValue || c.LoanApplicationDate >= FromDate) &&
                (!ToDate.HasValue || c.LoanApplicationDate <= ToDate) &&
                (string.IsNullOrEmpty(Status) || c.Status == Status);
        }

        public string RFC
        {
            get => _rfc;
            set
            {
                if (_rfc != value)
                {
                    _rfc = value;
                    OnPropertyChanged(nameof(RFC));
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

        public DateTime? FromDate
        {
            get => _fromDate;
            set
            {
                if (_fromDate != value)
                {
                    _fromDate = value;
                    OnPropertyChanged(nameof(FromDate));
                }
            }
        }

        public DateTime? ToDate
        {
            get => _toDate;
            set
            {
                if (_toDate != value)
                {
                    _toDate = value;
                    OnPropertyChanged(nameof(ToDate));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public interface ICreditApplicationRepository
    {
        int CountCreditApplications(CreditApplicationQuery query);

        int AddCreditApplication(CreditApplication creditApplication);

        List<CreditApplicationListView> GetCreditApplications(CreditApplicationQuery query);

        CreditApplication GetCreditApplication(int creditApplicationId);

        CreditApplication GetPendingCreditApplicationByClientId(int clientId);

        int SubmitDecision(Report report, List<AmortizationSchedule> amortizationSchedules);
        File GetDocument(int clientId, string type);

        Report GetReport(int creditApplicationId);
        int AddCreditApplicationWithFiles(Model.CreditApplication creditApplication, List<Model.File> additionalFiles);
        List<DataAccess.File> GetClientFiles(int clientId);
        bool DeleteClientFile(int fileId);
    }


    public class CreditApplicationRepository : ICreditApplicationRepository
    {

        private bool Validate(CreditApplicationQuery query)
        {
           
            if (query.FromDate.HasValue && query.ToDate.HasValue && query.FromDate > query.ToDate)
                throw new ArgumentException("La fecha inicial no puede ser posterior a la fecha final.");

            return true;
        }

        public int CountCreditApplications(CreditApplicationQuery query)
        {
            int total = 0;

            Validate(query);

            var predicate = query.BuildExpression();

            try
            {
                using (var context = new IndependienteEntities())
                {

                    total = context.CreditApplicationListView.Count(predicate);
                }
            }
            catch (Exception ex)
            {
                throw DbExceptionHandler.Handle(ex);
            }

            return total;
        }

        public int AddCreditApplication(CreditApplication creditApplication)
        {
            int id = 0;

            try
            {
                using (var context = new IndependienteEntities())
                {
                    var newCreditApplication = new CreditApplication
                    {
                        LoanAmount = creditApplication.LoanAmount,
                        LoanApplicationDate = creditApplication.LoanApplicationDate,
                        Status = creditApplication.Status,
                        ClientId = creditApplication.ClientId,
                        PromotionalOfferId = creditApplication.PromotionalOfferId,
                        File = new DataAccess.File
                        {
                            ClientId = creditApplication.ClientId,
                            Type = creditApplication.File.Type,
                            File1 = creditApplication.File.File1,
                        }

                    };
                    context.CreditApplication.Add(newCreditApplication);
                    context.SaveChanges();
                    id = newCreditApplication.CreditApplicationId;
                }
            }
            catch (Exception ex)
            {
                throw DbExceptionHandler.Handle(ex);
            }

            return id;
        }

        public CreditApplication GetCreditApplication(int creditApplicationId)
        {
            CreditApplication creditApplication = new CreditApplication();

            try
            {
                using (var context = new IndependienteEntities())
                {
                    var creditApplicationForSearch = context.CreditApplication.Find(creditApplicationId);

                    if (creditApplicationForSearch != null)
                    {
                        creditApplication = creditApplicationForSearch;
                        creditApplication.Client = creditApplicationForSearch.Client;
                        creditApplication.Client.PersonalData = creditApplicationForSearch.Client.PersonalData;
                        creditApplication.Client.AddressData = creditApplicationForSearch.Client.AddressData;

                        creditApplication.File = creditApplicationForSearch.File;
                        creditApplication.Client.Employee = creditApplicationForSearch.Client.Employee;
                        creditApplication.PromotionalOffer = creditApplicationForSearch.PromotionalOffer;
                    }
                }
            }
            catch (Exception ex)
            {
                throw DbExceptionHandler.Handle(ex);
            }

            return creditApplication;
        }

        public List<CreditApplicationListView> GetCreditApplications(CreditApplicationQuery query)
        {

            List<CreditApplicationListView> creditApplication = new List<CreditApplicationListView>();

            var predicate = query.BuildExpression();

            try
            {
                using (var context = new IndependienteEntities())
                {
                    var creditApplicationsForSearch = context.CreditApplicationListView
                        .Where(predicate)
                         .OrderBy(x => x.LoanApplicationDate)
                         .Skip((query.PageNumber - 1) * query.PageSize)
                         .Take(query.PageSize)
                        .ToList();

                    if (creditApplicationsForSearch != null)
                    {
                        creditApplication = creditApplicationsForSearch;
                    }
                }
            }
            catch (Exception ex)
            {
                throw DbExceptionHandler.Handle(ex);
            }

            return creditApplication;
        }

        public CreditApplication GetPendingCreditApplicationByClientId(int clientId)
        {
            try
            {
                using (var context = new IndependienteEntities())
                {
                    // Buscar la solicitud pendiente más reciente del cliente
                    var creditApplication = context.CreditApplication
                        .Where(ca => ca.ClientId == clientId && ca.Status == "Pending")
                        .OrderByDescending(ca => ca.LoanApplicationDate)
                        .FirstOrDefault();

                    if (creditApplication == null)
                        throw new KeyNotFoundException("No se encontró una solicitud pendiente para el cliente.");

                    // Cargar datos relacionados
                    creditApplication.Client = creditApplication.Client;
                    creditApplication.Client.PersonalData = creditApplication.Client.PersonalData;
                    creditApplication.Client.AddressData = creditApplication.Client.AddressData;
                    creditApplication.Client.Employee = creditApplication.Client.Employee;
                    creditApplication.File = creditApplication.File;
                    creditApplication.PromotionalOffer = creditApplication.PromotionalOffer;

                    return creditApplication;
                }
            }
            catch (Exception ex)
            {
                throw DbExceptionHandler.Handle(ex);
            }
        }


        private CreditApplicationStates ParseCreditApplicationStatus(string status)
        {
            return Enum.TryParse(status, out CreditApplicationStates result)
                ? result
                : CreditApplicationStates.Pending;
        }


        public int AddCreditApplicationWithFiles(Model.CreditApplication creditApplication, List<Model.File> additionalFiles)
        {
            int id = 0;

            using (var context = new IndependienteEntities())
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var savedFileIds = new List<int>();

                    if (additionalFiles != null && additionalFiles.Any())
                    {
                        foreach (var file in additionalFiles.Where(f => f.FileType.ToString() != "CA"))
                        {
                            var entityFile = new DataAccess.File
                            {
                                ClientId = creditApplication.Client.ClientId,
                                Type = file.FileType.ToString(),
                                File1 = file.FileContent
                            };
                            context.File.Add(entityFile);
                            context.SaveChanges();
                            savedFileIds.Add(entityFile.FileId);
                        }
                    }

                    var mainFile = additionalFiles?.FirstOrDefault(f => f.FileType.ToString() == "CA");
                    int mainFileId = 0;

                    if (mainFile != null)
                    {
                        var entityMainFile = new DataAccess.File
                        {
                            ClientId = creditApplication.Client.ClientId,
                            Type = mainFile.FileType.ToString(),
                            File1 = mainFile.FileContent
                        };
                        context.File.Add(entityMainFile);
                        context.SaveChanges();
                        mainFileId = entityMainFile.FileId;
                    }

                    var newCreditApplication = new DataAccess.CreditApplication
                    {
                        LoanAmount = creditApplication.LoanAmount ?? 0,
                        LoanApplicationDate = creditApplication.LoanApplicationDate,
                        Status = creditApplication.Status.ToString(),
                        ClientId = creditApplication.Client.ClientId,
                        PromotionalOfferId = creditApplication.PromotionalOffer.PromotionalOfferId,
                        FileId = mainFileId
                    };

                    context.CreditApplication.Add(newCreditApplication);
                    context.SaveChanges();

                    id = newCreditApplication.CreditApplicationId;
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw DbExceptionHandler.Handle(ex);
                }
            }

            return id;
        }

        public int AddCreditApplication(Model.CreditApplication creditApplication)
        {
            int id = 0;

            try
            {
                using (var context = new IndependienteEntities())
                {
                    int? fileId = null;
                    if (creditApplication.File != null)
                    {
                        var file = new DataAccess.File
                        {
                            ClientId = creditApplication.Client.ClientId,
                            Type = creditApplication.File.FileType.ToString(),
                            File1 = creditApplication.File.FileContent,
                        };

                        context.File.Add(file);
                        context.SaveChanges();
                        fileId = file.FileId;
                    }

                    var newCreditApplication = new DataAccess.CreditApplication
                    {
                        LoanAmount = (decimal)creditApplication.LoanAmount,
                        LoanApplicationDate = creditApplication.LoanApplicationDate,
                        Status = creditApplication.Status.ToString(),
                        ClientId = creditApplication.Client.ClientId,
                        PromotionalOfferId = creditApplication.PromotionalOffer.PromotionalOfferId,
                        FileId = (int)fileId
                    };

                    context.CreditApplication.Add(newCreditApplication);
                    context.SaveChanges();
                    id = newCreditApplication.CreditApplicationId;
                }
            }
            catch (Exception ex)
            {
                throw DbExceptionHandler.Handle(ex);
            }

            return id;
        }

        public List<DataAccess.File> GetClientFiles(int clientId)
        {
            var files = new List<DataAccess.File>();

            try
            {
                using (var context = new IndependienteEntities())
                {
                    files = context.File.Where(f => f.ClientId == clientId).ToList();
                }
            }
            catch (Exception ex)
            {
                throw DbExceptionHandler.Handle(ex);
            }

            return files;
        }

        public bool DeleteClientFile(int fileId)
        {
            try
            {
                using (var context = new IndependienteEntities())
                {
                    var file = context.File.Find(fileId);
                    if (file != null)
                    {
                        context.File.Remove(file);
                        context.SaveChanges();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw DbExceptionHandler.Handle(ex);
            }

            return false;
        }

        public File GetDocument(int clientId, string type)
        {
            File documentationFile = new File();

            try
            {
                using (var context = new IndependienteEntities())
                {
                    var file = context.File.FirstOrDefault(f => f.ClientId == clientId && f.Type == type);

                    if (file != null)
                    {
                        documentationFile = file;
                    }

                }
            }
            catch (Exception ex)
            {
                throw DbExceptionHandler.Handle(ex);
            }

            return documentationFile;
        }

        public int SubmitDecision(Report report, List<AmortizationSchedule> amortizationSchedules)
        {
            int result = 0;

            using (var context = new IndependienteEntities())
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var creditApplication = context.CreditApplication.Find(report.CreditApplicationId);

                    if (creditApplication != null)
                    {
                        if (creditApplication.Status != CreditApplicationStates.Pending.ToString())
                        {
                            throw new InvalidOperationException("El estado de la solicitud no es válido para enviar el dictamen.");
                        }

                        creditApplication.Status = report.CreditApplication.Status;

                        report.CreditApplication = null;

                        if (Equals(creditApplication.Status, CreditApplicationStates.Rejected.ToString()))
                        {
                            var attachedPolicies = new List<CreditPolicy>();

                            foreach (var policy in report.CreditPolicy)
                            {
                                var attached = new CreditPolicy { CreditPolicyId = policy.CreditPolicyId };
                                context.CreditPolicy.Attach(attached);
                                attachedPolicies.Add(attached);
                            }

                            report.CreditPolicy = attachedPolicies;
                        }
                        else if (Equals(creditApplication.Status, CreditApplicationStates.Accepted.ToString()))
                        {

                            foreach (var schedule in amortizationSchedules)
                            {
                                context.AmortizationSchedule.Add(schedule);
                            }

                        }

                        context.Report.Add(report);

                        result = context.SaveChanges();
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw DbExceptionHandler.Handle(ex);
                }
            }

            return result;
        }

        public Report GetReport(int creditApplicationId)
        {
            Report result = new Report { };

            using (var context = new IndependienteEntities())

            {
                try
                {
                    var report = context.Report.FirstOrDefault(r => r.CreditApplicationId == creditApplicationId);

                    if (report == null)
                    {
                        throw new ArgumentException("No se encontró ningún reporte para esa solicitud.");
                    }

                    var creditApplication = report.CreditApplication;


                    if (creditApplication != null)
                    {

                        var creditPolicies = report.CreditPolicy;

                        report.CreditApplication = null;
                        result = report;
                    }
                }
                catch (Exception ex)
                {
                    throw DbExceptionHandler.Handle(ex);
                }
            }

            return result;
        }




    }
}
