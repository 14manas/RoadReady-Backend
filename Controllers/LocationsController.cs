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
    public class LocationsController : ControllerBase
    {
        private readonly ILocationRepository _locationRepository;
        private readonly ILogger<LocationsController> _logger;

        public LocationsController(ILocationRepository locationRepository, ILogger<LocationsController> logger)
        {
            _locationRepository = locationRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocationDTO>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Retrieving all locations.");
                return Ok(await _locationRepository.GetAllAsync());
            }
            catch (LocationFoundException)
            {
                _logger.LogError("Failed to retrieve all locations.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LocationDTO>> GetById(int id)
        {
            try
            {
                _logger.LogInformation($"Retrieving location with ID: {id}");
                var locationDto = await _locationRepository.GetByIdAsync(id);
                if (locationDto == null)
                {
                    _logger.LogWarning($"Location with ID: {id} not found.");
                    return NotFound();
                }
                return Ok(locationDto);
            }
            catch (LocationFoundException)
            {
                _logger.LogError($"Failed to retrieve location with ID: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
       // [Authorize(Roles = "admin")]
        public async Task<ActionResult<LocationDTO>> Create(LocationDTO locationDto)
        {
            try
            {
                _logger.LogInformation("Creating new location.");
                var createdLocation = await _locationRepository.CreateAsync(locationDto);
                return CreatedAtAction(nameof(GetById), new { id = createdLocation.LocationId }, createdLocation);
            }
            catch (Exception)
            {
                _logger.LogError("Failed to create new location.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<LocationDTO>> Update(int id, LocationDTO locationDto)
        {
            try
            {
                _logger.LogInformation($"Updating location with ID: {id}");
                if (id != locationDto.LocationId)
                {
                    _logger.LogWarning("ID mismatch");
                    return BadRequest("ID mismatch");
                }

                var updatedLocation = await _locationRepository.UpdateAsync(id, locationDto);
                if (updatedLocation == null)
                {
                    _logger.LogWarning($"Location with ID: {id} not found.");
                    return NotFound();
                }
                return Ok(updatedLocation);
            }
            catch (LocationFoundException)
            {
                _logger.LogWarning($"Location with ID: {id} not found.");
                return NotFound("Location not found.");
            }
            catch (Exception)
            {
                _logger.LogError($"Failed to update location with ID: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting location with ID: {id}");
                var success = await _locationRepository.DeleteAsync(id);
                if (!success)
                {
                    _logger.LogWarning($"Location with ID: {id} not found.");
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception)
            {
                _logger.LogError($"Failed to delete location with ID: {id}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

