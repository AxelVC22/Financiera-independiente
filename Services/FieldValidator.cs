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
        private const string PasswordPattern = @"^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*()_+[\]{};':""\\|,.<>/?-]).{8,64}$";
        private const string EmailPattern = @"^(?=.{1,50}$)[^@\s]+@[^@\s]+\.[^@\s]+$";

        public static bool IsValidEmail(string email, out string errorMessage)
        {
            errorMessage = null;

            if (string.IsNullOrEmpty(email) || !Regex.IsMatch(email, EmailPattern))
            {
                errorMessage = Messages.InvalidEmailMessage;
                return false;
            }
            return true;
        }

        public static bool IsValidPassword(string password, out string errorMessage)
        {
            errorMessage = null;

            if (string.IsNullOrEmpty(password) || !Regex.IsMatch(password, PasswordPattern))
            {
                errorMessage = Messages.InvalidPasswordMessage;
                return false;
            }
            return true;
        }
    }
}
