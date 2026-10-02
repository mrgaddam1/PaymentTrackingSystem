using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentTrackingSystem.Shared;
using PaymentTrackingSystem.Web.Infrastructure.Interface;

namespace PaymentTrackingSystem.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly IStateManager stateManager;
        private readonly ILogger<StateController> logger;

        public StateController(IStateManager stateManager, ILogger<StateController> logger)
        {
            this.stateManager = stateManager;
            this.logger = logger;
        }

        [HttpGet("GetAllStates")]
        public async Task<IActionResult> GetAllStates()
        {
            try
            {
                var states = await stateManager.GetAllStates();
                return states.Count == 0 ? NoContent() : Ok(states);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to get states.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("GetStateDetailsById")]
        public async Task<IActionResult> GetStateDetailsById([FromQuery] int stateId)
        {
            if (stateId <= 0)
            {
                return BadRequest("A valid state ID is required.");
            }

            try
            {
                var state = await stateManager.GetStateDetailsById(stateId);
                return state == null ? NotFound() : Ok(state);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to get state {StateId}.", stateId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] StateViewModel stateViewModel)
        {
            if (stateViewModel == null)
            {
                return BadRequest("State data is required.");
            }

            try
            {
                if (!await stateManager.Add(stateViewModel))
                {
                    return BadRequest("Failed to add state.");
                }

                return Ok(stateViewModel);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to add state.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] StateViewModel stateViewModel)
        {
            if (stateViewModel == null || stateViewModel.StateId <= 0)
            {
                return BadRequest("Valid state data is required.");
            }

            try
            {
                if (!await stateManager.Update(stateViewModel))
                {
                    return BadRequest("Failed to update state.");
                }

                return Ok(stateViewModel);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to update state {StateId}.", stateViewModel.StateId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery] int stateId)
        {
            if (stateId <= 0)
            {
                return BadRequest("A valid state ID is required.");
            }

            try
            {
                if (!await stateManager.Delete(stateId))
                {
                    return BadRequest("Failed to delete state.");
                }

                return NoContent();
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to delete state {StateId}.", stateId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
