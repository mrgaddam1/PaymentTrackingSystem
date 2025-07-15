using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentTrackingSystem.Common.Helpers.Extensions;
using PaymentTrackingSystem.Core.Data.Models;
using PaymentTrackingSystem.Core.Helpers;
using PaymentTrackingSystem.Shared;
using PaymentTrackingSystem.Web.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Web.Infrastructure.Implementation
{
    public class PaymentInterestManager : IPaymentInterestManager
    {
        private readonly ILogger<PaymentInterestManager> logger;
        private readonly IMapper mapper;
        private PTSContext DbContext { get; set; }

        public PaymentInterestManager(PTSContext _DbContext, IMapper _mapper)
        {
            DbContext = _DbContext;
            mapper = _mapper;
        }
        public async Task<List<ClientPaymentInterestViewModel>> GetAll()
        {
            try
            {
                var interestPaymentData = await (from cpi in DbContext.ClientInterestPayments.AsNoTracking()
                                                 join cp in DbContext.ClientPayments.AsNoTracking()
                                                 on cpi.PaymentId equals cp.PaymentId
                                                 join c in DbContext.Clients.AsNoTracking()
                                                 on cpi.ClientId equals c.ClientId
                                                 where cpi.IsDeleted == false
                                                 select new ClientPaymentInterestViewModel
                                                 {
                                                     InterestId = cpi.InterestId,
                                                     ClientId = cpi.ClientId,
                                                     ClientName = c.FirstName + " " + c.LastName,
                                                     PaymentId = cp.PaymentId,
                                                     Amount = cp.Amount,
                                                     InterestRate = cp.InterestRate,
                                                     InterestAmount = cpi.InterestAmount,
                                                     InterestPaidDate = cpi.InterestPaidDate,
                                                     IsitPaidForTheCurrentMonth = cpi.IsitPaidForTheCurrentMonth,
                                                     IsitDeleted = cpi.IsDeleted,

                                                 }).OrderBy(x => x.InterestId).ToListAsync();

                return interestPaymentData;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "An error occured while processing details...");
                return new List<ClientPaymentInterestViewModel>();
            }
        }
        public async Task<ClientPaymentInterestViewModel> GetAllDetailsById(int interestId)
        {
            var interestPaymentData = new ClientPaymentInterestViewModel();
            try
            {


                interestPaymentData = await (from cpi in DbContext.ClientInterestPayments.AsNoTracking()
                                             join cp in DbContext.ClientPayments.AsNoTracking()
                                             on cpi.PaymentId equals cp.PaymentId
                                             join c in DbContext.Clients.AsNoTracking()
                                             on cpi.ClientId equals c.ClientId
                                             where cpi.InterestId == interestId
                                             select new ClientPaymentInterestViewModel
                                             {
                                                 InterestId = cpi.InterestId,
                                                 ClientId = cpi.ClientId,
                                                 ClientName = c.FirstName + " " + c.LastName,
                                                 PaymentId = cp.PaymentId,
                                                 Amount = cp.Amount,
                                                 InterestRate = cp.InterestRate,
                                                 InterestAmount = cpi.InterestAmount,
                                                 InterestPaidDate = cpi.InterestPaidDate,
                                                 IsitPaidForTheCurrentMonth = cpi.IsitPaidForTheCurrentMonth,
                                                 IsitDeleted = cpi.IsDeleted,

                                             }).OrderBy(x => x.InterestId).FirstOrDefaultAsync();


                return null;


            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "An error occured while processing details...");
                return new ClientPaymentInterestViewModel();
            }
        }
        public async Task<bool> Add(ClientPaymentInterestViewModel clientPaymentInterestViewModel)
        {
            bool isSuccess = false;
            try
            {
                var interestData = mapper.Map<ClientInterestPayment>(clientPaymentInterestViewModel);
                if (interestData != null)
                {
                    interestData.UserId = 1;
                    interestData.IsDeleted = false;
                    interestData.CreatedDate = DateTime.UtcNow;
                    DbContext.ClientInterestPayments.Add(interestData);
                    await DbContext.SaveChangesAsync();
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "An error occured while processing details...");
            }
            return isSuccess;
        }
        public async Task<bool> Update(ClientPaymentInterestViewModel clientPaymentInterestViewModel)
        {
            bool isSuccess = false;
            try
            {
                var interestPaymentData = await (DbContext
                                          .ClientInterestPayments
                                          .AsNoTracking()
                                          .Where(x => x.InterestId == clientPaymentInterestViewModel.InterestId)
                                          .OrderBy(x => x.CreatedDate)
                                          .SingleOrDefaultAsync());

                if (interestPaymentData != null)
                {
                    interestPaymentData.InterestPaidDate = clientPaymentInterestViewModel.InterestPaidDate;
                    interestPaymentData.UserId = 1;
                    interestPaymentData.ModifiedDate = DateTime.UtcNow;
                    DbContext.ClientInterestPayments.Update(interestPaymentData);
                    await DbContext.SaveChangesAsync();
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "An error occured while processing details...");
            }
            return isSuccess;
        }

        public async Task<bool> Delete(int interestId)
        {
            bool isDeleted = false;
            try
            {
                var interestPaymentData = await (DbContext
                                                .ClientInterestPayments
                                                .AsNoTracking()
                                                .Where(x => x.InterestId == interestId)
                                                .OrderBy(x => x.CreatedDate)
                                                .SingleOrDefaultAsync());

                if (interestPaymentData != null)
                {
                    interestPaymentData.UserId = 1;
                    interestPaymentData.DeletedDate = DateTime.UtcNow;
                    interestPaymentData.IsDeleted = true;
                    DbContext.ClientInterestPayments.Update(interestPaymentData);
                    await DbContext.SaveChangesAsync();
                    isDeleted = true;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "An error occured while processing details...");
            }
            return isDeleted;
        }


        public async Task<List<ClientPaymentInterestPending>> GetAllClientsPaymentInterestsPendingDetais()
        {
            var paymentInterestPending = new List<ClientPaymentInterestPending>();

            try
            {
                var data = DataHelper.GetData(DbContext.Database.GetDbConnection(), "Up_Payments_GetAllClients_Payments_Interest_Pending", null);
                paymentInterestPending = ConvertDataTableToGenericList.ConvertDataTable<ClientPaymentInterestPending>(data).
                                   OrderByDescending(x => x.ClientName).ToList();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return paymentInterestPending;
        }

    }
}
