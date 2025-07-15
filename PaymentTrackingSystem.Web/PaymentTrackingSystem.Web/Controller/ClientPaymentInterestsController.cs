using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Shared;
using PaymentTrackingSystem.Web.Infrastructure.Interface;

namespace PaymentTrackingSystem.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientPaymentInterestsController : ControllerBase
    {
        private readonly ILogger<ClientPaymentInterestsController> logger;
        private IPaymentInterestManager PaymentInterestManager { get; set; }
        public ClientPaymentInterestsController(IPaymentInterestManager _PaymentInterestManager, 
                                                ILogger<ClientPaymentInterestsController> _logger)
        {
            PaymentInterestManager = _PaymentInterestManager;
            logger = _logger;
        }

        [HttpGet]
        [Route("GetAllClientPaymentInterests")]
        public async Task<IActionResult> GetAllClientPaymentInterests()
        {
            try
            {
                var clientPayments = await PaymentInterestManager.GetAll();
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
        [Route("GetAllClientPaymentInterestDetailsByInterestId/{InterestId}")]
        public async Task<IActionResult> GetAllClientPaymentInterestDetailsByInterestId(int InterestId)
        {
            try
            {
                var clientPaymentInterests = await PaymentInterestManager.GetAllDetailsById(InterestId);
                if (clientPaymentInterests == null)
                {
                    return NoContent();
                }
                return Ok(clientPaymentInterests);
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
        public async Task<IActionResult> Add([FromBody] ClientPaymentInterestViewModel clientPaymentInterestViewModel)
        {
            try
            {
                if (clientPaymentInterestViewModel == null)
                {
                    return BadRequest("Client Payment Interest data is required.");
                }
                var response = await PaymentInterestManager.Add(clientPaymentInterestViewModel);
                if (response)
                {
                    var status = CreatedAtAction(nameof(Add), new { id = clientPaymentInterestViewModel.InterestId }, clientPaymentInterestViewModel);
                    return Ok(status);
                }
                else
                {
                    var status = StatusCode(StatusCodes.Status400BadRequest, "Failed to add Client Payment Interests");
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
        public async Task<IActionResult> Update([FromBody] ClientPaymentInterestViewModel clientPaymentInterestViewModel)
        {
            try
            {
                if (clientPaymentInterestViewModel == null)
                {
                    return BadRequest("Client Payment data is required.");
                }
                var response = await PaymentInterestManager.Update(clientPaymentInterestViewModel);
                if (response)
                {
                    var status = (new { message = "Client Payment Interest details are updated successfully", clientPaymentInterestViewModel });
                    return Ok(status);
                }
                else
                {
                    var status = StatusCode(StatusCodes.Status400BadRequest, "Failed to update Client Payment Interest");
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
        [Route("Delete/{interestId}")]
        public async Task<IActionResult> Delete(int interestId)
        {
            try
            {
                if (interestId == 0)
                {
                    return BadRequest("Client Payment Interest data is required.");
                }
                var response = await PaymentInterestManager.Delete(interestId);
                if (response)
                {
                    var status = (new { message = "Client Payment Interest details are deleted successfully" });
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


        [HttpGet]
        [Route("GetAllClientsPaymentInterestsPendingDetais")]
        public async Task<IActionResult> GetAllClientsPaymentInterestsPendingDetais()
        {
            try
            {
                var clientPaymentPendingList = await PaymentInterestManager.GetAllClientsPaymentInterestsPendingDetais();
                if (clientPaymentPendingList.Count == 0)
                {
                    return NoContent();
                }
                return Ok(clientPaymentPendingList);
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

