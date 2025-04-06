using Independiente.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Independiente.Services
{
    public class FieldValidator
    {
        private const string PasswordRegex = @"^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*()_+[\]{};':""\\|,.<>/?-]).{8,64}$";
        private const string EmailRegex = @"^(?=.{1,50}$)[^@\s]+@[^@\s]+\.[^@\s]+$";
        private const string NameRegex = @"^[A-Za-zÁÉÍÓÚáéíóúÜüÑñ\s']{2,50}(?:\s[A-Za-zÁÉÍÓÚáéíóúÜüÑñ\s']{2,50})*$";
        private const string RFCRegex = @"^[A-ZÑ&]{4}\d{6}[A-Z0-9]{3}$";
        private const string CURPRegex = @"^[A-Z]{4}\d{6}[HM][A-Z]{2}[A-Z0-9]{5}$";
        private const string PhoneNumberRegex = @"^(\+52|52)?[ -]*(\(?\d{2,3}\)?[ -]*)?\d{3}[ -]*\d{2}[ -]*\d{2}$";
        private const string StreetRegex = @"^[A-Za-zÁÉÍÓÚáéíóúÜüÑñ0-9\s#\-\.\,]+$";
        private const string NeighborhoodRegex = @"^[A-Za-zÁÉÍÓÚáéíóúÜüÑñ0-9\s\-\.\,]+$";
        private const string CityRegex = @"^[A-Za-zÁÉÍÓÚáéíóúÜüÑñ\s\-]+$";

        public static bool IsValidEmail(string email, out string errorMessage)
        {
            errorMessage = null;

            if (string.IsNullOrEmpty(email) || !Regex.IsMatch(email, EmailRegex))
            {
                errorMessage = Messages.InvalidEmailMessage;
                return false;
            }
            return true;
        }

        public static bool IsValidPassword(string password, out string errorMessage)
        {
            errorMessage = null;

            if (string.IsNullOrEmpty(password) || !Regex.IsMatch(password, PasswordRegex))
            {
                errorMessage = Messages.InvalidPasswordMessage;
                return false;
            }
            return true;
        }

        public static bool IsValidName(string name, out string errorMessage)
        {
            errorMessage = null;

            if (string.IsNullOrEmpty(name) || !Regex.IsMatch(name, NameRegex))
            {
                errorMessage = Messages.InvalidNameMessage;
                return false;
            }
            return true;
        }

        public static bool IsValidSurname(string surname, out string errorMessage)
        {
            errorMessage = null;

            if (string.IsNullOrEmpty(surname) || !Regex.IsMatch(surname, NameRegex))
            {
                errorMessage = Messages.InvalidSurnameMessage;
                return false;
            }
            return true;
        }

        public static bool IsValidRFC(string rfc, out string errorMessage)
        {
            errorMessage = null;

            if (string.IsNullOrEmpty(rfc) || !Regex.IsMatch(rfc, RFCRegex))
            {
                errorMessage = Messages.InvalidRFCMessage;
                return false;
            }
            return true;
        }

        public static bool IsValidCURP(string curp, out string errorMessage)
        {
            errorMessage = null;

            if (string.IsNullOrEmpty(curp) || !Regex.IsMatch(curp, CURPRegex))
            {
                errorMessage = Messages.InvalidCURPMessage;
                return false;
            }
            return true;
        }

        public static bool IsValidPhoneNumber(string phoneNumber, out string errorMessage)
        {
            errorMessage = null;
            if (string.IsNullOrEmpty(phoneNumber) || !Regex.IsMatch(phoneNumber, PhoneNumberRegex))
            {
                errorMessage = Messages.InvalidPhoneNumberMessage;
                return false;
            }
            return true;
        }

        public static bool IsValidStreet(string street, out string errorMessage)
        {
            errorMessage = null;
            if (string.IsNullOrEmpty(street) || !Regex.IsMatch(street, StreetRegex))
            {
                errorMessage = Messages.InvalidStreetMessage;
                return false;
            }
            return true;
        }

        public static bool IsValidNeighborhood(string neighborhood, out string errorMessage)
        { 
            errorMessage = null;
            if (!string.IsNullOrEmpty(neighborhood) || !Regex.IsMatch(neighborhood, NeighborhoodRegex))
            {
                errorMessage = Messages.InvalidNeighborhoodMessage;
                return false;
            }
            return true;
        }

        public static bool IsValidCity(string city, out string errorMessage)
        {
            errorMessage = null;
            if(!string.IsNullOrEmpty(city) || !Regex.IsMatch(city, CityRegex))
            {
                errorMessage = Messages.InvalidCityMessage;
                return false;
            }
            return true;
        }
    }
}
