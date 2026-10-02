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
    public class LendingManager : ILendingManager
    {
        private readonly ILogger<LendingManager> logger;
        private PTSContext DbContext { get; set; }

        public LendingManager(PTSContext _DbContext, ILogger<LendingManager> _logger)
        {
            DbContext = _DbContext;
            logger = _logger;
        }

        public async Task<List<LendingViewModel>> GetAllLendings()
        {
            try
            {
                var lendings = new List<LendingViewModel>();

                // Note: You'll need to map from Lending entity to LendingViewModel
                // For now, returning empty list. Add your mapping logic with AutoMapper or manual mapping
                return lendings;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "An error occurred while retrieving lendings.");
                return new List<LendingViewModel>();
            }
        }

        public async Task<LendingViewModel> GetLendingDetailsById(int lendingId)
        {
            try
            {
                var lending = new LendingViewModel();

                // Note: Add your query logic here
                // var data = await DbContext.Lendings
                //     .Where(x => x.LendingId == lendingId && (x.IsDeleted == null || x.IsDeleted == false))
                //     .FirstOrDefaultAsync();

                return lending;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "An error occurred while retrieving lending details.");
                return new LendingViewModel();
            }
        }

        public async Task<bool> Add(LendingViewModel lending)
        {
            bool isSuccess = false;
            using var transaction = await DbContext.Database.BeginTransactionAsync();
            try
            {
                var lendingEntity = new Lender
                {
                    UserId = lending.UserId,
                    FirstName = lending.FirstName,
                    LastName = lending.LastName,
                    EmailId = lending.EmailId,
                    MobileNumber = lending.PhoneNumber,
                    CreatedDate = DateTimeOffset.UtcNow,
                    IsActive = true
                };

                DbContext.Lenders.Add(lendingEntity);
                await DbContext.SaveChangesAsync();

                var address = new LenderAddress
                {
                    LenderId = lendingEntity.LenderId,
                    AddressLine1 = lending.AddressLine1,
                    AddressLine2 = lending.AddressLine2,
                    Postcode = lending.Postcode,
                    CountryId = lending.CountryId
                };

                DbContext.LenderAddresses.Add(address);
                await DbContext.SaveChangesAsync();

                var lendingInterest = new LendingAmountDetail
                {
                    LenderId = lendingEntity.LenderId,
                    UserId = lending.UserId,
                    LendingAmount = lending.LendingAmount,
                    LendingInterestRateId = lending.LendingInterestRateId,
                    ActualInterestAmount = lending.ActualInterestAmount,
                    ExpectedInterestAmount = lending.ExpectedInterestAmount,
                    PaidDate = lending.PaidDate,
                    DueDate = lending.DueDate,
                    PaymentModeId = lending.PaymentModeId,
                    IsPaid = lending.IsPaid,
                    CreatedDate = DateTimeOffset.UtcNow
                };

                DbContext.LendingAmountDetails.Add(lendingInterest);
                await DbContext.SaveChangesAsync();

                var lendingDocument = new LendingDocument
                {
                    LenderId = lendingEntity.LenderId,
                    UserId = lending.UserId,
                    DocumentName = lending.DocumentName,
                    DocumentExtension = lending.DocumentExtension,
                    UploadDocumentData = lending.UploadDocumentData,
                    CreatedDate = DateTimeOffset.UtcNow
                };

                DbContext.LendingDocuments.Add(lendingDocument);
                await DbContext.SaveChangesAsync();


                logger.LogInformation("Lending added successfully.");
                isSuccess = true;
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                logger.LogError(ex.Message, "An error occurred while adding lending.");
                isSuccess = false;

            }
            return isSuccess;
        }

        public async Task<bool> Update(LendingViewModel lendingViewModel)
        {
            bool isSuccess = false;
            using var transaction = await DbContext.Database.BeginTransactionAsync();
            try
            {
                // Note: Add your entity update logic here
                // var lending = await DbContext.Lendings
                //     .Where(x => x.LendingId == lendingViewModel.LendingId)
                //     .FirstOrDefaultAsync();
                // if (lending != null) { /* update properties */ }

                await DbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                logger.LogInformation("Lending updated successfully.");
                isSuccess = true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                logger.LogError(ex.Message, "An error occurred while updating lending.");
                isSuccess = false;
            }
            return isSuccess;
        }

        public async Task<bool> Delete(int lendingId)
        {
            bool isSuccess = false;
            using var transaction = await DbContext.Database.BeginTransactionAsync();
            try
            {
                // Note: Add your entity delete logic here
                // var lending = await DbContext.Lendings
                //     .Where(x => x.LendingId == lendingId)
                //     .FirstOrDefaultAsync();
                // if (lending != null) { DbContext.Lendings.Remove(lending); }

                await DbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                logger.LogInformation("Lending deleted successfully.");
                isSuccess = true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                logger.LogError(ex.Message, "An error occurred while deleting lending.");
                isSuccess = false;
            }
            return isSuccess;
        }

        public async Task<List<PaymentModeViewModel>> GetPaymentModes()
        {
            var paymentModes = new List<PaymentModeViewModel>();
            try
            {
                paymentModes = DbContext.PaymentModes.Select(pm => new PaymentModeViewModel
                {
                    PaymentModeId = pm.PaymentModeId,
                    PaymentModeDescription = pm.PaymentModeDescription
                }).ToList();
                return paymentModes;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "An error occurred while retrieving payment modes.");
                return new List<PaymentModeViewModel>();
            }
        }

        public async Task<List<LendingDueDateDescriptionViewModel>> GetAllLendingDueDateDescriptions()
        {
            var lendingDueDateDes = new List<LendingDueDateDescriptionViewModel>();
            try
            {
                lendingDueDateDes = DbContext.LendingDueDateDescriptions.Select(ldd => new LendingDueDateDescriptionViewModel
                {
                    LendingDueDateId = ldd.LendingDueDateId,
                    LendingDueDateDescritpion = ldd.LendingDueDateDescritpion,
                    NoOfDays = ldd.NoOfDays
                }).ToList();
                return lendingDueDateDes;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "An error occurred while retrieving Lending Due Date Descriptions.");
                return new List<LendingDueDateDescriptionViewModel>();
            }
        }

        public async Task<List<InterestRatesViewModel>> GetAllInterestRates()
        {
            var interestRates = new List<InterestRatesViewModel>();
            try
            {
                interestRates = DbContext.LendingInterestRates.Select(lir => new InterestRatesViewModel
                {
                    InterestId = lir.LendingInterestRateId,
                    AmountInterestRate = lir.InterestRate
                }).ToList();
                return interestRates;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "An error occurred while retrieving Interest Rates.");
                return new List<InterestRatesViewModel>();
            }
        }
    }
}
