using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoadReady.DTO;
using RoadReady.Exceptions;
using RoadReady.Repositories;

namespace RoadReady.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserRepository userRepository, ILogger<UsersController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all users");
                return Ok(await _userRepository.GetAllAsync());
            }
            catch (UserisNotFoundException)
            {
                _logger.LogError( "Error occurred while fetching all users");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching user with ID: {Id}", id);
                var userDto = await _userRepository.GetByIdAsync(id);
                if (userDto == null)
                {
                    _logger.LogWarning("User with ID: {Id} not found", id);
                    return NotFound();
                }
                return Ok(userDto);
            }
            catch (UserisNotFoundException)
            {
                _logger.LogError( "Error occurred while fetching user with ID: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
       // [Authorize(Roles = "admin")]
        public async Task<ActionResult<UserDTO>> Create(UserDTO userDto)
        {
            try
            {
                var createdUser = await _userRepository.CreateAsync(userDto);
                return CreatedAtAction(nameof(GetById), new { id = createdUser.UserId }, createdUser);
            }
            catch (Exception)
            {
                _logger.LogError("Error occurred while creating a new user");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
      //  [Authorize(Roles = "admin, caragent, customer")]
        public async Task<ActionResult<UserDTO>> Update(int id, UserDTO userDto)
        {
            try
            {
                _logger.LogInformation("Updating user with ID: {Id}", id);
                if (id != userDto.UserId)
                {
                    _logger.LogWarning("ID mismatch");
                    return BadRequest("ID mismatch");
                }

                var updatedUser = await _userRepository.UpdateAsync(id, userDto);
                if (updatedUser == null)
                {
                    _logger.LogWarning("User with ID: {Id} not found", id);
                    return NotFound("User not found.");
                }

                return Ok(updatedUser);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("User not found.");
            }
            catch (Exception)
            {
                _logger.LogError("Error occurred while updating user with ID: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
       // [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Deleting user with ID: {Id}", id);
                var success = await _userRepository.DeleteAsync(id);
                if (!success)
                {
                    _logger.LogWarning("User with ID: {Id} not found", id);
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception)
            {
                _logger.LogError("Error occurred while deleting user with ID: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

