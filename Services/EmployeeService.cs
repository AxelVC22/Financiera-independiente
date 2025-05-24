using Independiente.DataAccess.Repositories;
using Independiente.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Independiente.Services.Mappers;

namespace Independiente.Services
{
    
    public interface IEmployeeService
    {
        int CountEmployees(EmployeeQuery query);

        List<Independiente.Model.Employee> GetEmployees(EmployeeQuery query);

        Independiente.Model.Employee GetEmployee(int employeeId);

        int AddEmployee(Independiente.Model.Employee employee);

        int UpdateEmployee(Independiente.Model.Employee employee);

        int DeleteEmployee(Independiente.Model.Employee employee);
    }

    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        private bool ValidateQuery(EmployeeQuery query)
        {
            if (!string.IsNullOrEmpty(query.RFC))
            {
                FieldValidator.IsValidRFC(query.RFC);
            }
            return true;
        }

        public int CountEmployees(EmployeeQuery query)
        {
            int total = 0;
            if (ValidateQuery(query))
            {
                total = _employeeRepository.CountEmployees(query);
            }
            return total;
        }

        public List<Independiente.Model.Employee> GetEmployees(EmployeeQuery query)
        {
            List<Independiente.DataAccess.EmployeeView> employeeList = new List<Independiente.DataAccess.EmployeeView>();

            List<Independiente.Model.Employee> employees1 = new List<Model.Employee>();

            if (ValidateQuery(query))
            {
                employeeList = _employeeRepository.GetEmployees(query);

                foreach (var c in employeeList)
                {
                    employees1.Add(EmployeeMapper.ToViewModel(c));
                }
            }
            return employees1;
        }

        public Independiente.Model.Employee GetEmployee(int employeeId)
        {
            Independiente.Model.Employee employee = null;

            if (employeeId > 0)
            {
                var application = _employeeRepository.GetEmployee(employeeId);
                employee = EmployeeMapper.ToViewModel(application);
            }
            return employee;
        }

        public int AddEmployee(Independiente.Model.Employee employee)
        {
            int id = 0;

            if (employee != null)
            {
                id = _employeeRepository.AddEmployee(EmployeeMapper.ToDataModel(employee));
            }
            return id;
        }

        public int UpdateEmployee(Independiente.Model.Employee employee)
        {
            int affectedRows = 0;

            if (employee != null)
            {
                affectedRows = _employeeRepository.UpdateEmployee(EmployeeMapper.ToDataModel(employee));
            }
            return affectedRows;
        }

        public int DeleteEmployee(Independiente.Model.Employee employee)
        {
            int result = 0;

            if (employee != null)
            {
                result = _employeeRepository.DeleteEmployee(EmployeeMapper.ToDataModel(employee));
            }
            return result;
        }
    }
}
