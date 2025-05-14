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

namespace Independiente
{

    public partial class MainWindow : Window
    {

        public ICreditApplicationService CreditApplicationService {  get; set; }
        public INavigationService NavigationService { get;  set; }

        public ICreditApplicationRepository CreditApplicationRepository { get; set; }

        public ICreditPolicyRepository CreditPolicyRepository { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            IDialogService dialogService = new DialogService();

            CreditPolicyRepository = new CreditPolicyRepository();
            CreditApplicationRepository = new CreditApplicationRepository();
            CreditApplicationService = new CreditApplicationService(CreditApplicationRepository, CreditPolicyRepository);

            NavigationService = new FrameNavigationService(
                PageFrame,
                (viewModelType, parameter) =>
                {
                    if (viewModelType == typeof(PersonalDataViewModel))
                    {
                        var param = parameter as PersonDataParams ?? new PersonDataParams(PageMode.Registration, RegistrationType.Client, new Client());

                        var viewModel = new PersonalDataViewModel(
                            dialogService, NavigationService, param.Mode, param.RegistrationType, param.Person
                        );

                        return new View.Pages.PersonalData(viewModel);
                    }
                    else if (viewModelType == typeof(FinancialDataViewModel))
                    {
                        var param = parameter as PersonDataParams ?? new PersonDataParams(PageMode.Registration, RegistrationType.Client, new Client());

                        var viewModel = new FinancialDataViewModel(
                            dialogService, NavigationService, param.Mode
                        );

                        return new FinancialData(viewModel);
                    }
                    else if (viewModelType == typeof(ReferencesViewModel))
                    {
                        var param = parameter as PersonDataParams ?? new PersonDataParams(PageMode.Registration, RegistrationType.Client, new Client());

                        var viewModel = new ReferencesViewModel(
                            dialogService, NavigationService, param.Mode
                        );

                        return new References(viewModel);
                    }
                    else if (viewModelType == typeof(CreditDetailsViewModel))
                    {
                        var param = parameter as PersonDataParams ?? new PersonDataParams(PageMode.Registration, RegistrationType.Client, new Client());

                        var viewModel = new CreditDetailsViewModel(
                            dialogService, NavigationService, param.Mode
                        );

                        return new CreditDetails(viewModel);
                    }
                    else if (viewModelType == typeof(EmployeeAndClientConsultationViewModel))
                    {

                        var viewModel = new EmployeeAndClientConsultationViewModel(
                            dialogService, NavigationService
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
                            dialogService, NavigationService
                        );

                        return new CreditPoliciesManagement(viewModel);
                    }
                    else if (viewModelType == typeof(PromotionalOffersManagementViewModel))
                    {

                        var viewModel = new PromotionalOffersManagementViewModel(
                            dialogService, NavigationService
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
                    throw new ArgumentException("ViewModel desconocido");
                });

            NavigationService.NavigateTo<CreditApplicationsViewModel>();

            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(dialogService);
            this.DataContext = mainWindowViewModel;
        }





    }
}
