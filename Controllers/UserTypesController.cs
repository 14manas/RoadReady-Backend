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


    public class UserTypesController : ControllerBase
    {
        private readonly IUserTypeRepository _userTypeRepository;
        private readonly ILogger<UserTypesController> _logger;

        public UserTypesController(IUserTypeRepository userTypeRepository, ILogger<UserTypesController> logger)
        {
            _userTypeRepository = userTypeRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserTypeDTO>>> GetAll()
        {
            try
            {
                return Ok(await _userTypeRepository.GetAllAsync());
            }
            catch (UserTypeisFoundException)
            {
                _logger.LogError("An error occurred while fetching all user types.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserTypeDTO>> GetById(int id)
        {
            try
            {
                var userTypeDto = await _userTypeRepository.GetByIdAsync(id);
                if (userTypeDto == null)
                {
                    _logger.LogInformation($"User type with ID {id} not found.");
                    return NotFound();
                }
                return Ok(userTypeDto);
            }
            catch (UserTypeisFoundException)
            {
                _logger.LogError($"An error occurred while fetching user type with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]

       // [Authorize(Roles = "admin")]
        public async Task<ActionResult<UserTypeDTO>> Create(UserTypeDTO userTypeDto)
        {
            try
            {
                var createdUserType = await _userTypeRepository.CreateAsync(userTypeDto);
                return CreatedAtAction(nameof(GetById), new { id = createdUserType.Usertypeid }, createdUserType);
            }
            catch (Exception)
            {
                _logger.LogError("An error occurred while creating a new user type.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<UserTypeDTO>> Update(int id, UserTypeDTO userTypeDto)
        {
            try
            {
                if (id != userTypeDto.Usertypeid)
                {
                    _logger.LogWarning($"ID mismatch in update request. Requested ID: {id}, UserType ID: {userTypeDto.Usertypeid}");
                    return BadRequest("ID mismatch");
                }

                var updatedUserType = await _userTypeRepository.UpdateAsync(id, userTypeDto);
                if (updatedUserType == null)
                {
                    _logger.LogInformation($"User type with ID {id} not found for update.");
                    return NotFound();
                }

                return Ok(updatedUserType);
            }
            catch (KeyNotFoundException)
            {
                _logger.LogInformation($"User type with ID {id} not found ");
                return NotFound("UserType not found.");
            }
            catch (Exception)
            {
                _logger.LogError($"An error occurred while updating user type with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _userTypeRepository.DeleteAsync(id);
                if (!success)
                {
                    _logger.LogInformation($"User type with ID {id} not found for deletion.");
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception)
            {
                _logger.LogError($"An error occurred while deleting user type with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

