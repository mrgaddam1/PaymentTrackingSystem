using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentTrackingSystem.Shared;
using PaymentTrackingSystem.Web.Infrastructure.Interface;

namespace PaymentTrackingSystem.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantTypeController : ControllerBase
    {
        private readonly ITenantTypeManager tenantTypeManager;
        private readonly ILogger<TenantTypeController> logger;

        public TenantTypeController(ITenantTypeManager tenantTypeManager, ILogger<TenantTypeController> logger)
        {
            this.tenantTypeManager = tenantTypeManager;
            this.logger = logger;
        }

        [HttpGet("GetAllTenantTypes")]
        public async Task<IActionResult> GetAllTenantTypes()
        {
            try
            {
                var tenantTypes = await tenantTypeManager.GetAllTenantTypes();
                return tenantTypes.Count == 0 ? NoContent() : Ok(tenantTypes);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to get tenant types.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("GetTenantTypeDetailsById")]
        public async Task<IActionResult> GetTenantTypeDetailsById([FromQuery] int tenantTypeId)
        {
            if (tenantTypeId <= 0)
            {
                return BadRequest("A valid tenant type ID is required.");
            }

            try
            {
                var tenantType = await tenantTypeManager.GetTenantTypeDetailsById(tenantTypeId);
                return tenantType == null ? NotFound() : Ok(tenantType);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to get tenant type {TenantTypeId}.", tenantTypeId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] TenantTypeViewModel tenantTypeViewModel)
        {
            if (tenantTypeViewModel == null || string.IsNullOrWhiteSpace(tenantTypeViewModel.TenantTypeName))
            {
                return BadRequest("Tenant type name is required.");
            }

            try
            {
                if (!await tenantTypeManager.Add(tenantTypeViewModel))
                {
                    return BadRequest("Failed to add tenant type.");
                }

                return Ok(tenantTypeViewModel);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to add tenant type.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] TenantTypeViewModel tenantTypeViewModel)
        {
            if (tenantTypeViewModel == null || tenantTypeViewModel.TenantTypeId <= 0 || string.IsNullOrWhiteSpace(tenantTypeViewModel.TenantTypeName))
            {
                return BadRequest("Valid tenant type data is required.");
            }

            try
            {
                if (!await tenantTypeManager.Update(tenantTypeViewModel))
                {
                    return BadRequest("Failed to update tenant type.");
                }

                return Ok(tenantTypeViewModel);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to update tenant type {TenantTypeId}.", tenantTypeViewModel.TenantTypeId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery] int tenantTypeId)
        {
            if (tenantTypeId <= 0)
            {
                return BadRequest("A valid tenant type ID is required.");
            }

            try
            {
                if (!await tenantTypeManager.Delete(tenantTypeId))
                {
                    return BadRequest("Failed to delete tenant type.");
                }

                return NoContent();
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to delete tenant type {TenantTypeId}.", tenantTypeId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
