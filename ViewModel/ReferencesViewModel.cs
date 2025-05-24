using Independiente.Commands;
using Independiente.Model;
using Independiente.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Independiente.ViewModel
{
    public class ReferencesViewModel : ModificableViewModel
    {
        public Reference FirstReference { get; set; }

        public Reference SecondReference { get; set; }

        public WorkCenter WorkCenter { get; set; }


        private string _selectedState;

        public List<string> RelationshipsList { get; set; }

        private IDialogService _dialogService { get; set; }

        private INavigationService _navigationService { get; set; }

        private IClientManagementService _clientManagementService { get; set; }

        private PageMode _pageMode { get; set; }

        private Client Client { get; set; }

        public ReferencesViewModel()
        {

        }
        public ReferencesViewModel(IDialogService dialogService, INavigationService navigationService, PageMode mode, IClientManagementService ClientManagementService, Client client)
        {
            NextCommand = new RelayCommand(Next, CanNext);
            EditCommand = new RelayCommand(Edit, CanNext);
            CancelCommand = new RelayCommand(Cancel, CanNext);
            SaveCommand = new RelayCommand(Save, CanNext);
            GoBackCommand = new RelayCommand(GoBack, CanNext);

            FirstReference = new Reference();

            SecondReference = new Reference();

            WorkCenter = new WorkCenter();
            LoadRelationships();
            _navigationService = navigationService;
            _dialogService = dialogService;
            SwitchMode(mode);
            _pageMode = mode;
            _clientManagementService = ClientManagementService;
            Client = client;
        }

        private void LoadRelationships()
        {
            RelationshipsList = new List<string>();
            var resourceManager = new ResourceManager("Independiente.Properties.Relationships", typeof(ReferencesViewModel).Assembly);

            var resourceSet = resourceManager.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, true, true);

            var states = resourceSet.Cast<DictionaryEntry>()
                                    .Where(entry => entry.Value is string)
                                    .Select(entry => entry.Value.ToString())
                                    .ToList();
            RelationshipsList = states;
        }

        private void GoBack(object obj)
        {
            _navigationService.GoBack();
        }

        private void Next(object obj)
        {
            string message = string.Empty;
            bool validation = false;
            try
            {
                if (_clientManagementService.ValidateReference(FirstReference) &&
                        _clientManagementService.ValidateReference(SecondReference))
                {
                    validation = true;
                }
                else
                {
                    message = "Las referencias no son válidas";
                    validation = false;
                }
            }
            catch (ArgumentException exception)
            {
                message = exception.Message;
            }

            if (validation)
            {
                Client.FirstReference = FirstReference;
                Client.SecondReference = SecondReference;
                Client.WorkCenter = WorkCenter;
                _navigationService.NavigateTo<CreditDetailsViewModel>(new ClientDataParams(_pageMode, Client));
            }
            else
            {
                _dialogService.Dismiss(message, System.Windows.MessageBoxImage.Information);
            }
        }

        private void Cancel(object obj)
        {
            Console.WriteLine("cancelaste");
            SwitchMode(PageMode.View);
        }

        private void Save(object obj)
        {
            Console.WriteLine(FirstReference.ToString());

            Console.WriteLine(SecondReference.ToString());

            Console.WriteLine(WorkCenter.ToString());
            SwitchMode(PageMode.View);
        }

        private void Edit(object obj)
        {

            SwitchMode(PageMode.Update);
        }

        private bool CanNext(object obj)
        {
            return true;
        }



    }
}
