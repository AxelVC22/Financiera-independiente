using Independiente.DataAccess;
using Independiente.DataAccess.Repositories;
using Independiente.Model;
using Independiente.Services;
using Independiente.View;
using Independiente.View.Pages;
using Independiente.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Client = Independiente.Model.Client;
using CreditApplication = Independiente.Model.CreditApplication;
using Employee = Independiente.Model.Employee;

namespace Independiente
{

    public partial class MainWindow : Window
    {

        public ICreditApplicationService CreditApplicationService {  get; set; }

        public IFilePickerService FilePickerService { get; set; }

        public IPaymentService PaymentService { get; set; }

        public IAmortizationScheduleService AmortizationScheduleService { get; set; }
        public INavigationService NavigationService { get;  set; }

        public ICreditApplicationRepository CreditApplicationRepository { get; set; }

        public IAmortizationScheduleRepository AmortizationScheduleRepository { get; set; }

        public IPaymentRepository PaymentRepository { get; set; }

        public ICreditPolicyRepository CreditPolicyRepository { get; set; }
        public ICreditApplicationGeneratorService CreditApplicationGeneratorService { get; set; }
        public IClientManagementService ClientManagementService { get; private set; }
        public ICatalogService CatalogService { get; private set; }
        public IPromotionalOfferService PromotionalOfferService { get; set; }
        public IPromotionalOfferRepository PromotionalOfferRepository { get; set; }

        public Model.Client Client { get; private set; }

        public ICreditPolicyService CreditPolicyService { get; private set; }

        public ICatalogRepository CatalogRepository { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            CatalogRepository catalogRepository = new CatalogRepository();

            IDialogService dialogService = new DialogService();
            ClientManagementService = new ClientManagerService();
            CatalogService = new CatalogService(catalogRepository);
            FilePickerService = new FilePickerService();
            Client = new Model.Client();

            CreditPolicyRepository = new CreditPolicyRepository();
            PaymentRepository = new PaymentRepository();
            AmortizationScheduleRepository = new AmortizationScheduleRepository();
            CreditApplicationRepository = new CreditApplicationRepository();
            CreditApplicationService = new CreditApplicationService(CreditApplicationRepository, CreditPolicyRepository);
            AmortizationScheduleService = new AmortizationScheduleService(AmortizationScheduleRepository);
            PaymentService = new PaymentService(PaymentRepository);
            CreditPolicyService = new CreditPolicyService(CreditPolicyRepository);
            PromotionalOfferRepository = new PromotionalOfferRepository();
            PromotionalOfferService = new PromotionalOfferService(PromotionalOfferRepository);
            CreditApplicationGeneratorService = new CreditApplicationGeneratorService();

            NavigationService = new FrameNavigationService(
                PageFrame,
                (viewModelType, parameter) =>
                {
                    if (viewModelType == typeof(PersonalDataViewModel))
                    {
                        var param = parameter as PersonDataParams ?? new PersonDataParams(PageMode.Registration, RegistrationType.Client, Client);

                        var viewModel = new PersonalDataViewModel(
                            dialogService, NavigationService, param.Mode, param.RegistrationType, param.Person, ClientManagementService
                        );

                        return new View.Pages.PersonalData(viewModel);
                    }
                    else if (viewModelType == typeof(FinancialDataViewModel))
                    {
                        var param = parameter as ClientDataParams ?? new ClientDataParams(PageMode.Registration, RegistrationType.Client, Client);

                        var viewModel = new FinancialDataViewModel(
                            dialogService, NavigationService, param.Mode, ClientManagementService, CatalogService, param.Client
                        );

                        return new FinancialData(viewModel);
                    }
                    else if (viewModelType == typeof(ReferencesViewModel))
                    {
                        var param = parameter as ClientDataParams ?? new ClientDataParams(PageMode.Registration, RegistrationType.Client, Client);

                        var viewModel = new ReferencesViewModel(
                            dialogService, NavigationService, param.Mode, ClientManagementService, param.Client
                        );

                        return new References(viewModel);
                    }
                    else if (viewModelType == typeof(CreditDetailsViewModel))
                    {
                        var param = parameter as ClientDataParams ?? new ClientDataParams(PageMode.Registration, RegistrationType.Client, Client);

                        var viewModel = new CreditDetailsViewModel(
                            dialogService, NavigationService, param.Mode, param.Client, CreditApplicationService, PromotionalOfferService, CreditApplicationGeneratorService, ClientManagementService
                        );

                        return new CreditDetails(viewModel);
                    }
                    else if (viewModelType == typeof(EmployeeAndClientConsultationViewModel))
                    {
                        var param = parameter as ConsultationParams ?? new ConsultationParams(RegistrationType.Client);

                        var viewModel = new EmployeeAndClientConsultationViewModel(
                            dialogService, NavigationService, param.RegistrationType, ClientManagementService
                        );

                        return new EmployeeAndClientConsultation(viewModel);
                    }
                    else if (viewModelType == typeof(UserRegistrationViewModel))
                    {
                        var param = parameter as PersonDataParams ?? new PersonDataParams(PageMode.Update, new Employee());

                        var viewModel = new UserRegistrationViewModel(
                            dialogService, NavigationService, param.Mode, param.Person
                        );

                        return new UserRegistration(viewModel);
                    }
                    else if (viewModelType == typeof(CatalogsViewModel))
                    {

                        var viewModel = new CatalogsViewModel(
                            dialogService, NavigationService
                        );

                        return new Catalogs(viewModel);
                    }
                    else if (viewModelType == typeof(CreditPoliciesManagementViewModel))
                    {

                        var viewModel = new CreditPoliciesManagementViewModel(
                            dialogService, NavigationService, CreditPolicyService
                        );

                        return new CreditPoliciesManagement(viewModel);
                    }
                    else if (viewModelType == typeof(PromotionalOffersManagementViewModel))
                    {

                        var viewModel = new PromotionalOffersManagementViewModel(
                            dialogService, NavigationService, PromotionalOfferService
                        );

                        return new PromotionalOffersManagement(viewModel);
                    }
                    else if (viewModelType == typeof(CreditApplicationsViewModel))
                    {

                        var viewModel = new CreditApplicationsViewModel(
                            dialogService, NavigationService, CreditApplicationService
                        );

                        return new CreditApplications(viewModel);
                    }
                    else if (viewModelType == typeof(CreditApplicationValidationViewModel))
                    {

                        var param = parameter as CreditApplication ?? new CreditApplication();

                        var viewModel = new CreditApplicationValidationViewModel(
                            dialogService, NavigationService, CreditApplicationService, param
                        );

                        return new CreditApplicationValidation(viewModel);
                    }
                    else if (viewModelType == typeof(AmortizationScheduleViewModel))
                    {
                        var param = parameter as CreditApplication ?? new CreditApplication();
                        var viewModel = new AmortizationScheduleViewModel(
                            NavigationService, dialogService, AmortizationScheduleService, param
                        );
                        return new Independiente.View.Pages.AmortizationSchedule(viewModel);
                    }
                    else if (viewModelType == typeof(PaymentsViewModel))
                    {
                        var viewModel = new PaymentsViewModel(
                            dialogService, NavigationService, PaymentService, CatalogService, FilePickerService
                        );
                        return new Payments(viewModel);
                    }
                    else if (viewModelType == typeof(CreditPoliciesManagementViewModel))
                    {
                        var viewModel = new CreditPoliciesManagementViewModel(
                            dialogService, NavigationService, CreditPolicyService
                        );
                        return new CreditPoliciesManagement(viewModel);
                    }
                    throw new ArgumentException("ViewModel desconocido");
                });

            NavigationService.NavigateTo<PromotionalOffersManagementViewModel>();

            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(dialogService);
            this.DataContext = mainWindowViewModel;
        }





    }
}
