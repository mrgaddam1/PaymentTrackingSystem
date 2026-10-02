using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentTrackingSystem.Shared;
using PaymentTrackingSystem.Web.Infrastructure.Interface;

namespace PaymentTrackingSystem.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictController : ControllerBase
    {
        private readonly IDistrictManager districtManager;
        private readonly ILogger<DistrictController> logger;

        public DistrictController(IDistrictManager districtManager, ILogger<DistrictController> logger)
        {
            this.districtManager = districtManager;
            this.logger = logger;
        }

        [HttpGet("GetAllDistricts")]
        public async Task<IActionResult> GetAllDistricts()
        {
            try
            {
                var districts = await districtManager.GetAllDistricts();
                return districts.Count == 0 ? NoContent() : Ok(districts);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to get districts.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("GetDistrictDetailsById")]
        public async Task<IActionResult> GetDistrictDetailsById([FromQuery] int districtId)
        {
            if (districtId <= 0)
            {
                return BadRequest("A valid district ID is required.");
            }

            try
            {
                var district = await districtManager.GetDistrictDetailsById(districtId);
                return district == null ? NotFound() : Ok(district);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to get district {DistrictId}.", districtId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] DistrictViewModel districtViewModel)
        {
            if (districtViewModel == null)
            {
                return BadRequest("District data is required.");
            }

            try
            {
                if (!await districtManager.Add(districtViewModel))
                {
                    return BadRequest("Failed to add district.");
                }

                return Ok(districtViewModel);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to add district.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] DistrictViewModel districtViewModel)
        {
            if (districtViewModel == null || districtViewModel.DistrictId <= 0)
            {
                return BadRequest("Valid district data is required.");
            }

            try
            {
                if (!await districtManager.Update(districtViewModel))
                {
                    return BadRequest("Failed to update district.");
                }

                return Ok(districtViewModel);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to update district {DistrictId}.", districtViewModel.DistrictId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery] int districtId)
        {
            if (districtId <= 0)
            {
                return BadRequest("A valid district ID is required.");
            }

            try
            {
                if (!await districtManager.Delete(districtId))
                {
                    return BadRequest("Failed to delete district.");
                }

                return NoContent();
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to delete district {DistrictId}.", districtId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
