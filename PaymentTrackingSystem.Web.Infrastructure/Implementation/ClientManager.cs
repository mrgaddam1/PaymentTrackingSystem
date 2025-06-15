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
        private PTSContext DbContext { get; set; }

        public ClientManager(PTSContext _DbContext) 
        {
            DbContext = _DbContext;
        }
        public async Task<bool> Add()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete()
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

        public async Task<ClientViewModel> GetClientDetailsById()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update()
        {
            throw new NotImplementedException();
        }
    }
}
