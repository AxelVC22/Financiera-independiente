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
                                EmployeeId = client.Employee.EmployeeId,
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
            }
            catch (EntityException ex)
            {
            }

            return id;
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
