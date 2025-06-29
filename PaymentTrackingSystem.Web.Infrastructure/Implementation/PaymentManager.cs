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
    public class PaymentManager : IPaymentManager
    {
        private readonly ILogger<PaymentManager> logger;
        private readonly IMapper mapper;
        private PTSContext DbContext { get; set; }

        public PaymentManager(PTSContext _DbContext, IMapper _mapper)
        {
            DbContext = _DbContext;
            mapper = _mapper;
        }
        public async Task<List<ClientPaymentViewModel>> GetAllClientPayments()
        {
            var clientPaymentdata = new List<ClientPaymentViewModel>();
            try
            {
                clientPaymentdata = await (from p in DbContext.ClientPayments
                                           join c in DbContext.Clients on p.ClientId equals c.ClientId
                                           where p.IsDeleted == false
                                           select new ClientPaymentViewModel
                                           {
                                               ClientId = p.ClientId,
                                               PaymentId = p.PaymentId,
                                               FirstName = c.FirstName,
                                               LastName = c.LastName,
                                               Amount = p.Amount,
                                               AmountTransferedDate = p.AmountTransferedDate,
                                               InterestRate = p.InterestRate,
                                           }).ToListAsync();

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "An error occured while processing Get all Client Details request.");
                clientPaymentdata = new List<ClientPaymentViewModel>();
            }
            return clientPaymentdata;
        }

        public async Task<ClientPaymentViewModel> GetClientPaymentDetailsById(int paymentId)
        {
            var clientPaymentdata = new ClientPaymentViewModel();
            try
            {

                clientPaymentdata = await (from p in DbContext.ClientPayments
                                 join c in DbContext.Clients on p.ClientId equals c.ClientId
                                 where p.PaymentId == paymentId &&  p.IsDeleted == false
                                 select new ClientPaymentViewModel
                                 {
                                    ClientId = c.ClientId,
                                    PaymentId = p.PaymentId,
                                    FirstName = c.FirstName,
                                    LastName = c.LastName,
                                    Amount = p.Amount,
                                    AmountTransferedDate = p.AmountTransferedDate,
                                    InterestRate = p.InterestRate,
                                 }).SingleOrDefaultAsync();
          
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "An error occured while processing Client Details by Payment Id request.");
                clientPaymentdata = new ClientPaymentViewModel();
            }
            return clientPaymentdata;
        }

        public async Task<bool> Add(ClientPaymentViewModel clientPayment)
        {
            try
            {
                var clientPaymentData = mapper.Map<ClientPayment>(clientPayment);
                clientPaymentData.IsDeleted = false;
                clientPaymentData.CreatedDate = DateTime.UtcNow;
                clientPaymentData.UserId = 1;

                DbContext.ClientPayments.Add(clientPaymentData);
                await DbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "An error occured while processing Add request.");
                return false;
            }
        }        
        public async Task<bool> Update(ClientPaymentViewModel clientPaymentViewModel)
        {
            try
            {
                var clientPaymentData = await (DbContext.ClientPayments.FindAsync(clientPaymentViewModel.PaymentId));
                if (clientPaymentData == null)
                {
                    return false;
                }
                clientPaymentData.Amount = clientPaymentViewModel.Amount;
                clientPaymentData.InterestRate = clientPaymentViewModel.InterestRate;
                clientPaymentData.ModifiedDate = DateTime.Now;
                clientPaymentData.UserId = 1;               
                DbContext.ClientPayments.Update(clientPaymentData);
                await DbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "An error occured while processing Update request.");
                return false;
            }
        }
        public async Task<bool> Delete(int paymentId)
        {
            try
            {
                var clientPaymentData = await (DbContext.ClientPayments.FindAsync(paymentId));
                if (clientPaymentData == null)
                {
                    return false;
                }
                clientPaymentData.DeletedDate = DateTime.Now;
                clientPaymentData.UserId = 1;
                clientPaymentData.IsDeleted = true;
                DbContext.ClientPayments.Update(clientPaymentData);
                await DbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "An error occured while processing Delete request.");
                return false;
            }
        }



        }
}
