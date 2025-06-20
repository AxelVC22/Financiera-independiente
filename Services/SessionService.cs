using Independiente.DataAccess;
using Independiente.DataAccess.Repositories;
using Independiente.Model;
using Independiente.Properties;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Forms.Design;

namespace Independiente.Services
{
    public sealed class SessionService
    {
        public static SessionService Innstance { get; private set; }

        public Model.User CurrentUser { get;  set; }
        private readonly IUserRepository _userRepository;

        public SessionService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            CurrentUser = null;
            Innstance = this;
        }

        public (bool success, string message) AuthEmployee(string email, string password)
        {
            bool auth = false;
            string infoMessage = string.Empty;
            Model.User tempUser = null;

            try
            {
                if (FieldValidator.IsValidEmail(email) && FieldValidator.IsValidPassword(password))
                {

                    tempUser = _userRepository.GetUserByEmployeeCredentials(email, password);
                    if (tempUser != null)
                    {
                        CurrentUser = tempUser;
                        auth = true;
                    }
                    else
                    {
                        auth = false;
                        infoMessage = Messages.CredentialsNotFoundMessage;
                    }
                }
            }
            catch (ArgumentException exception)
            {
                infoMessage = exception.Message;
            }

            return (auth, infoMessage);
        }

        public void LogOut()
        {
            CurrentUser = null;
        }

        private string EncryptPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] stream = sha256.ComputeHash(Encoding.ASCII.GetBytes(password));
                StringBuilder stringBuilder = new StringBuilder();

                for (int i = 0; i < stream.Length; i++)
                {
                    stringBuilder.AppendFormat("{0:x2}", stream[i]);
                }
                return stringBuilder.ToString();
            }
        }
    }
}