using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentTrackingSystem.Web.Infrastructure.Implementation;
using PaymentTrackingSystem.Web.Infrastructure.Interface;

namespace PaymentTrackingSystem.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ILogger<CountryController> logger;
        private ICountryManager countryManager   { get; set; }

        public CountryController(ICountryManager _countryManager,
                                 ILogger<CountryController> _logger)
        {
            countryManager = _countryManager;
            logger = _logger;
        }

        [HttpGet]
        [Route("GetAllCountries")]
        public async Task<IActionResult> GetAllCountries()
        {
            try
            {
                var countries = await countryManager.GetCountries();
                if (countries.Count == 0)
                {
                    return NoContent();
                }
                return Ok(countries);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "An error occured while processing your request.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = ex.Message,
                    Details = ex.StackTrace
                });
            }

        }
    }
}
