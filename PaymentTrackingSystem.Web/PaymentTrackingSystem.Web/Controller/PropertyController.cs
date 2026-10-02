using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentTrackingSystem.Shared;
using PaymentTrackingSystem.Web.Infrastructure.Interface;

namespace PaymentTrackingSystem.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly ILogger<PropertyController> logger;
        private IPropertyManager PropertyManager { get; set; }

        public PropertyController(IPropertyManager _PropertyManager, ILogger<PropertyController> _logger)
        {
            PropertyManager = _PropertyManager;
            logger = _logger;
        }

        [HttpGet]
        [Route("GetAllProperties")]
        public async Task<IActionResult> GetAllProperties()
        {
            try
            {
                var properties = await PropertyManager.GetAllProperties();
                if (properties.Count == 0)
                {
                    return NoContent();
                }
                return Ok(properties);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "An error occurred while processing your request.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = ex.Message,
                    Details = ex.StackTrace
                });
            }
        }

        [HttpGet]
        [Route("GetPropertyDetailsById")]
        public async Task<IActionResult> GetPropertyDetailsById(int propertyId)
        {
            try
            {
                if (propertyId <= 0)
                {
                    return BadRequest("Property ID is required.");
                }
                var property = await PropertyManager.GetPropertyDetailsById(propertyId);
                if (property == null)
                {
                    return NotFound();
                }
                return Ok(property);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "An error occurred while processing your request.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = ex.Message,
                    Details = ex.StackTrace
                });
            }
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] PropertyViewModel propertyViewModel)
        {
            try
            {
                if (propertyViewModel == null)
                {
                    return BadRequest("Property data is required.");
                }
                var response = await PropertyManager.Add(propertyViewModel);
                if (response)
                {
                    var status = CreatedAtAction(nameof(Add), new { id = propertyViewModel.PropertyId }, propertyViewModel);
                    return Ok(status);
                }
                else
                {
                    var status = StatusCode(StatusCodes.Status400BadRequest, "Failed to add Property");
                    return BadRequest(status);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "An error occurred while processing your request.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = ex.Message,
                    Details = ex.StackTrace
                });
            }
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] PropertyViewModel propertyViewModel)
        {
            try
            {
                if (propertyViewModel == null)
                {
                    return BadRequest("Property data is required.");
                }
                var response = await PropertyManager.Update(propertyViewModel);
                if (response)
                {
                    var status = new { message = "Property details are updated successfully", propertyViewModel };
                    return Ok(status);
                }
                else
                {
                    var status = StatusCode(StatusCodes.Status400BadRequest, "Failed to update Property");
                    return BadRequest(status);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "An error occurred while processing your request.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = ex.Message,
                    Details = ex.StackTrace
                });
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int propertyId)
        {
            try
            {
                if (propertyId <= 0)
                {
                    return BadRequest("Property ID is required.");
                }
                var response = await PropertyManager.Delete(propertyId);
                if (response)
                {
                    var status = new { message = "Property deleted successfully" };
                    return Ok(status);
                }
                else
                {
                    var status = StatusCode(StatusCodes.Status400BadRequest, "Failed to delete Property");
                    return BadRequest(status);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "An error occurred while processing your request.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = ex.Message,
                    Details = ex.StackTrace
                });
            }
        }
    }
}
