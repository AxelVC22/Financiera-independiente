using Independiente.Model;
using Independiente.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Contexts;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Independiente.DataAccess.Repositories
{

    public class EmployeeQuery : INotifyPropertyChanged, IQueryObject<EmployeeView>
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        private string _rfc;

        private string _department;

        private string _role;

        private string _status;

        public string RFC
        {
            get => _rfc;
            set
            {
                if (_rfc != value)
                {
                    _rfc = value;
                    OnPropertyChanged(nameof(RFC));
                }
            }
        }

        public string Department
        {
            get => _department;
            set
            {
                if (_department != value)
                {
                    _department = value;
                    OnPropertyChanged(nameof(Department));
                }
            }
        }

        public string Role
        {
            get => _role;
            set
            {
                if (_role != value)
                {
                    _role = value;
                    OnPropertyChanged(nameof(Role));
                }
            }
        }

        public string Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        public Expression<Func<EmployeeView, bool>> BuildExpression()
        {            
            return c =>
                (string.IsNullOrEmpty(RFC) || c.RFC == RFC) &&
                (string.IsNullOrEmpty(Status) || c.Status == Status) &&
                (string.IsNullOrEmpty(Role) || c.Role == Role) &&
                (string.IsNullOrEmpty(Department) || c.Department == Department);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public interface IEmployeeRepository
    {
        int CountEmployees(EmployeeQuery query);

        List<EmployeeView> GetEmployees(EmployeeQuery query);

        Employee GetEmployee(int employeeId);
        
        int AddEmployee(Employee employee);

        int UpdateEmployee(Employee employee);

        int DeleteEmployee(Employee employee);
    }

    public class EmployeeRepository : IEmployeeRepository
    {
        public int CountEmployees(EmployeeQuery query)
        {
            int total = 0;
            var predicate = query.BuildExpression();

            try
            {
                using (var context = new IndependienteEntities())
                {

                    total = context.EmployeeView.Count(predicate);
                }
            }
            catch (DbEntityValidationException ex)
            {

            }
            catch (EntityException ex)
            {

            }

            return total;
        }

        public List<EmployeeView> GetEmployees(EmployeeQuery query)
        {
            List<EmployeeView> employees = new List<EmployeeView>();

            var predicate = query.BuildExpression();

            try
            {
                using (var context = new IndependienteEntities())
                {
                    var employeeForSearch = context.EmployeeView
                        .Where(predicate)
                        .OrderBy(x => x.HireDate)
                        .Skip((query.PageNumber - 1) * query.PageSize)
                        .Take(query.PageSize)
                        .ToList();

                    if (employeeForSearch != null)
                    {
                        employees = employeeForSearch;
                    }
                }
            }
            catch (DbUpdateException ex)
            {
            }
            catch (EntityException ex)
            {
            }

            return employees;
        }

        public Employee GetEmployee(int employeeId)
        {
            Employee employee = new Employee();

            try
            {
                using (var context = new IndependienteEntities())
                {
                    var employeeForSearch = context.Employee.Find(employeeId);

                    if (employeeForSearch != null)
                    {
                        employee = employeeForSearch;
                        
                        employee.PersonalData = employeeForSearch.PersonalData;
                        employee.AddressData = employeeForSearch.AddressData;
                        employee.User = employeeForSearch.User;
                        
                        employee.Client = employeeForSearch.Client != null ? employeeForSearch.Client.ToList() : new List<Client>();
                        employee.Payment = employeeForSearch.Payment != null ? employeeForSearch.Payment.ToList() : new List<Payment>();
                    }
                }
            }
            catch (DbUpdateException ex)
            {
            }
            catch (EntityException ex)
            {
            }

            return employee;
        }

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

        public int UpdateEmployee(Employee employee)
        {
            int affectedRows = 0;

            try
            {
                using (var context = new IndependienteEntities())
                {
                    var existingEmployee = context.Employee
                        .Include(e => e.PersonalData)
                        .Include(e => e.AddressData)
                        .Include(e => e.User)
                        .FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);

                    if (existingEmployee != null)
                    {
                        existingEmployee.NSS = employee.NSS;
                        existingEmployee.HireDate = employee.HireDate;
                        existingEmployee.Department = employee.Department;
                        existingEmployee.Status = employee.Status;

                        context.Entry(existingEmployee.PersonalData).CurrentValues.SetValues(employee.PersonalData);
                        context.Entry(existingEmployee.AddressData).CurrentValues.SetValues(employee.AddressData);
                        context.Entry(existingEmployee.User).CurrentValues.SetValues(employee.User);

                        affectedRows = context.SaveChanges();
                    }
                }
            }
            catch (DbUpdateException ex)
            {
            }
            catch (EntityException ex)
            {
            }

            return affectedRows;
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
                            var employeeForDelete = context.Employee
                                .Include(e => e.PersonalData)
                                .Include(e => e.AddressData)
                                .Include(e => e.User)
                                .FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);

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
