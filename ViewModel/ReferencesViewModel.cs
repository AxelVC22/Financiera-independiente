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

        private IDialogService _dialogService {  get; set; }
        private INavigationService _navigationService { get; set; }

        private PageMode _pageMode { get; set; }

        public ReferencesViewModel ()
        {

        }
        public ReferencesViewModel(IDialogService dialogService, INavigationService navigationService, PageMode mode)
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

            _navigationService.NavigateTo<CreditDetailsViewModel>(new PersonDataParams(_pageMode));


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
