using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentTrackingSystem.Web.Infrastructure.Interface;

namespace PaymentTrackingSystem.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ILogger<ClientController> logger;
        private IClientManager ClientManager { get; set; }
        public ClientController(IClientManager _ClientManager, ILogger<ClientController> _logger)
        {
            ClientManager = _ClientManager;
            logger = _logger;
        }

        [HttpGet]
        [Route("GetAllClients")]
        public async Task<IActionResult> GetAllClients()
        {
            try
            {
                var clients = await ClientManager.GetAllClients();
                if (clients.Count == 0)
                {
                    return NoContent();
                }
                return Ok(clients);
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
