using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Independiente.DataAccess.Repositories
{
    public interface IUserRepository
    {
        int AddUser(Model.User user);
        Model.User GetUserByEmployeeCredentials(string email, string password);
    }
    public class UserRepository : IUserRepository
    {
        public int AddUser(Model.User user)
        {
            int id = 0;

            if (user == null)
            {
                return 0;
            }

            try
            {
                using (var context = new IndependienteEntities())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var newUser = new User
                            {
                                Role = user.Role,
                                Password = user.Password
                            };
                            context.User.Add(newUser);
                            context.SaveChanges();
                            transaction.Commit();
                            id = newUser.UserId;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (DbUpdateException ex)
            {
            }
            catch (EntityException ex)
            {
            }

            return id;  
        }

        public Model.User GetUserByEmployeeCredentials(string email, string password)
        {
            Model.User user = null;

            try
            {

                using (var context = new IndependienteEntities())
                {
                    var searchedEmployee = context.EmployeeView.FirstOrDefault(employee => employee.Email == email && employee.Password == password);

                    if (searchedEmployee != null)
                    {
                        user = new Model.User
                        {
                            EmployeeId = searchedEmployee.EmployeeId,
                            Id = searchedEmployee.UserId,
                            Name = searchedEmployee.EmployeeName,
                            Role = searchedEmployee.Role
                        };
                    }
                }
            }
            catch (DbUpdateException ex)
            {
            }
            catch (EntityException ex)
            {
            }

            return user;
        }
    }
}
