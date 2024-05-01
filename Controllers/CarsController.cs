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
    public class CarsController : ControllerBase
    {
        private readonly ICarRepository _carRepository;
        private readonly ILogger<CarsController> _logger;

        public CarsController(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarDTO>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Retrieving all cars.");
                return Ok(await _carRepository.GetAllAsync());
            }
            catch (CarSearchedNotFoundException ex)
            {
                _logger.LogError(ex, "Failed to retrieve all cars.");
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarDTO>> GetById(int id)
        {
            try
            {
                _logger.LogInformation($"Retrieving car with ID: {id}");
                var carDto = await _carRepository.GetByIdAsync(id);
                if (carDto == null)
                {
                    _logger.LogWarning($"Car with ID: {id} not found.");
                    return NotFound();

                }
                return Ok(carDto);
            }
            catch (CarSearchedNotFoundException)
            {
                _logger.LogInformation($"Retrieving car with ID: {id}");
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }
       // [Authorize(Roles = "admin, caragent")]
        [HttpPost]
        public async Task<ActionResult<CarDTO>> Create(CarDTO carDto)
        {
            try
            {
                _logger.LogInformation("Creating new car.");
                var createdCar = await _carRepository.CreateAsync(carDto);
                return CreatedAtAction(nameof(GetById), new { id = createdCar.CarId }, createdCar);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create new car.");
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
       // [Authorize(Roles = "admin, caragent")]
        public async Task<ActionResult<CarDTO>> Update(int id, CarDTO carDto)
        {
            try
            {
                _logger.LogInformation($"Updating car with ID: {id}");
                if (id != carDto.CarId)
                {
                    _logger.LogWarning("ID mismatch");
                    return BadRequest("ID mismatch");
                }

                var updatedCar = await _carRepository.UpdateAsync(id, carDto);
                if (updatedCar == null)
                {
                    _logger.LogWarning($"Car with ID: {id} not found.");
                    return NotFound();
                }

                return Ok(updatedCar);
            }
            catch (CarSearchedNotFoundException ex)
            {
                _logger.LogWarning($"Car with ID: {id} not found.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update car with ID: {id}");
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
       // [Authorize(Roles = "admin, caragent")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting car with ID: {id}");
                var success = await _carRepository.DeleteAsync(id);
                if (!success)
                {
                    _logger.LogWarning($"Car with ID: {id} not found.");
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete car with ID: {id}");
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

