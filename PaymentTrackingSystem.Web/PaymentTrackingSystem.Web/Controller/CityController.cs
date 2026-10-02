using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentTrackingSystem.Shared;
using PaymentTrackingSystem.Web.Infrastructure.Interface;

namespace PaymentTrackingSystem.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityManager cityManager;
        private readonly ILogger<CityController> logger;

        public CityController(ICityManager cityManager, ILogger<CityController> logger)
        {
            this.cityManager = cityManager;
            this.logger = logger;
        }

        [HttpGet("GetAllCities")]
        public async Task<IActionResult> GetAllCities()
        {
            try
            {
                var cities = await cityManager.GetAllCities();
                return cities.Count == 0 ? NoContent() : Ok(cities);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to get cities.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("GetCityDetailsById")]
        public async Task<IActionResult> GetCityDetailsById([FromQuery] int cityId)
        {
            if (cityId <= 0)
            {
                return BadRequest("A valid city ID is required.");
            }

            try
            {
                var city = await cityManager.GetCityDetailsById(cityId);
                return city == null ? NotFound() : Ok(city);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to get city {CityId}.", cityId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] CityViewModel cityViewModel)
        {
            if (cityViewModel == null || string.IsNullOrWhiteSpace(cityViewModel.CityName))
            {
                return BadRequest("City name is required.");
            }

            try
            {
                if (!await cityManager.Add(cityViewModel))
                {
                    return BadRequest("Failed to add city.");
                }

                return Ok(cityViewModel);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to add city.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] CityViewModel cityViewModel)
        {
            if (cityViewModel == null || cityViewModel.CityId <= 0 || string.IsNullOrWhiteSpace(cityViewModel.CityName))
            {
                return BadRequest("Valid city data is required.");
            }

            try
            {
                if (!await cityManager.Update(cityViewModel))
                {
                    return BadRequest("Failed to update city.");
                }

                return Ok(cityViewModel);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to update city {CityId}.", cityViewModel.CityId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery] int cityId)
        {
            if (cityId <= 0)
            {
                return BadRequest("A valid city ID is required.");
            }

            try
            {
                if (!await cityManager.Delete(cityId))
                {
                    return BadRequest("Failed to delete city.");
                }

                return NoContent();
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to delete city {CityId}.", cityId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
