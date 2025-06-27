using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentTrackingSystem.Shared;
using PaymentTrackingSystem.Web.Infrastructure.Implementation;
using PaymentTrackingSystem.Web.Infrastructure.Interface;

namespace PaymentTrackingSystem.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientPaymentsController : ControllerBase
    {
        private readonly ILogger<ClientPaymentsController> logger;
        private IPaymentManager PaymentManager { get; set; }
        public ClientPaymentsController(IPaymentManager _PaymentManager, ILogger<ClientPaymentsController> _logger)
        {
            PaymentManager = _PaymentManager;
            logger = _logger;
        }

        [HttpGet]
        [Route("GetAllClientPayments")]
        public async Task<IActionResult> GetAllClientPayments()
        {
            try
            {
                var clientPayments = await PaymentManager.GetAllClientPayments();
                if (clientPayments.Count == 0)
                {
                    return NoContent();
                }
                return Ok(clientPayments);
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

        [HttpGet]
        [Route("GetAllClientPaymentsDetailsByPaymentId/{PaymentId}")]
        public async Task<IActionResult> GetAllClientPaymentsDetailsByPaymentId(int PaymentId)
        {
            try
            {
                var clientPayments = await PaymentManager.GetClientPaymentDetailsById(PaymentId);
                if (clientPayments == null)
                {
                    return NoContent();
                }
                return Ok(clientPayments);
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
        public async Task<IActionResult> Add([FromBody] ClientPaymentViewModel clientPaymentViewModel)
        {
            try
            {
                if (clientPaymentViewModel == null)
                {
                    return BadRequest("Client Payment data is required.");
                }
                var response = await PaymentManager.Add(clientPaymentViewModel);
                if (response)
                {
                    var status = CreatedAtAction(nameof(Add), new { id = clientPaymentViewModel.PaymentId }, clientPaymentViewModel);
                    return Ok(status);
                }
                else
                {
                    var status = StatusCode(StatusCodes.Status400BadRequest, "Failed to add Client Payments");
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
        public async Task<IActionResult> Update([FromBody] ClientPaymentViewModel clientPaymentViewModel)
        {
            try
            {
                if (clientPaymentViewModel == null)
                {
                    return BadRequest("Client Payment data is required.");
                }
                var response = await PaymentManager.Update(clientPaymentViewModel);
                if (response)
                {
                    var status = (new { message = "Client Payment details are updated successfully", clientPaymentViewModel });
                    return Ok(status);
                }
                else
                {
                    var status = StatusCode(StatusCodes.Status400BadRequest, "Failed to update Client Payment");
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



        [HttpDelete]
        [Route("Delete/{paymentId}")]
        public async Task<IActionResult> Delete(int paymentId)
        {
            try
            {
                if (paymentId == null)
                {
                    return BadRequest("Client Payment data is required.");
                }
                var response = await PaymentManager.Delete(paymentId);
                if (response)
                {
                    var status = (new { message = "Client Payment details are deleted successfully" });
                    return Ok(status);
                }
                else
                {
                    var status = StatusCode(StatusCodes.Status400BadRequest, "Failed to delete Client Payment details");
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
