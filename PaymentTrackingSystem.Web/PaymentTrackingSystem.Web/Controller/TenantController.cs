using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentTrackingSystem.Shared;
using PaymentTrackingSystem.Web.Infrastructure.Interface;

namespace PaymentTrackingSystem.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly ILogger<TenantController> logger;
        private readonly ITenantManager tenantManager;

        public TenantController(ITenantManager tenantManager, ILogger<TenantController> logger)
        {
            this.tenantManager = tenantManager;
            this.logger = logger;
        }

        [HttpGet("GetAllTenants")]
        public async Task<IActionResult> GetAllTenants()
        {
            try
            {
                var tenants = await tenantManager.GetAllTenants();
                return tenants.Count == 0 ? NoContent() : Ok(tenants);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while loading tenants.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while loading tenants.");
            }
        }

        [HttpGet("GetTenantDetailsById")]
        public async Task<IActionResult> GetTenantDetailsById(int tenantId)
        {
            if (tenantId <= 0)
            {
                return BadRequest("Tenant ID is required.");
            }

            try
            {
                var tenant = await tenantManager.GetTenantDetailsById(tenantId);
                return tenant == null ? NotFound() : Ok(tenant);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while loading tenant {TenantId}.", tenantId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while loading the tenant.");
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] TenantViewModel tenantViewModel)
        {
            if (tenantViewModel == null)
            {
                return BadRequest("Tenant data is required.");
            }

            try
            {
                return await tenantManager.Add(tenantViewModel)
                    ? Ok(tenantViewModel)
                    : BadRequest("Failed to add tenant.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while adding a tenant.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding the tenant.");
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] TenantViewModel tenantViewModel)
        {
            if (tenantViewModel == null || tenantViewModel.TenantId <= 0)
            {
                return BadRequest("Tenant data and ID are required.");
            }

            try
            {
                return await tenantManager.Update(tenantViewModel)
                    ? Ok(tenantViewModel)
                    : NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while updating tenant {TenantId}.", tenantViewModel.TenantId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the tenant.");
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int tenantId)
        {
            if (tenantId <= 0)
            {
                return BadRequest("Tenant ID is required.");
            }

            try
            {
                return await tenantManager.Delete(tenantId)
                    ? Ok()
                    : NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while deleting tenant {TenantId}.", tenantId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the tenant.");
            }
        }
    }
}
