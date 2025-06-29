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
                var interestPayment = (DbContext
                                      .ClientInterestPayments
                                      .AsNoTracking()
                                      .OrderBy(x => x.CreatedDate)
                                      .ToListAsync());
                return mapper.Map<List<ClientPaymentInterestViewModel>>(interestPayment) ?? new List<ClientPaymentInterestViewModel>();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "An error occured while processing details...");
                return new List<ClientPaymentInterestViewModel>();
            }
        }
        public async Task<ClientPaymentInterestViewModel> GetAllDetailsById(int interestId)
        {
            try
            {
                var interestPayment = (DbContext
                                      .ClientInterestPayments
                                      .AsNoTracking()
                                      .Where(x=>x.InterestId == interestId)
                                      .OrderBy(x => x.CreatedDate)
                                      .FirstOrDefaultAsync());
                return mapper.Map<ClientPaymentInterestViewModel>(interestPayment) ?? new ClientPaymentInterestViewModel();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "An error occured while processing details...");
                return new ClientPaymentInterestViewModel();
            }
        }
        public async Task<bool> Add(ClientPaymentInterestViewModel clientPaymentInterestViewModel)
        {
            bool isSuccess= false;
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
                    isSuccess =  true;
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
                var interestPaymentData = await(DbContext
                                          .ClientInterestPayments
                                          .AsNoTracking()
                                          .Where(x => x.InterestId == clientPaymentInterestViewModel.InterestId)
                                          .OrderBy(x => x.CreatedDate)
                                          .SingleOrDefaultAsync());
               
                if (interestPaymentData != null)
                {
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
            bool isDeleted= false;
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


    }
}
