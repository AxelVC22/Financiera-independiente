using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.Services.Mappers
{
    public static class ClientMapper
    {
        public static DataAccess.Client ToDataModel(this Model.Client source)
        {
            DataAccess.Client client = new DataAccess.Client();

            if (source != null)
            {
                client = new DataAccess.Client
                {
                    PersonalData = source?.PersonalData != null ? PersonalDataMapper.ToDataModel(source.PersonalData) : null,
                    AddressData = source?.AddressData != null ? AddressDataMapper.ToDataModel(source.AddressData) : null,
                    Reference = source?.SecondReference != null ? ReferenceMapper.ToDataModel(source.SecondReference) : null,
                    Reference1 = source?.FirstReference != null ? ReferenceMapper.ToDataModel(source.FirstReference) : null,
                    Account = source?.DepositAccount != null ? AccountMapper.ToDataModel(source.DepositAccount) : null,
                    Account1 = source?.PaymentAccount != null ? AccountMapper.ToDataModel(source.PaymentAccount) : null,
                    WorkCenter = source?.WorkCenter != null ? WorkCenterMapper.ToDataModel(source.WorkCenter) : null,
                    EmployeeId = source.Employee,
                };
            }
            return client;
        }

        public static Model.Client ToViewModel(this DataAccess.Client source)
        {
            Model.Client client = new Model.Client();

            if (source != null)
            {
                return new Model.Client
                {
                    ClientId = source.ClientId,
                    PersonalData = PersonalDataMapper.ToViewModel(source.PersonalData),
                    AddressData = AddressDataMapper.ToViewModel(source.AddressData),
                };
            }

            return client;
        }
    }

    public static class PersonalDataMapper
    {
        public static DataAccess.PersonalData ToDataModel(Independiente.Model.PersonalData source)
        {
            DataAccess.PersonalData personalData = new DataAccess.PersonalData();

            if (source != null)
            {
                personalData = new DataAccess.PersonalData
                {
                    Name = source.Name,
                    Lastname = source.LastName,
                    Surname = source.Surname,
                    RFC = source.RFC,
                    PhoneNumber = source.PhoneNumber,
                    AlternativePhoneNumber = source.AlternativePhoneNumber,
                    Email = source.Email,
                    BirthDate = (DateTime)source.BirthDate,
                    CURP = source.CURP
                };
            }
            return personalData;
        }

        public static Model.PersonalData ToViewModel(Independiente.DataAccess.PersonalData source)
        {
            Model.PersonalData personalData = new Model.PersonalData();

            if (source != null)
            {
                personalData = new Model.PersonalData
                {
                    Name = source.Name,
                    LastName = source.Lastname,
                    Surname = source.Surname,
                    RFC = source.RFC,
                    PhoneNumber = source.PhoneNumber,
                    Email = source.Email,
                    BirthDate = (DateTime)source.BirthDate,
                    CURP = source.CURP
                };
            }

            return personalData;

        }
    }

    public static class AddressDataMapper
    {
        public static DataAccess.AddressData ToDataModel(Independiente.Model.AddressData source)
        {
            DataAccess.AddressData addressData = new DataAccess.AddressData();

            if (source != null)
            {
                addressData = new DataAccess.AddressData
                {
                    Street = source.Street,
                    City = source.City,
                    State = source.State,
                    Neighborhood = source.NeighborHood
                };
            }
            return addressData;
        }

        public static Model.AddressData ToViewModel(Independiente.DataAccess.AddressData source)
        {
            Model.AddressData addressData = new Model.AddressData();

            if (source != null)
            {
                addressData = new Model.AddressData
                {
                    Street = source.Street,
                    City = source.City,
                    State = source.State,
                    NeighborHood = source.Neighborhood
                };
            }
            return addressData;
        }
    }

    public static class ReferenceMapper
    {
        public static DataAccess.Reference ToDataModel(Independiente.Model.Reference source)
        {
            DataAccess.Reference reference = new DataAccess.Reference();
            if (source != null)
            {
                reference = new DataAccess.Reference
                {
                    Name = source.Name,
                    FullLastName = source.FullLastName,
                    Relationship = source.Relationship,
                    PhoneNumber = source.PhoneNumber,
                    Email = source.Email
                };
            }
            return reference;
        }
        public static Model.Reference ToViewModel(Independiente.DataAccess.Reference source)
        {
            Model.Reference reference = new Model.Reference();
            if (source != null)
            {
                reference = new Model.Reference
                {
                    Name = source.Name,
                    FullLastName = source.FullLastName,
                    Relationship = source.Relationship,
                    PhoneNumber = source.PhoneNumber,
                    Email = source.Email
                };
            }
            return reference;
        }
    }

    public static class AccountMapper
    {
        public static DataAccess.Account ToDataModel(Independiente.Model.Account source)
        {
            DataAccess.Account account = new DataAccess.Account();
            if (source != null)
            {
                account = new DataAccess.Account
                {
                    CLABE = source.CLABE,
                    BankId = source.Bank,
                };
            }
            return account;
        }
        public static Model.Account ToViewModel(Independiente.DataAccess.Account source)
        {
            Model.Account account = new Model.Account();
            if (source != null)
            {
                account = new Model.Account
                {
                    CLABE = source.CLABE,
                    Bank = source.BankId,
                };
            }
            return account;
        }
    }

    public static class WorkCenterMapper
    {
        public static DataAccess.WorkCenter ToDataModel(Independiente.Model.WorkCenter source)
        {
            DataAccess.WorkCenter workCenter = new DataAccess.WorkCenter();
            if (source != null)
            {
                workCenter = new DataAccess.WorkCenter
                {
                    Name = source.Name,
                    Role = source.Role,
                    HiringDate = (DateTime)source.HiringDate,
                    MontlyIncome = (Decimal)source.MontlyIncome,
                };
            }
            return workCenter;
        }
        public static Model.WorkCenter ToViewModel(Independiente.DataAccess.WorkCenter source)
        {
            Model.WorkCenter workCenter = new Model.WorkCenter();
            if (source != null)
            {
                workCenter = new Model.WorkCenter
                {
                    Name = source.Name,
                    Role = source.Role,
                    HiringDate = (DateTime)source.HiringDate,
                    MontlyIncome = (Decimal)source.MontlyIncome,
                };
            }
            return workCenter;
        }
    }
}
