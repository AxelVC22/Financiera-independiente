using Independiente.DataAccess;
using Independiente.Model;
using Independiente.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.Services
{
    public interface IClientManagementService
    {
       // Client[] GetAllClientsBy
        bool ValidatePersonalData(Model.PersonalData personalData);

        bool ValidateAddressData(Model.AddressData addressData);

        bool ValidateReference(Model.Reference reference);

        bool IsRFCRegistered(string RFC, out string message);

        bool IsValidAge(DateTime birthDate, out string message);

        bool IsPhoneNumberRepeated(string phoneNumber, string alternativePhoneNumber, out string message);
    }

    public class ClientManagerService : IClientManagementService
    {
        public bool IsRFCRegistered(string RFC, out string message)
        {
            message = null;
            bool validation = false;
            using (var context = new IndependienteEntities())
            {
                if (context.PersonalData.Any(personalData => personalData.RFC == RFC))
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
                   FieldValidator.IsValidEmail(personalData.Email);
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
    }
}
