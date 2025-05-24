using Independiente.DataAccess;
using Independiente.Model;
using Independiente.Properties;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace Independiente.Services
{
    public sealed class SessionService
    {
        private static readonly Lazy<SessionService> _instance = new Lazy<SessionService>(() => new SessionService());

        public static SessionService Instance => _instance.Value;

        public Model.User CurrentUser { get; private set; }

        public SessionService()
        {
            CurrentUser = null;
        }

        public (bool success, string message) AuthEmployee(string email, string password)
        {
            bool auth = false;
            string infoMessage = string.Empty;

            try
            {
                if (FieldValidator.IsValidEmail(email) && FieldValidator.IsValidPassword(password))
                {
                    using (var context = new IndependienteEntities())
                    {
                        var searchedEmployee = context.EmployeeView.FirstOrDefault(
                            employee => employee.Email == email && employee.Password == password);

                        if (searchedEmployee != null)
                        {
                            CurrentUser = new Model.User
                            {
                                Id = searchedEmployee.UserId,
                                EmployeeId = searchedEmployee.EmployeeId,
                                Name = searchedEmployee.EmployeeName,
                                Role = searchedEmployee.Role,
                            };
                            auth = true;
                        }
                        else
                        {
                            auth = false;
                            infoMessage = Messages.CredentialsNotFoundMessage;
                        }
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