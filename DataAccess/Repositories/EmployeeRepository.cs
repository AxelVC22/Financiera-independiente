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
            catch (Exception ex)
            {
                throw DbExceptionHandler.Handle(ex);
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
            catch (Exception ex)
            {
                throw DbExceptionHandler.Handle(ex);
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
            catch (Exception ex)
            {
                throw DbExceptionHandler.Handle(ex);
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
                            if (employee.User != null)
                            {
                                context.User.Add(employee.User);
                            }

                            if (employee.AddressData != null)
                            {
                                context.AddressData.Add(employee.AddressData);
                            }

                            if (employee.PersonalData != null)
                            {
                                context.PersonalData.Add(employee.PersonalData);
                            }

                            var newEmployee = new Employee
                            {
                                NSS = employee.NSS,
                                HireDate = employee.HireDate,
                                Department = employee.Department,
                                Status = employee.Status,
                                User = employee.User,
                                AddressData = employee.AddressData,
                                PersonalData = employee.PersonalData
                            };

                            context.Employee.Add(newEmployee);
                            context.SaveChanges();
                            transaction.Commit();

                            id = newEmployee.EmployeeId;
                        }
                        catch (DbEntityValidationException ex)
                        {
                            foreach (var validationErrors in ex.EntityValidationErrors)
                            {
                                foreach (var validationError in validationErrors.ValidationErrors)
                                {
                                    Console.WriteLine($"Entidad: {validationErrors.Entry.Entity.GetType().Name}, " +
                                                      $"Propiedad: {validationError.PropertyName}, " +
                                                      $"Error: {validationError.ErrorMessage}");
                                }
                            }

                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw DbExceptionHandler.Handle(ex);
            }

            return id;
        }

        public int UpdateEmployee(Employee employee)
        {
            int affectedRows = 0;

            using (var context = new IndependienteEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var existingEmployee = context.Employee
                            .Include(e => e.PersonalData)
                            .Include(e => e.AddressData)
                            .Include(e => e.User)
                            .FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);

                        if (existingEmployee == null)
                            throw new InvalidOperationException("Empleado no encontrado.");

                        if (employee.PersonalData != null)
                            employee.PersonalData.PersonalDataId = existingEmployee.PersonalDataId;

                        string newRfc = employee.PersonalData?.RFC?.Trim().ToUpperInvariant() ?? "";
                        string oldRfc = existingEmployee.PersonalData?.RFC?.Trim().ToUpperInvariant() ?? "";

                        if (!newRfc.Equals(oldRfc, StringComparison.OrdinalIgnoreCase))
                        {
                            bool rfcExists = context.PersonalData
                                .Any(p => p.RFC.ToUpper() == newRfc && p.PersonalDataId != existingEmployee.PersonalDataId);

                            if (rfcExists)
                                throw new InvalidOperationException("El RFC ya está registrado a otro empleado.");
                        }

                        existingEmployee.NSS = employee.NSS;
                        existingEmployee.HireDate = employee.HireDate;
                        existingEmployee.Department = employee.Department;
                        existingEmployee.Status = employee.Status;

                        var pd = existingEmployee.PersonalData;
                        var newPd = employee.PersonalData;
                        if (pd != null && newPd != null)
                        {
                            pd.Name = newPd.Name;
                            pd.Lastname = newPd.Lastname;
                            pd.Surname = newPd.Surname;
                            pd.BirthDate = newPd.BirthDate;
                            pd.RFC = newPd.RFC;
                            pd.CURP = newPd.CURP;
                            pd.PhoneNumber = newPd.PhoneNumber;
                            pd.AlternativePhoneNumber = newPd.AlternativePhoneNumber;
                            pd.Email = newPd.Email;
                        }

                        var ad = existingEmployee.AddressData;
                        var newAd = employee.AddressData;
                        if (ad != null && newAd != null)
                        {
                            ad.Street = newAd.Street;
                            ad.City = newAd.City;
                            ad.State = newAd.State;
                            ad.Neighborhood = newAd.Neighborhood;
                        }

                        var u = existingEmployee.User;
                        var newU = employee.User;
                        if (u != null && newU != null)
                        {
                            u.Role = newU.Role;

                            if (!string.IsNullOrWhiteSpace(newU.Password))
                            {
                                u.Password = newU.Password;
                            }
                        }

                        affectedRows = context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        throw DbExceptionHandler.Handle(ex);

                    }
                }
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
                           
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw DbExceptionHandler.Handle(ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw DbExceptionHandler.Handle(ex);
            }

            return result;
        }
    }
}
