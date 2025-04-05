using Independiente.Commands;
using Independiente.Model;
using Independiente.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;

namespace Independiente.ViewModel
{
    public class CreditDetailsViewModel : ModificableViewModel
    {

        public CreditDetails CreditDetails { get; set; }

        private IDialogService _dialogService;
        private INavigationService _navigationService;
        private readonly IFilePickerService _filePickerService;

        public ICommand SelectINECommand { get; set; }

        public string _inePath { get; set; }

        public string ProofOfAddressPath { get; set; }

        public string AccountStatementCoverPagePath { get; set; }

        public string CreditApplicationLabelGlobal {  get; set; }

        private PageMode _pageMode { get; set; }


        public CreditDetailsViewModel()
        {

        }

        public CreditDetailsViewModel(IDialogService dialogService, INavigationService navigationService, PageMode mode)
        {
            NextCommand = new RelayCommand(Next, CanNext);
            EditCommand = new RelayCommand(Edit, CanNext);
            CancelCommand = new RelayCommand(Cancel, CanNext);
            SaveCommand = new RelayCommand(Save, CanNext);
            GoBackCommand = new RelayCommand(GoBack, CanNext);
            SelectINECommand = new RelayCommand(SelectINE, CanNext);

            CreditDetails = new CreditDetails();

            _dialogService = dialogService;
            _navigationService = navigationService;
            _filePickerService = new FilePickerService();

        }

        public string INEPath
        {
            get => _inePath;
            set
            {
                if (_inePath != value)
                {
                    _inePath = value;
                    OnPropertyChanged(nameof(INEPath));
                    OnPropertyChanged(nameof(INEFileName));// Notifica el cambio de la propiedad
                }
            }
        }
        public string INEFileName
        {
            get => Path.GetFileName(INEPath); // Devuelve solo el nombre del archivo
        }

        private void SelectINE(object obj)
        {
            INEPath = OpenFile();

            Console.WriteLine(INEPath);
        }

        private string OpenFile()
        {
            string filePath = _filePickerService.PickFile();
            if (string.IsNullOrEmpty(filePath))
            {
                IDialogService dialogService = new DialogService();
                dialogService.Dismiss("gg");
            }
            return filePath;

        }

        private void Next(object obj)
        {
             _navigationService.NavigateTo<ReferencesViewModel>(new PersonalDataParams(_pageMode));
        }

        private void GoBack(object obj)
        {
            _navigationService.GoBack();
        }


        private void Cancel(object obj)
        {
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

       

    }
}
