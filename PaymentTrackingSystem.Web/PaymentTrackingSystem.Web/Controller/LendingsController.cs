using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentTrackingSystem.Shared;
using PaymentTrackingSystem.Web.Infrastructure.Interface;

namespace PaymentTrackingSystem.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LendingsController : ControllerBase
    {
        private readonly ILogger<LendingsController> logger;
        private ILendingManager LendingManager { get; set; }

        public LendingsController(ILendingManager _LendingManager, ILogger<LendingsController> _logger)
        {
            LendingManager = _LendingManager;
            logger = _logger;
        }

        [HttpGet]
        [Route("GetAllLendings")]
        public async Task<IActionResult> GetAllLendings()
        {
            try
            {
                var lendings = await LendingManager.GetAllLendings();
                if (lendings.Count == 0)
                {
                    return NoContent();
                }
                return Ok(lendings);
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
        [Route("GetLendingDetailsById/{lendingId}")]
        public async Task<IActionResult> GetLendingDetailsById(int lendingId)
        {
            try
            {
                var lending = await LendingManager.GetLendingDetailsById(lendingId);
                if (lending == null)
                {
                    return NoContent();
                }
                return Ok(lending);
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
        public async Task<IActionResult> Add([FromBody] LendingViewModel lendingViewModel)
        {
            try
            {
                if (lendingViewModel == null)
                {
                    return BadRequest("Lending data is required.");
                }
                var response = await LendingManager.Add(lendingViewModel);
                if (response)
                {
                    return CreatedAtAction(nameof(Add), new { id = lendingViewModel.LendingId }, lendingViewModel);
                }
                else
                {
                    return BadRequest("Failed to add lending.");
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

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] LendingViewModel lendingViewModel)
        {
            try
            {
                if (lendingViewModel == null)
                {
                    return BadRequest("Lending data is required.");
                }
                var response = await LendingManager.Update(lendingViewModel);
                if (response)
                {
                    var status = new { message = "Lending details updated successfully", lendingViewModel };
                    return Ok(status);
                }
                else
                {
                    return BadRequest("Failed to update lending.");
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
        [Route("Delete/{lendingId}")]
        public async Task<IActionResult> Delete(int lendingId)
        {
            try
            {
                if (lendingId == 0)
                {
                    return BadRequest("Lending ID is required.");
                }
                var response = await LendingManager.Delete(lendingId);
                if (response)
                {
                    var status = new { message = "Lending deleted successfully" };
                    return Ok(status);
                }
                else
                {
                    return BadRequest("Failed to delete lending.");
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

        [HttpGet]
        [Route("GetAllPaymentModes")]
        public async Task<IActionResult> GetAllPaymentModes()
        {
            try
            {
                var paymentModes = await LendingManager.GetPaymentModes();
                if (paymentModes.Count == 0)
                {
                    return NoContent();
                }
                return Ok(paymentModes);
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
        [Route("GetAllLendingDueDateDescriptions")]
        public async Task<IActionResult> GetAllLendingDueDateDescriptions()
        {
            try
            {
                var dueDateDescription = await LendingManager.GetAllLendingDueDateDescriptions();
                if (dueDateDescription.Count == 0)
                {
                    return NoContent();
                }
                return Ok(dueDateDescription);
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
        [Route("GetAllInterestRates")]
        public async Task<IActionResult> GetAllInterestRates()
        {
            try
            {
                var interestRateData = await LendingManager.GetAllInterestRates();
                if (interestRateData.Count == 0)
                {
                    return NoContent();
                }
                return Ok(interestRateData);
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