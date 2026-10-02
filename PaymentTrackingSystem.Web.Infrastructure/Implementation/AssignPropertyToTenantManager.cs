using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentTrackingSystem.Core.Data.Models;
using PaymentTrackingSystem.Shared;
using PaymentTrackingSystem.Web.Infrastructure.Interface;

namespace PaymentTrackingSystem.Web.Infrastructure.Implementation
{
    public class AssignPropertyToTenantManager : IAssignPropertyToTenantManager
    {
        private readonly PTSContext dbContext;
        private readonly ILogger<AssignPropertyToTenantManager> logger;

        public AssignPropertyToTenantManager(PTSContext dbContext, ILogger<AssignPropertyToTenantManager> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<List<AssignPropertyToTenantViewModel>> GetAllAssignments()
        {
            return await (
                from assignment in dbContext.TenantPropertyAssigneds.AsNoTracking()
                join tenant in dbContext.Tenants.AsNoTracking() on assignment.TenantId equals tenant.TenantId
                join property in dbContext.Properties.AsNoTracking() on assignment.PropertyId equals property.PropertyId
                where assignment.DeletedDatet == null
                      && tenant.DeletedDate == null
                      && property.DeleteDate == null
                orderby assignment.CreatedDate descending, assignment.RentId descending
                select new AssignPropertyToTenantViewModel
                {
                    RentId = assignment.RentId,
                    TenantId = assignment.TenantId,
                    TenantName = ((tenant.FirstName ?? string.Empty) + " " + (tenant.LastName ?? string.Empty)).Trim(),
                    PropertyId = assignment.PropertyId,
                    PropertyName = property.PropertyName,
                    RentAmount = assignment.Amount,
                    TenantStartDate = assignment.TenantStartDate,
                    TenantEndDate = assignment.TenantEndDate,
                    AnyAgreement = assignment.AnyAgreement ?? false,
                    //AgreementFileName = dbContext.TenantAgreements
                    //    .Where(agreement => agreement.RentId == assignment.RentId && agreement.TenantId == assignment.TenantId)
                    //    .Select(agreement => agreement.AgreementFileName)
                    //    .FirstOrDefault(),
                    //AgreementFileType = dbContext.TenantAgreements
                    //    .Where(agreement => agreement.RentId == assignment.RentId && agreement.TenantId == assignment.TenantId)
                    //    .Select(agreement => agreement.AgreementFileType)
                    //    .FirstOrDefault()
                }).ToListAsync();
        }

        public async Task<AssignPropertyToTenantViewModel?> GetAssignmentById(int rentId)
        {
            var assignment = await (
                from item in dbContext.TenantPropertyAssigneds.AsNoTracking()
                join tenant in dbContext.Tenants.AsNoTracking() on item.TenantId equals tenant.TenantId
                join property in dbContext.Properties.AsNoTracking() on item.PropertyId equals property.PropertyId
                where item.RentId == rentId
                      && item.DeletedDatet == null
                      && tenant.DeletedDate == null
                      && property.DeleteDate == null
                select new AssignPropertyToTenantViewModel
                {
                    RentId = item.RentId,
                    TenantId = item.TenantId,
                    TenantName = ((tenant.FirstName ?? string.Empty) + " " + (tenant.LastName ?? string.Empty)).Trim(),
                    PropertyId = item.PropertyId,
                    PropertyName = property.PropertyName,
                    RentAmount = item.Amount,
                    TenantStartDate = item.TenantStartDate,
                    TenantEndDate = item.TenantEndDate,
                    AnyAgreement = item.AnyAgreement ?? false
                }).FirstOrDefaultAsync();

            if (assignment == null)
            {
                return null;
            }

            var agreement = await dbContext.TenantAgreements
                .AsNoTracking()
                .FirstOrDefaultAsync(item => item.RentId == rentId && item.TenantId == assignment.TenantId);

            if (agreement != null)
            {
                assignment.AgreementFileName = agreement.AgreementFileName;
                assignment.AgreementFileType = agreement.AgreementFileType;
                assignment.AgreementData = agreement.AgreementData;
            }

            return assignment;
        }

        public async Task<bool> Add(AssignPropertyToTenantViewModel assignment)
        {
            if (!IsValid(assignment) ||
                !await dbContext.Tenants.AnyAsync(tenant => tenant.TenantId == assignment.TenantId && tenant.DeletedDate == null) ||
                !await dbContext.Properties.AnyAsync(property => property.PropertyId == assignment.PropertyId && property.DeleteDate == null))
            {
                return false;
            }

            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                var entity = new TenantPropertyAssigned
                {
                    UserId = 1,
                    TenantId = assignment.TenantId,
                    PropertyId = assignment.PropertyId,
                    Amount = assignment.RentAmount,
                    TenantStartDate = assignment.TenantStartDate,
                    TenantEndDate = assignment.TenantEndDate,
                    AnyAgreement = assignment.AnyAgreement,
                    CreatedDate = DateTime.UtcNow
                };

                dbContext.TenantPropertyAssigneds.Add(entity);
                await dbContext.SaveChangesAsync();

                if (assignment.AnyAgreement && assignment.AgreementData is { Length: > 0 })
                {
                    dbContext.TenantAgreements.Add(new TenantAgreement
                    {
                        UserId = 1,
                        RentId = entity.RentId,
                        TenantId = entity.TenantId,
                        AgreementFileName = assignment.AgreementFileName,
                        AgreementFileType = assignment.AgreementFileType,
                        AgreementData = assignment.AgreementData
                    });
                    await dbContext.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                assignment.RentId = entity.RentId;
                return true;
            }
            catch (Exception exception)
            {
                await transaction.RollbackAsync();
                logger.LogError(exception, "Failed to add tenant property assignment.");
                return false;
            }
        }

        public async Task<bool> Update(AssignPropertyToTenantViewModel assignment)
        {
            if (!IsValid(assignment) || assignment.RentId <= 0 ||
                !await dbContext.Tenants.AnyAsync(tenant => tenant.TenantId == assignment.TenantId && tenant.DeletedDate == null) ||
                !await dbContext.Properties.AnyAsync(property => property.PropertyId == assignment.PropertyId && property.DeleteDate == null))
            {
                return false;
            }

            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                var entity = await dbContext.TenantPropertyAssigneds
                    .FirstOrDefaultAsync(item => item.RentId == assignment.RentId && item.DeletedDatet == null);
                if (entity == null)
                {
                    return false;
                }

                var previousTenantId = entity.TenantId;
                entity.TenantId = assignment.TenantId;
                entity.PropertyId = assignment.PropertyId;
                entity.Amount = assignment.RentAmount;
                entity.TenantStartDate = assignment.TenantStartDate;
                entity.TenantEndDate = assignment.TenantEndDate;
                entity.AnyAgreement = assignment.AnyAgreement;
                entity.ModifiedDate = DateTime.UtcNow;

                var agreement = await dbContext.TenantAgreements
                    .FirstOrDefaultAsync(item => item.RentId == entity.RentId && item.TenantId == previousTenantId);

                if (!assignment.AnyAgreement)
                {
                    if (agreement != null)
                    {
                        dbContext.TenantAgreements.Remove(agreement);
                    }
                }
                else if (assignment.AgreementData is { Length: > 0 })
                {
                    if (agreement == null)
                    {
                        agreement = new TenantAgreement
                        {
                            UserId = 1,
                            RentId = entity.RentId,
                            TenantId = entity.TenantId
                        };
                        dbContext.TenantAgreements.Add(agreement);
                    }

                    agreement.AgreementFileName = assignment.AgreementFileName;
                    agreement.AgreementFileType = assignment.AgreementFileType;
                    agreement.AgreementData = assignment.AgreementData;
                }

                if (assignment.AnyAgreement && agreement != null)
                {
                    agreement.TenantId = entity.TenantId;
                }

                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception exception)
            {
                await transaction.RollbackAsync();
                logger.LogError(exception, "Failed to update tenant property assignment {RentId}.", assignment.RentId);
                return false;
            }
        }

        public async Task<bool> Delete(int rentId)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                var entity = await dbContext.TenantPropertyAssigneds
                    .FirstOrDefaultAsync(item => item.RentId == rentId && item.DeletedDatet == null);
                if (entity == null)
                {
                    return false;
                }

                entity.DeletedDatet = DateTime.UtcNow;
                entity.ModifiedDate = DateTime.UtcNow;
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception exception)
            {
                await transaction.RollbackAsync();
                logger.LogError(exception, "Failed to delete tenant property assignment {RentId}.", rentId);
                return false;
            }
        }

        private static bool IsValid(AssignPropertyToTenantViewModel assignment)
        {
            return assignment != null
                   && assignment.TenantId > 0
                   && assignment.PropertyId > 0
                   && assignment.RentAmount >= 0
                   && assignment.TenantStartDate != default;
        }
    }
}
