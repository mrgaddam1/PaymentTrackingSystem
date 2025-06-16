using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentTrackingSystem.Shared;
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

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] ClientViewModel clientViewModel)
        {
            try
            {
                if (clientViewModel == null)
                {
                    return BadRequest("Client data is required.");
                }
                var response = await ClientManager.Add(clientViewModel);
                if (response)
                {
                    var status = CreatedAtAction(nameof(Add), new { id = clientViewModel.ClientId }, clientViewModel);
                    return Ok(status);
                }
                else
                {
                    var status = StatusCode(StatusCodes.Status400BadRequest, "Failed to add Client");
                    return BadRequest(status);
                }

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

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] ClientViewModel clientViewModel)
        {
            try
            {
                if (clientViewModel == null)
                {
                    return BadRequest("Client data is required.");
                }
                var response = await ClientManager.Update(clientViewModel);
                if (response)
                {
                    var status = (new { message = "Client details are updated successfully", clientViewModel });
                    return Ok(status);
                }
                else
                {
                    var status = StatusCode(StatusCodes.Status400BadRequest, "Failed to update Client");
                    return BadRequest(status);
                }

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
