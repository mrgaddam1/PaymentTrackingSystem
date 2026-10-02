using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentTrackingSystem.Shared;
using PaymentTrackingSystem.Web.Infrastructure.Interface;

namespace PaymentTrackingSystem.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignPropertyToTenantController : ControllerBase
    {
        private readonly IAssignPropertyToTenantManager manager;
        private readonly ILogger<AssignPropertyToTenantController> logger;

        public AssignPropertyToTenantController(
            IAssignPropertyToTenantManager manager,
            ILogger<AssignPropertyToTenantController> logger)
        {
            this.manager = manager;
            this.logger = logger;
        }

        [HttpGet("GetAllAssignments")]
        public async Task<IActionResult> GetAllAssignments()
        {
            try
            {
                var assignments = await manager.GetAllAssignments();
                return assignments.Count == 0 ? NoContent() : Ok(assignments);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to load tenant property assignments.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to load assignments.");
            }
        }

        [HttpGet("GetAssignmentById")]
        public async Task<IActionResult> GetAssignmentById([FromQuery] int rentId)
        {
            if (rentId <= 0)
            {
                return BadRequest("A valid assignment ID is required.");
            }

            try
            {
                var assignment = await manager.GetAssignmentById(rentId);
                return assignment == null ? NotFound() : Ok(assignment);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to load tenant property assignment {RentId}.", rentId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to load the assignment.");
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] AssignPropertyToTenantViewModel assignment)
        {
            if (assignment == null)
            {
                return BadRequest("Assignment data is required.");
            }

            try
            {
                if (!await manager.Add(assignment))
                {
                    return BadRequest("Failed to create the assignment.");
                }

                return Ok(assignment);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to create tenant property assignment.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create the assignment.");
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] AssignPropertyToTenantViewModel assignment)
        {
            if (assignment == null || assignment.RentId <= 0)
            {
                return BadRequest("Assignment data and a valid assignment ID are required.");
            }

            try
            {
                if (!await manager.Update(assignment))
                {
                    return NotFound();
                }

                return Ok(assignment);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to update tenant property assignment {RentId}.", assignment.RentId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update the assignment.");
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery] int rentId)
        {
            if (rentId <= 0)
            {
                return BadRequest("A valid assignment ID is required.");
            }

            try
            {
                return await manager.Delete(rentId) ? NoContent() : NotFound();
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to delete tenant property assignment {RentId}.", rentId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete the assignment.");
            }
        }
    }
}
