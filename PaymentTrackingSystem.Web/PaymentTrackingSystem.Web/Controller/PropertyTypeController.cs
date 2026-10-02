using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentTrackingSystem.Shared;
using PaymentTrackingSystem.Web.Infrastructure.Interface;

namespace PaymentTrackingSystem.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyTypeController : ControllerBase
    {
        private readonly IPropertyTypeManager propertyTypeManager;
        private readonly ILogger<PropertyTypeController> logger;

        public PropertyTypeController(IPropertyTypeManager propertyTypeManager, ILogger<PropertyTypeController> logger)
        {
            this.propertyTypeManager = propertyTypeManager;
            this.logger = logger;
        }

        [HttpGet("GetAllPropertyTypes")]
        public async Task<IActionResult> GetAllPropertyTypes()
        {
            try
            {
                var propertyTypes = await propertyTypeManager.GetAllPropertyTypes();
                return propertyTypes.Count == 0 ? NoContent() : Ok(propertyTypes);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to get property types.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("GetPropertyTypeDetailsById")]
        public async Task<IActionResult> GetPropertyTypeDetailsById([FromQuery] int propertyTypeId)
        {
            if (propertyTypeId <= 0)
            {
                return BadRequest("A valid property type ID is required.");
            }

            try
            {
                var propertyType = await propertyTypeManager.GetPropertyTypeDetailsById(propertyTypeId);
                return propertyType == null ? NotFound() : Ok(propertyType);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to get property type {PropertyTypeId}.", propertyTypeId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] PropertyTypeViewModel propertyTypeViewModel)
        {
            if (propertyTypeViewModel == null)
            {
                return BadRequest("Property type data is required.");
            }

            try
            {
                if (!await propertyTypeManager.Add(propertyTypeViewModel))
                {
                    return BadRequest("Failed to add property type.");
                }

                return Ok(propertyTypeViewModel);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to add property type.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] PropertyTypeViewModel propertyTypeViewModel)
        {
            if (propertyTypeViewModel == null || propertyTypeViewModel.PropertyTypeId <= 0)
            {
                return BadRequest("Valid property type data is required.");
            }

            try
            {
                if (!await propertyTypeManager.Update(propertyTypeViewModel))
                {
                    return BadRequest("Failed to update property type.");
                }

                return Ok(propertyTypeViewModel);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to update property type {PropertyTypeId}.", propertyTypeViewModel.PropertyTypeId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery] int propertyTypeId)
        {
            if (propertyTypeId <= 0)
            {
                return BadRequest("A valid property type ID is required.");
            }

            try
            {
                if (!await propertyTypeManager.Delete(propertyTypeId))
                {
                    return BadRequest("Failed to delete property type.");
                }

                return NoContent();
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to delete property type {PropertyTypeId}.", propertyTypeId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
