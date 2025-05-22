using Independiente.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.Services.Mappers
{
    public static class EmployeeMapper
    {
        public static DataAccess.Employee ToDataModel(this Model.Employee source)
        {
            DataAccess.Employee employee = new DataAccess.Employee();

            if (source != null)
            {
                employee = new DataAccess.Employee
                {
                    EmployeeId = source.EmployeeId,
                    NSS = source.NSS,
                    HireDate = source.HireDate,
                    Department = source.Department,
                    Status = source.Status.ToString(),
                    PersonalData = source?.PersonalData != null ? PersonalDataMapper.ToDataModel(source.PersonalData) : null,
                    AddressData = source?.AddressData != null ? AddressDataMapper.ToDataModel(source.AddressData) : null,
                    //User = source?.User != null ? UserMapper.ToDataModel(source.User) : null, TODO
                };
            }
            return employee;
        }

        public static Model.Employee ToViewModel(this DataAccess.Employee source)
        {
            Model.Employee employee = new Model.Employee();

            if (source != null)
            {
                employee = new Model.Employee
                {
                    EmployeeId = source.EmployeeId,
                    NSS = source.NSS,
                    HireDate = source.HireDate,
                    Department = source.Department,
                    Status = (EmployeeStates)Enum.Parse(typeof(EmployeeStates), source.Status),
                    PersonalData = source?.PersonalData != null ? PersonalDataMapper.ToViewModel(source.PersonalData) : null,
                    AddressData = source?.AddressData != null ? AddressDataMapper.ToViewModel(source.AddressData) : null,
                    //User = source?.User != null ? UserMapper.ToDataModel(source.User) : null, TODO
                };
            }
            return employee;
        }
        public static Model.Employee ToViewModel(this DataAccess.EmployeeView source)
        {
            Model.Employee employee = new Model.Employee();

            if (source != null)
            {
                employee = new Model.Employee
                {
                    EmployeeId = source.EmployeeId,
                    NSS = source.NSS,
                    HireDate = source.HireDate,
                    Department = source.Department,
                    Status = (EmployeeStates)Enum.Parse(typeof(EmployeeStates), source.Status),
                    PersonalData = new Independiente.Model.PersonalData
                    {
                        Name = source.EmployeeName,
                        LastName = source.Lastname,
                        Surname = source.Surname,
                        BirthDate = source.BirthDate,
                        RFC = source.RFC,
                        CURP = source.CURP,
                        PhoneNumber = source.PhoneNumber,
                        Email = source.Email
                    },
                    AddressData = new Independiente.Model.AddressData
                    {
                        Street = source.Street,
                        State = source.State,
                        NeighborHood = source.Neighborhood
                    },
                    User = new Independiente.Model.User
                    {
                        Role = source.Role,
                        Password = source.Password,                        
                    }
                };
            }
            return employee;
        }
    }
}
