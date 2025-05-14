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

}
