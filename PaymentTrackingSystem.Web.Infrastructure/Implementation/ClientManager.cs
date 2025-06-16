using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentTrackingSystem.Core.Data.Models;
using PaymentTrackingSystem.Shared;
using PaymentTrackingSystem.Web.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Web.Infrastructure.Implementation
{
    public class ClientManager : IClientManager
    {
        private readonly ILogger<ClientManager> logger;
        private readonly IMapper mapper;
        private PTSContext DbContext { get; set; }

        public ClientManager(PTSContext _DbContext, IMapper _mapper) 
        {
            DbContext = _DbContext;
            mapper = _mapper;
        }
        public async Task<bool> Add(ClientViewModel clientViewModel)
        {
            using var transaction = await DbContext.Database.BeginTransactionAsync();
            //var client = mapper.Map<Client>(clientViewModel);
            try
            {
                var client = new Client
                {
                    FirstName = clientViewModel.FirstName,
                    LastName = clientViewModel.LastName,
                    EmailId=clientViewModel.Email,
                    MobileNumber = clientViewModel.MobileNumber,
                    UserId = 1,
                    CreatedDate = DateTime.Now,
                };
               
                DbContext.Clients.Add(client);
                await DbContext.SaveChangesAsync();

                var clientAddress = new ClientAddress
                {
                    ClientId = client.ClientId,
                    AddressLine1 = clientViewModel.AddressLine1,
                    AddressLine2 = clientViewModel.AddressLine2,
                    City = clientViewModel.City,
                    Postcode = clientViewModel.Postcode,
                };                
                DbContext.ClientAddresses.Add(clientAddress);
                await DbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                logger.LogError(ex.Message, "An error occured while processing the request.");
                return false;
            }
        }

            
        public async Task<bool> Delete(int clientId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ClientViewModel>> GetAllClients()
        {
            var clientData = new List<ClientViewModel>();
            try
            {
                clientData = await (from c in DbContext.Clients
                                    join ca in DbContext.ClientAddresses
                                    on c.ClientId equals ca.ClientId
                                    select new ClientViewModel
                                    {
                                        ClientId = c.ClientId,
                                        FirstName = c.FirstName,
                                        LastName = c.LastName,
                                        Email = c.EmailId,
                                        MobileNumber = c.MobileNumber,
                                        AddressLine1 = ca.AddressLine1,
                                        AddressLine2 = ca.AddressLine2,
                                        City = ca.City,
                                        Postcode = ca.Postcode,
                                    }).OrderBy(x => x.FirstName).ToListAsync();
            }
            catch(Exception ex)  
            {
                logger.LogError(ex.Message, "An error occured while processing the request.");
                clientData = null;
            }            
            return clientData;
        }

        public async Task<ClientViewModel> GetClientDetailsById(int clientId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(ClientViewModel clientViewModel)
        {
            throw new NotImplementedException();
        }
    }
}
