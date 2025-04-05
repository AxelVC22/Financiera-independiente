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


        private List<string> _statesList;
        private string _selectedState;

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
            _navigationService = navigationService;
            _dialogService = dialogService;
            SwitchMode(mode);
            _pageMode = mode;
        }
        private void GoBack(object obj)
        {
            _navigationService.GoBack();
        }

        private void Next(object obj)
        {

            _navigationService.NavigateTo<CreditDetailsViewModel>(new PersonalDataParams(_pageMode));

        }

        private void Cancel(object obj)
        {
            Console.WriteLine("cancelaste");
            SwitchMode(PageMode.View);
        }

        private void Save(object obj)
        {

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
     
        public List<string> StatesList
        {
            get => _statesList;
            set
            {
                _statesList = value;
                OnPropertyChanged(nameof(StatesList));
            }
        }

    }
}
