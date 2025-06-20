using Independiente.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.DataAccess.Repositories
{
    public interface IClientRepository
    {
        int AddClient(Client client);

        int DeleteClient(Client client);               

        (int result, Model.PersonalData personalData) UpdatePersonalData(Model.PersonalData personalData);

        (int result, Model.AddressData addressData) UpdateAddressData(Model.AddressData addressData);

        (int result, Model.Account account) UpdateAccount(Model.Account account);

        (int resultRef1, Model.Reference updatedRef1, int resultRef2, Model.Reference updatedRef2, int resultWC, Model.WorkCenter updatedWorkCenter) UpdateReferencesAndWorkCenter(Model.Reference ref1, Model.Reference ref2, Model.WorkCenter workCenter);

    }

    public class ClientRepository : IClientRepository
    {
        public int AddClient(Client client)
        {
            int id = 0;

            if (client == null)
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
                            var newClient = new Client
                            {
                                Account = client.Account,
                                WorkCenter = client.WorkCenter,
                                PersonalData = client.PersonalData,
                                AddressData = client.AddressData,
                                EmployeeId = client.EmployeeId,
                                Reference = client.Reference,
                                Reference1 = client.Reference1,
                                Account1 = client.Account1
                                
                            };
                            context.Client.Add(newClient);
                            context.SaveChanges();
                            transaction.Commit();
                            id = newClient.ClientId;
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
                return -1;
            }
            catch (EntityException ex)
            {
                return -1;
            }

            return id;
        }

        public (int result, Model.PersonalData personalData) UpdatePersonalData(Model.PersonalData personalData)
        {
            int result = 0;
            Model.PersonalData updatedPersonalData = null;

            try
            {
                using (var context = new IndependienteEntities())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var personalDataForUpdate = context.PersonalData.Find(personalData.PersonalDataId);
                            if (personalDataForUpdate != null)
                            {
                                personalDataForUpdate.Name = personalData.Name;
                                personalDataForUpdate.Lastname = personalData.LastName;
                                personalDataForUpdate.Surname = personalData.Surname;
                                personalDataForUpdate.BirthDate = (DateTime)personalData.BirthDate;
                                personalDataForUpdate.RFC = personalData.RFC;
                                personalDataForUpdate.CURP = personalData.CURP;
                                personalDataForUpdate.PhoneNumber = personalData.PhoneNumber;
                                personalDataForUpdate.AlternativePhoneNumber = personalData.AlternativePhoneNumber;
                                personalDataForUpdate.Email = personalData.Email;

                                result = context.SaveChanges();
                                transaction.Commit();

                                updatedPersonalData = new Model.PersonalData
                                {
                                    PersonalDataId = personalDataForUpdate.PersonalDataId,
                                    Name = personalDataForUpdate.Name,
                                    LastName = personalDataForUpdate.Lastname,
                                    Surname = personalDataForUpdate.Surname,
                                    BirthDate = personalDataForUpdate.BirthDate,
                                    RFC = personalDataForUpdate.RFC,
                                    CURP = personalDataForUpdate.CURP,
                                    PhoneNumber = personalDataForUpdate.PhoneNumber,
                                    AlternativePhoneNumber = personalDataForUpdate.AlternativePhoneNumber,
                                    Email = personalDataForUpdate.Email
                                };
                            }
                            else
                            {
                                return (0, null);
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
                return (-1, null);
            }
            catch (EntityException ex)
            {
                return (-1, null);
            }

            return (result, updatedPersonalData);
        }

        public (int result, Model.AddressData addressData) UpdateAddressData(Model.AddressData addressData)
        {
            int result = 0;
            Model.AddressData updatedAddressData = null;
            try
            {
                using (var context = new IndependienteEntities())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var addressDataForUpdate = context.AddressData.Find(addressData.AddressDataId);
                            if (addressDataForUpdate != null)
                            {
                                addressDataForUpdate.Street = addressData.Street;
                                addressDataForUpdate.State = addressData.State;
                                addressDataForUpdate.Neighborhood = addressData.NeighborHood;
                                addressDataForUpdate.City = addressData.City;
                                result = context.SaveChanges();
                                transaction.Commit();
                                updatedAddressData = new Model.AddressData
                                {
                                    AddressDataId = addressDataForUpdate.AddressDataId,
                                    Street = addressDataForUpdate.Street,
                                    State = addressDataForUpdate.State,
                                    NeighborHood = addressDataForUpdate.Neighborhood,
                                    City = addressDataForUpdate.City                                  
                                };
                            }
                            else
                            {
                                return (0, null);
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
                return (-1, null);
            }
            catch (EntityException ex)
            {
                return (-1, null);
            }
            return (result, updatedAddressData);
        }

        public (int result, Model.Account account) UpdateAccount(Model.Account account)
        {
            int result = 0;
            Model.Account updatedAccount = null;
            try
            {
                using (var context = new IndependienteEntities())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var accountForUpdate = context.Account.Find(account.AccountId);
                            if (accountForUpdate != null)
                            {
                                accountForUpdate.CLABE = account.CLABE;
                                accountForUpdate.BankId = account.Bank;
                                result = context.SaveChanges();
                                transaction.Commit();
                                updatedAccount = new Model.Account
                                {
                                    AccountId = accountForUpdate.AccountId,
                                    Bank = accountForUpdate.BankId,
                                    CLABE = accountForUpdate.CLABE
                                };
                            }
                            else
                            {
                                return (0, null);
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
                return (-1, null);
            }
            catch (EntityException ex)
            {
                return (-1, null);
            }
            return (result, updatedAccount);
        }

        public (int resultRef1, Model.Reference updatedRef1, int resultRef2, Model.Reference updatedRef2, int resultWC, Model.WorkCenter updatedWorkCenter) UpdateReferencesAndWorkCenter(Model.Reference ref1, Model.Reference ref2, Model.WorkCenter workCenter)
        {
            int resultRef1 = 0, resultRef2 = 0, resultWC = 0;
            Model.Reference updatedRef1 = null, updatedRef2 = null;
            Model.WorkCenter updatedWC = null;

            try
            {
                using (var context = new IndependienteEntities())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var ref1Db = context.Reference.Find(ref1.ReferenceId);
                            if (ref1Db != null)
                            {
                                ref1Db.Name = ref1.Name;
                                ref1Db.FullLastName = ref1.FullLastName;
                                ref1Db.PhoneNumber = ref1.PhoneNumber;
                                ref1Db.Relationship = ref1.Relationship;
                                ref1Db.Email = ref1.Email;

                                resultRef1 = context.SaveChanges();

                                updatedRef1 = new Model.Reference
                                {
                                    ReferenceId = ref1Db.ReferenceId,
                                    Name = ref1Db.Name,
                                    FullLastName = ref1Db.FullLastName,
                                    PhoneNumber = ref1Db.PhoneNumber,
                                    Relationship = ref1Db.Relationship,
                                    Email = ref1Db.Email
                                };
                            }

                            var ref2Db = context.Reference.Find(ref2.ReferenceId);
                            if (ref2Db != null)
                            {
                                ref2Db.Name = ref2.Name;
                                ref2Db.FullLastName = ref2.FullLastName;
                                ref2Db.PhoneNumber = ref2.PhoneNumber;
                                ref2Db.Relationship = ref2.Relationship;
                                ref2Db.Email = ref2.Email;

                                resultRef2 = context.SaveChanges();

                                updatedRef2 = new Model.Reference
                                {
                                    ReferenceId = ref2Db.ReferenceId,
                                    Name = ref2Db.Name,
                                    FullLastName = ref2Db.FullLastName,
                                    PhoneNumber = ref2Db.PhoneNumber,
                                    Relationship = ref2Db.Relationship,
                                    Email = ref2Db.Email
                                };
                            }

                            var wcDb = context.WorkCenter.Find(workCenter.WorkCenterId);
                            if (wcDb != null)
                            {
                                wcDb.Name = workCenter.Name;
                                wcDb.Role = workCenter.Role;
                                wcDb.MontlyIncome = (Decimal)workCenter.MontlyIncome;
                                wcDb.HiringDate = (DateTime)workCenter.HiringDate;

                                resultWC = context.SaveChanges();

                                updatedWC = new Model.WorkCenter
                                {
                                    WorkCenterId = wcDb.WorkCenterId,
                                    Name = wcDb.Name,
                                    Role = wcDb.Role,
                                    MontlyIncome = wcDb.MontlyIncome,
                                    HiringDate = wcDb.HiringDate
                                };
                            }

                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (DbUpdateException)
            {
                return (-1, null, -1, null, -1, null);
            }
            catch (EntityException)
            {
                return (-1, null, -1, null, -1, null);
            }

            return (resultRef1, updatedRef1, resultRef2, updatedRef2, resultWC, updatedWC);
        }


        //Only for tests
        public int DeleteClient(Client client)
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
                            var clientForDelete = context.Client.Find(client.ClientId);

                            if (clientForDelete != null)
                            {
                                var personalData = clientForDelete.PersonalData;
                                var addressData = clientForDelete.AddressData;
                                var workCenter = clientForDelete.WorkCenter;
                                var firstReference = clientForDelete.Reference;
                                var secondReference = clientForDelete.Reference1;
                                var depositAccount = clientForDelete.Account;
                                var paymentAccount = clientForDelete.Account1;

                                context.Client.Remove(clientForDelete);
                                context.PersonalData.Remove(personalData);
                                context.AddressData.Remove(addressData);
                                context.WorkCenter.Remove(workCenter);
                                context.Account.Remove(depositAccount);
                                context.Account.Remove(paymentAccount);
                                context.Reference.Remove(firstReference);
                                context.Reference.Remove(secondReference);

                                result = context.SaveChanges();

                                transaction.Commit();
                            }
                            else
                            {
                              //  throw new Exception("Employee not found.");
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
