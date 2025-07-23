using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
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

        public PaymentInterestManager(PTSContext _DbContext, IMapper _mapper, 
            ILogger<PaymentInterestManager> _logger)
        {
            DbContext = _DbContext;
            mapper = _mapper;
            logger = _logger;
        }
        public async Task<List<ClientPaymentInterestViewModel>> GetAll()
        {
            try
            {
                var interestPaymentData = new List<ClientPaymentInterestViewModel>();
                try
                {
                    var data = DataHelper.GetData(DbContext.Database.GetDbConnection(), "Up_GetAll_ClientPaymentInterestDetails", null);
                    interestPaymentData = ConvertDataTableToGenericList.ConvertDataTable<ClientPaymentInterestViewModel>(data).ToList();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message, "An error occured while processing the request.");
                    interestPaymentData = new List<ClientPaymentInterestViewModel>();
                }
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
                                                 ClientId = cpi.ClientId.Value,
                                                 ClientName = c.FirstName + " " + c.LastName,
                                                 PaymentId = cp.PaymentId,
                                                 Amount = cp.Amount.Value,
                                                 InterestRate = cp.InterestRate.Value,
                                                 InterestAmount = cpi.InterestAmount.Value,
                                                 InterestPaidDate = cpi.InterestPaidDate.Value,
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
            string message = string.Empty;

            try
            {
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter("@PaymentId", clientPaymentInterestViewModel.PaymentId),
                    new SqlParameter("@ClientId", clientPaymentInterestViewModel.ClientId),
                    new SqlParameter("@UserId", 1),
                    new SqlParameter("@InterestAmount", clientPaymentInterestViewModel.InterestAmount),
                    new SqlParameter("@InterestPaidDate", clientPaymentInterestViewModel.InterestPaidDate),
                    new SqlParameter("@IsitPaidForTheCurrentMonth", clientPaymentInterestViewModel.IsitPaidForTheCurrentMonth),
                };

                var data = DataHelper.GetData(DbContext.Database.GetDbConnection(), "Up_Insert_Client_Payment_Interest_Details", sqlParameters.ToArray());

                if (data.Rows.Count > 0)
                {
                    message = data.Rows[0]["Transaction_Message"].ToString();
                    if (message == "Data inserted successfully.")
                    {
                        logger.LogInformation(message);
                        message = "Success";
                    }
                    else if (message == "Unable to process transaction.")
                    {
                        logger.LogInformation(message);
                        message = ClientPaymentInterestValidationMessages.ClientPaymentInterestCommonErrorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ClientPaymentInterestValidationMessages.ClientPaymentInterestCommonErrorMessage);
                message = ClientPaymentInterestValidationMessages.ClientPaymentInterestCommonErrorMessage;
            }
            return message;
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
