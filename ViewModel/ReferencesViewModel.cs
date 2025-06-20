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
        private Reference OldFirstReference { get; set; }
        private Reference OldSecondReference { get; set; }
        private WorkCenter OldWorkCenter { get; set; }
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

            FirstReference = client?.FirstReference ?? new Reference();
            SecondReference = client?.SecondReference ?? new Reference();
            WorkCenter = client?.WorkCenter ?? new WorkCenter();

            LoadRelationships();

            _navigationService = navigationService;
            _dialogService = dialogService;
            SwitchMode(mode);
            _pageMode = mode;
            _clientManagementService = ClientManagementService;
            Client = client;

            if (_pageMode == PageMode.View)
            {
                OldFirstReference = CreateReferenceCopy(FirstReference);
                OldSecondReference = CreateReferenceCopy(SecondReference);
                OldWorkCenter = CreateWorkCenterCopy(WorkCenter);
            }

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
                        _clientManagementService.ValidateReference(SecondReference) &&
                        _clientManagementService.ValidateWorkCenter(WorkCenter))
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
            if (_dialogService.Confirm("¿Desea cancelar los cambios realizados?"))
            {
                if (OldFirstReference != null)
                {
                    FirstReference.Name = OldFirstReference.Name;
                    FirstReference.FullLastName = OldFirstReference.FullLastName;
                    FirstReference.PhoneNumber = OldFirstReference.PhoneNumber;
                    FirstReference.Relationship = OldFirstReference.Relationship;
                    FirstReference.Email = OldFirstReference.Email;
                }

                if (OldSecondReference != null)
                {
                    SecondReference.Name = OldSecondReference.Name;
                    SecondReference.FullLastName = OldSecondReference.FullLastName;
                    SecondReference.PhoneNumber = OldSecondReference.PhoneNumber;
                    SecondReference.Relationship = OldSecondReference.Relationship;
                    SecondReference.Email = OldSecondReference.Email;
                }

                if (OldWorkCenter != null)
                {
                    WorkCenter.Name = OldWorkCenter.Name;
                    WorkCenter.Role = OldWorkCenter.Role;
                    WorkCenter.HiringDate = OldWorkCenter.HiringDate;
                    WorkCenter.MontlyIncome = OldWorkCenter.MontlyIncome;
                }

                SwitchMode(PageMode.View);
            }
        }

        private void Save(object obj)
        {
            string message = string.Empty;
            bool validation = false;

            try
            {
                if (_clientManagementService.ValidateReference(FirstReference) &&
                    _clientManagementService.ValidateReference(SecondReference) &&
                    _clientManagementService.ValidateWorkCenter(WorkCenter))
                {
                    validation = true;
                }
                else
                {
                    message = "Las referencias o el centro de trabajo no son válidos.";
                }
            }
            catch (ArgumentException ex)
            {
                message = ex.Message;
            }

            if (validation)
            {
                var (updateMessage, updatedRef1, updatedRef2, updatedWC) = _clientManagementService.UpdateReferencesAndWorkCenter(FirstReference, SecondReference, WorkCenter);

                Client.FirstReference = updatedRef1;
                Client.SecondReference = updatedRef2;
                Client.WorkCenter = updatedWC;

                _dialogService.Dismiss(updateMessage, System.Windows.MessageBoxImage.Information);

                OldFirstReference = CreateReferenceCopy(updatedRef1);
                OldSecondReference = CreateReferenceCopy(updatedRef2);
                OldWorkCenter = CreateWorkCenterCopy(updatedWC);

                SwitchMode(PageMode.View);
            }
            else
            {
                _dialogService.Dismiss(message, System.Windows.MessageBoxImage.Warning);
            }
        }


        private void Edit(object obj)
        {

            SwitchMode(PageMode.Update);
        }

        private bool CanNext(object obj)
        {
            return true;
        }

        private Reference CreateReferenceCopy(Reference reference)
        {
            if (reference == null) return null;

            return new Reference
            {
                ReferenceId = reference.ReferenceId,
                Name = reference.Name,
                FullLastName = reference.FullLastName,
                PhoneNumber = reference.PhoneNumber,
                Relationship = reference.Relationship,
                Email = reference.Email
            };
        }

        private WorkCenter CreateWorkCenterCopy(WorkCenter workCenter)
        {
            if (workCenter == null) return null;

            return new WorkCenter
            {
                WorkCenterId = workCenter.WorkCenterId,
                Name = workCenter.Name,
                Role = workCenter.Role,
                HiringDate = workCenter.HiringDate,
                MontlyIncome = workCenter.MontlyIncome
            };
        }


    }
}
