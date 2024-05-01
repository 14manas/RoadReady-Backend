using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoadReady.DTO;
using RoadReady.Repositories;

namespace RoadReady.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class StatesController : ControllerBase
    {
        private readonly IStateRepository _stateRepository;
        private readonly ILogger<StatesController> _logger;

        public StatesController(IStateRepository stateRepository, ILogger<StatesController> logger)
        {
            _stateRepository = stateRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StateDTO>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Retrieving all states.");
                return Ok(await _stateRepository.GetAllAsync());
            }
            catch (Exception)
            {
                _logger.LogError("Failed to retrieve all states.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StateDTO>> GetById(int id)
        {
            try
            {
                _logger.LogInformation($"Retrieving state with ID: {id}");
                var stateDto = await _stateRepository.GetByIdAsync(id);
                if (stateDto == null)
                {
                    _logger.LogWarning($"State with ID: {id} not found.");
                    return NotFound();
                }
                return Ok(stateDto);
            }
            catch (Exception)
            {
                _logger.LogError($"Failed to retrieve state with ID: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
       // [Authorize(Roles = "admin")]
        public async Task<ActionResult<StateDTO>> Create(StateDTO stateDto)
        {
            try
            {
                _logger.LogInformation("Creating new state.");
                var createdState = await _stateRepository.CreateAsync(stateDto);
                return CreatedAtAction(nameof(GetById), new { id = createdState.Stateid }, createdState);
            }
            catch (Exception)
            {
                _logger.LogError("Failed to create new state.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<StateDTO>> Update(int id, StateDTO stateDto)
        {
            try
            {
                _logger.LogInformation($"Updating state with ID: {id}");
                if (id != stateDto.Stateid)
                {
                    _logger.LogWarning("ID mismatch");
                    return BadRequest("ID mismatch");
                }

                var updatedState = await _stateRepository.UpdateAsync(id, stateDto);
                if (updatedState == null)
                {
                    _logger.LogWarning($"State with ID: {id} not found.");
                    return NotFound();
                }

                return Ok(updatedState);
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning($"State with ID: {id} not found.");
                return NotFound("State not found.");
            }
            catch (Exception)
            {
                _logger.LogError($"Failed to update state with ID: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _stateRepository.DeleteAsync(id);
                if (!success) return NotFound();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

