using Independiente.Model;
using Independiente.Services;
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
        public INavigationService NavigationService { get; private set; }
        public IClientManagementService ClientManagementService { get; private set; }
        public ICatalogService CatalogService { get; private set; }
        public Client Client { get; private set; }
        public MainWindow()
        {
            InitializeComponent();

            IDialogService dialogService = new DialogService();
            ClientManagementService = new ClientManagerService();
            CatalogService = new CatalogManagerService();
            Client = new Client();

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
                        var param = parameter as PersonDataParams ?? new PersonDataParams(PageMode.Registration, RegistrationType.Client, Client);

                        var viewModel = new FinancialDataViewModel(
                            dialogService, NavigationService, param.Mode, ClientManagementService, CatalogService
                        );

                        return new FinancialData(viewModel);
                    }
                    else if (viewModelType == typeof(ReferencesViewModel))
                    {
                        var param = parameter as PersonDataParams ?? new PersonDataParams(PageMode.Registration, RegistrationType.Client, new Client());

                        var viewModel = new ReferencesViewModel(
                            dialogService, NavigationService, param.Mode, ClientManagementService
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
                        var param = parameter as ConsultationParams ?? new ConsultationParams(RegistrationType.Employee);

                        var viewModel = new EmployeeAndClientConsultationViewModel(
                            dialogService, NavigationService, param.RegistrationType
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
                    throw new ArgumentException("ViewModel desconocido");
                });

            NavigationService.NavigateTo<CreditPoliciesManagementViewModel>();

            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(dialogService);
            this.DataContext = mainWindowViewModel;
        }





    }
}