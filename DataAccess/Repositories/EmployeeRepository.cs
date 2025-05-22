using Independiente.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.DataAccess.Repositories
{

    public interface IEmployeeRepository
    {
        int AddEmployee(Employee employee);

        int DeleteEmployee(Employee employee);
    }

    public class EmployeeRepository : IEmployeeRepository
    {
        public int AddEmployee(Employee employee)
        {
            int id = 0;

            try
            {
                using (var context = new IndependienteEntities()) 
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var newEmployee = new Employee
                            {
                                NSS = employee.NSS,
                                HireDate = employee.HireDate,
                                Department = employee.Department,
                                AddressData = employee.AddressData,
                                PersonalData = employee.PersonalData,
                                Status = employee.Status,
                                User = employee.User
                            };
                            context.Employee.Add(newEmployee); 
                            context.SaveChanges(); 
                            transaction.Commit();
                            id = newEmployee.EmployeeId;
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

        public int DeleteEmployee(Employee employee)
        {
            int result = 0;

            try
            {
                using (var context = new IndependienteEntities())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var employeeForDelete = context.Employee.Find(employee.EmployeeId);

                            if (employeeForDelete != null)
                            {
                                var personalData = employeeForDelete.PersonalData;
                                var user = employeeForDelete.User;
                                var addressData = employeeForDelete.AddressData;

                                context.Employee.Remove(employeeForDelete);
                                context.PersonalData.Remove(personalData);
                                context.AddressData.Remove(addressData);
                                context.User.Remove(user);

                                result = context.SaveChanges();

                                transaction.Commit();
                            } 
                            else
                            {
                                //throw new Exception("Employee not found.");
                            }
                        }
                        catch (Exception)
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

            return result;
        }
    }
}
