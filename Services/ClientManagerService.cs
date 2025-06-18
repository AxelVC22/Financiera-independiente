using Independiente.DataAccess;
using Independiente.DataAccess.Repositories;
using Independiente.Model;
using Independiente.Properties;
using Independiente.Services.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.Services
{
    public interface IClientManagementService
    {
        List<Model.Client> GetAllClientsByEmployeeId(int employeeId);
        bool ValidatePersonalData(Model.PersonalData personalData);

        bool ValidateAddressData(Model.AddressData addressData);

        bool ValidateReference(Model.Reference reference);

        bool ValidateWorkCenter(Model.WorkCenter workCenter);

        bool IsRFCRegistered(string RFC, int currentPersonalDataId, out string message);

        bool IsValidAge(DateTime birthDate, out string message);

        bool IsPhoneNumberRepeated(string phoneNumber, string alternativePhoneNumber, out string message);

        int AddClient(Model.Client client);

        (string message, Model.PersonalData personalData) UpdatePersonalData(Model.PersonalData personalData);

        (string message, Model.AddressData addressData) UpdateAddressData(Model.AddressData addressData);

        (string message, Model.Account depositAccount, Model.Account paymentAccount) UpdateAccounts(Model.Account depositAccount, Model.Account paymentAccount);
    }

    public class ClientManagerService : IClientManagementService
    {
        public List<Model.Client> GetAllClientsByEmployeeId(int employeeId)
        {
            List<Model.Client> clients = new List<Model.Client>();
            using (var context = new IndependienteEntities())
            {
                var Clients = context.ClientView.Where(client => client.EmployeeId == employeeId).ToList();

                foreach (var Client in Clients)
                {
                    clients.Add(Model.Client.FromClientView(Client));
                }
            }

            return clients;

        }

        public bool IsRFCRegistered(string RFC, int currentPersonalDataId, out string message)
        {
            message = null;
            bool validation = false;
            using (var context = new IndependienteEntities())
            {
                if (context.PersonalData.Any(pd => pd.RFC == RFC && pd.PersonalDataId != currentPersonalDataId))
                {
                    message = Messages.RFCRegisteredMessage;
                    validation = true;
                }
            }

            return validation;
        }

        public bool ValidatePersonalData(Model.PersonalData personalData)
        {
            return FieldValidator.IsValidName(personalData.Name) &&
                   FieldValidator.IsValidSurname(personalData.Surname) &&
                   FieldValidator.IsValidSurname(personalData.LastName) &&
                   FieldValidator.IsValidRFC(personalData.RFC) &&
                   FieldValidator.IsValidCURP(personalData.CURP) &&
                   FieldValidator.IsValidPhoneNumber(personalData.PhoneNumber) &&
                   FieldValidator.IsValidPhoneNumber(personalData.AlternativePhoneNumber) &&
                   FieldValidator.IsValidEmail(personalData.Email) &&
                   FieldValidator.IsValidDate(personalData.BirthDate);
        }

        public bool ValidateAddressData(Model.AddressData addressData)
        {
            return FieldValidator.IsValidStreet(addressData.Street) &&
                    FieldValidator.IsValidNeighborhood(addressData.NeighborHood) &&
                    FieldValidator.IsValidCity(addressData.City);
        }

        public bool ValidateReference(Model.Reference reference)
        {
            return FieldValidator.IsValidName(reference.Name) &&
                FieldValidator.IsValidSurname(reference.FullLastName) &&
                FieldValidator.IsValidEmail(reference.Email) &&
                FieldValidator.IsValidPhoneNumber(reference.PhoneNumber);
        }

        public bool ValidateWorkCenter(Model.WorkCenter workCenter)
        {
            return FieldValidator.IsValidName(workCenter.Name) &&
                FieldValidator.IsValidRole(workCenter.Role) &&
                FieldValidator.IsValidMoney(workCenter.MontlyIncome) &&
                FieldValidator.IsValidDate(workCenter.HiringDate);
        }

        public bool IsValidAge(DateTime birthDate, out string message)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            bool ageValidation = true;
            message = null;

            if (birthDate.Date > today.AddYears(-age))
            {
                age--;
            }

            if (age <= 18)
            {
                message = Messages.UnderEighteenYearsMessage;
                ageValidation = false;
            }
            if (age >= 60)
            {
                message = Messages.OverSixtyYearsMessage;
                ageValidation = false;
            }

            return ageValidation;
        }

        public bool IsPhoneNumberRepeated(string phoneNumber, string alternativePhoneNumber, out string message)
        {
            message = null;
            bool validation = false; ;

            if (phoneNumber.Equals(alternativePhoneNumber))
            {
                message = Messages.PhoneNumberIsRepeated;
                validation = true;
            }
            return validation;
        }

        public int AddClient(Model.Client client)
        {
            int success = 0;

            IClientRepository clientRepository = new ClientRepository();
            success = clientRepository.AddClient(ClientMapper.ToDataModel(client));

            return success;
        }

        public (string message, Model.PersonalData personalData) UpdatePersonalData(Model.PersonalData personalData)
        {
            int success = 0;
            string message = null;
            Model.PersonalData updatedPersonalData = null;

            IClientRepository clientRepository = new ClientRepository();
            (success, updatedPersonalData) = clientRepository.UpdatePersonalData(personalData);

            message = GetUpdateMessage(success);

            return (message, updatedPersonalData);
        }

        public (string message, Model.AddressData addressData) UpdateAddressData(Model.AddressData addressData)
        {
            int success = 0;
            string message = null;
            Model.AddressData updatedAddressData = null;

            IClientRepository clientRepository = new ClientRepository();
            (success, updatedAddressData) = clientRepository.UpdateAddressData(addressData);
            message = GetUpdateMessage(success);
            return (message, updatedAddressData);
        }

        public (string message, Model.Account depositAccount, Model.Account paymentAccount) UpdateAccounts(Model.Account depositAccount, Model.Account paymentAccount)
        {
            int successDeposit = 0;
            int successPayment = 0;
            string message = string.Empty;
            Model.Account updatedDeposit = null;
            Model.Account updatedPayment = null;
            IClientRepository clientRepository = new ClientRepository();

            if (depositAccount?.AccountId > 0)
            {
                (successDeposit, updatedDeposit) = clientRepository.UpdateAccount(depositAccount);
            }

            if (paymentAccount?.AccountId > 0)
            {
                (successPayment, updatedPayment) = clientRepository.UpdateAccount(paymentAccount);
            }

            if (successDeposit == -1 || successPayment == -1)
            {
                message = GetUpdateMessage(-1);
            }
            else if (successDeposit == 0 && successPayment == 0)
            {
                message = GetUpdateMessage(0);
            }
            else
            {
                message = GetUpdateMessage(successDeposit);
            }

            return (message, updatedDeposit ?? depositAccount, updatedPayment ?? paymentAccount);
        }


        private string GetUpdateMessage(int result)
        {
            switch (result)
            {
                case -1:
                    return "Error en la base de datos";
                case 0:
                    return "";
                case 1:
                    return "Datos actualizados exitosamente";
                default:
                    return $"Se actualizaron {result} registros";
            }
        }
    }
}
