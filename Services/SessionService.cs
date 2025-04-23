using Independiente.DataAccess;
using Independiente.Model;
using Independiente.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Independiente.Services
{
    public class SessionService
    {
        public Model.User CurrentUser { get; set; }


        public (bool success, string message) AuthEmployee(string email, string password)
        {
            bool auth = false;
            string infoMessage = string.Empty;

            try
            {
                if (FieldValidator.IsValidEmail(email) && FieldValidator.IsValidPassword(password))
                {
                    //String hashedPassword = EncryptPassword(Password);

                    using (var context = new IndependienteEntities())
                    {
                        var searchedEmployee = context.EmployeeView.FirstOrDefault(employee => employee.Email == email && employee.Password == password);
                        if (searchedEmployee != null)
                        {
                            CurrentUser = new Model.User
                            {
                                Id = searchedEmployee.UserId,
                                Name = searchedEmployee.Name,
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

            return(auth, infoMessage);
        }

        public void LogOut()
        {
            CurrentUser = null;
        }

        private string EncryptPassword(string password)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder stringBuilder = new StringBuilder();

            stream = sha256.ComputeHash(encoding.GetBytes(password));
            for (int i = 0; i < stream.Length; i++)
            {
                stringBuilder.AppendFormat("{0:x2}", stream[i]);
            }
            return stringBuilder.ToString();
        }
    }

}
