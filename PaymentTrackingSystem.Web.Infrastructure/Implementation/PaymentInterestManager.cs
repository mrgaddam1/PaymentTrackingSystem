using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using PaymentTrackingSystem.Common.Helpers.Extensions;
using PaymentTrackingSystem.Common.Utils.ClientPaymentInterestMessages;
using PaymentTrackingSystem.Core.Data.Models;
using PaymentTrackingSystem.Core.Helpers;
using PaymentTrackingSystem.Shared;
using PaymentTrackingSystem.Web.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
                                                     Amount = cp.Amount.Value,
                                                     InterestRate = cp.InterestRate.Value,
                                                     InterestAmount = cpi.InterestAmount.Value,
                                                     InterestPaidDate = cpi.InterestPaidDate,
                                                     AmountTransferedDate = cp.AmountTransferedDate,
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
                                                 Amount = cp.Amount.Value,
                                                 InterestRate = cp.InterestRate.Value,
                                                 InterestAmount = cpi.InterestAmount.Value,
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
        public async Task<string> Add(ClientPaymentInterestViewModel clientPaymentInterestViewModel)
        {
            string message = "";
            bool isRecordAlreadyExists;
           
            using var transaction = await DbContext.Database.BeginTransactionAsync();          
            try
            {
                var interestData = mapper.Map<ClientInterestPayment>(clientPaymentInterestViewModel);

                //check for duplicates....
                isRecordAlreadyExists =  CheckPaymentInterestDataExistsOrNot(clientPaymentInterestViewModel);

                if (!isRecordAlreadyExists)
                {

                    //check number of days is should be 30 days or 60 days
                    DateTime today = DateTime.Today;
                    var paymentDueData = await DbContext.PaymentDueDates
                                                        .Where(x => x.PaymentId == interestData.PaymentId
                                                        && x.ClientId == interestData.ClientId)
                                                        .SingleOrDefaultAsync();

                    TimeSpan diffenceInDays;
            
                    if (paymentDueData.DueDate.Value.Date > clientPaymentInterestViewModel.InterestPaidDate.Value.Date)
                    {
                        diffenceInDays = (paymentDueData.DueDate.Value.Date - clientPaymentInterestViewModel.InterestPaidDate.Value.Date);

                        if (diffenceInDays.TotalDays >= 30)
                        {
                           if (interestData != null)
                            {
                                //Adding Missing Payments...
                                return await AddPaymentInterestDetails(transaction, interestData);
                            }
                        }

                    }
                    else
                    {
                        DateTime previousMonthInterestPaidate;

                        diffenceInDays = (clientPaymentInterestViewModel.InterestPaidDate.Value.Date - paymentDueData.DueDate.Value.Date);

                        if (diffenceInDays.TotalDays > 45 && diffenceInDays.TotalDays < 90)
                        {
                            DateTime? maxInterestPaidDate = await DbContext.ClientInterestPayments
                                                            .Where(x => x.PaymentId == interestData.PaymentId
                                                            && x.ClientId == interestData.ClientId)
                                                            .MaxAsync(o => (DateTime?)o.InterestPaidDate);
                       
                            diffenceInDays = (clientPaymentInterestViewModel.InterestPaidDate.Value.Date - maxInterestPaidDate.Value.Date);
                        }

                    }

                    if (diffenceInDays.TotalDays <= 30 || (diffenceInDays.TotalDays > 40 && diffenceInDays.TotalDays < 90))
                    {
                        message = await AddPaymentInterestAndPaymentDueData(message, transaction, interestData, paymentDueData, diffenceInDays);
                    }
                    else
                    {
                        message = ClientPaymentInterestValidationMessages.ClientPaymentInterestMissingTwoMonthsErrorMessage;
                    }
                }
                else
                {
                    message = ClientPaymentInterestValidationMessages.ClientPaymentInterestRecordAlreadyExistsErrorMessage;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ClientPaymentInterestValidationMessages.ClientPaymentInterestCommonErrorMessage);
                message = ClientPaymentInterestValidationMessages.ClientPaymentInterestCommonErrorMessage;
                await transaction.RollbackAsync();
            }
            return message;
          
        }

        private async Task<string> AddPaymentInterestAndPaymentDueData(string message, Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction, ClientInterestPayment? interestData, PaymentDueDate paymentDueData, TimeSpan diffenceInDays)
        {
            if (interestData != null)
            {
                interestData.IsItMissedPayment = false;
                interestData.UserId = 1;
                interestData.IsDeleted = false;
                interestData.CreatedDate = DateTime.UtcNow;
                DbContext.ClientInterestPayments.Add(interestData);
                await DbContext.SaveChangesAsync();
            }
            if (paymentDueData != null)
            {

                paymentDueData.DueDate = diffenceInDays.TotalDays > 60
                                    ? paymentDueData.DueDate.Value.AddDays(65)
                                    : paymentDueData.DueDate.Value.AddDays(40);

                paymentDueData.MonthName = paymentDueData.DueDate.Value.ToString("MMMM", new CultureInfo("en-GB"));
                paymentDueData.CreatedDate = DateTime.Now;

                DbContext.PaymentDueDates.Update(paymentDueData);
                await DbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                message = "Success";
            }

            return message;
        }

        private async Task<string> AddPaymentInterestDetails(Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction, ClientInterestPayment? interestData)
        {
            interestData.IsItMissedPayment = true;
            interestData.UserId = 1;
            interestData.IsDeleted = false;
            interestData.CreatedDate = DateTime.UtcNow;
            DbContext.ClientInterestPayments.Add(interestData);
            await DbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return "Success";
        }

        private bool CheckPaymentInterestDataExistsOrNot(ClientPaymentInterestViewModel clientPaymentInterestViewModel)
        {
            var checkDataExistsOrNot = (DbContext.ClientInterestPayments.Where(
                                            x => x.ClientId == clientPaymentInterestViewModel.ClientId &&
                                            x.PaymentId == clientPaymentInterestViewModel.PaymentId &&
                                            x.InterestPaidDate.Value.Date == clientPaymentInterestViewModel.InterestPaidDate.Value.Date
                                            ));
            return checkDataExistsOrNot.Any() ? true : false;
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
