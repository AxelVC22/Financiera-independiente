using Independiente.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
        private const string CLABERegex = @"^\d{18}$";

        public static bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password) || !Regex.IsMatch(password, PasswordRegex))
            {
                throw new ArgumentException(Messages.InvalidPasswordMessage);
            }
            return true;
        }

        public static bool IsValidEmail(string email)
        {

            if (string.IsNullOrEmpty(email) || !Regex.IsMatch(email, EmailRegex))
            {
                throw new ArgumentException(Messages.InvalidEmailMessage);
            }
            return true;
        }

        public static bool IsValidName(string name)
        {
            if (string.IsNullOrEmpty(name) || !Regex.IsMatch(name, NameRegex))
            {
                throw new ArgumentException(Messages.InvalidNameMessage);
            }
            return true;
        }

        public static bool IsValidSurname(string surname)
        {
            if (string.IsNullOrEmpty(surname) || !Regex.IsMatch(surname, NameRegex))
            {
                throw new ArgumentException(Messages.InvalidSurnameMessage);
            }
            return true;
        }

        public static bool IsValidRFC(string rfc)
        {
            if (string.IsNullOrEmpty(rfc) || !Regex.IsMatch(rfc, RFCRegex))
            {
                throw new ArgumentException(Messages.InvalidRFCMessage);
            }
            return true;
        }

        public static bool IsValidCURP(string curp)
        {
            if (string.IsNullOrEmpty(curp) || !Regex.IsMatch(curp, CURPRegex))
            {
                throw new ArgumentException(Messages.InvalidCURPMessage);
            }
            return true;
        }

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber) || !Regex.IsMatch(phoneNumber, PhoneNumberRegex))
            {
                throw new ArgumentException(Messages.InvalidPhoneNumberMessage);
            }
            return true;
        }

        public static bool IsValidStreet(string street)
        {
            if (string.IsNullOrEmpty(street) || !Regex.IsMatch(street, StreetRegex))
            {
                throw new ArgumentException(Messages.InvalidStreetMessage);
            }
            return true;
        }

        public static bool IsValidNeighborhood(string neighborhood)
        {
            if (string.IsNullOrEmpty(neighborhood) || !Regex.IsMatch(neighborhood, NeighborhoodRegex))
            {
                throw new ArgumentException(Messages.InvalidNeighborhoodMessage);
            }
            return true;
        }

        public static bool IsValidCity(string city)
        {
            if (string.IsNullOrEmpty(city) || !Regex.IsMatch(city, CityRegex))
            {
                throw new ArgumentException(Messages.InvalidCityMessage);
            }
            return true;
        }

        public static bool IsValidCLABE(string clabe)
        {
            if (string.IsNullOrEmpty(clabe) || !Regex.IsMatch(clabe, CLABERegex))
            {
                throw new ArgumentException(Messages.InvalidCLABEMessage);
            }
            return true;
        }

    }
}