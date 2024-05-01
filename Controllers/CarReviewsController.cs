using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoadReady.DTO;
using RoadReady.Exceptions;
using RoadReady.Repositories;

namespace RoadReady.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarReviewsController : ControllerBase
    {
        private readonly ICarReviewRepository _carReviewRepository;
        private readonly ILogger<CarReviewsController> _logger;

        public CarReviewsController(ICarReviewRepository carReviewRepository, ILogger<CarReviewsController> logger)
        {
            _carReviewRepository = carReviewRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarReviewDTO>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Retrieving all car reviews.");
                return Ok(await _carReviewRepository.GetAllAsync());
            }
            catch (CarReviewsisNotFoundException)
            {_logger.LogError("Failed to retrieve all car reviews.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarReviewDTO>> GetById(int id)
        {
            try
            {
                var carReviewDto = await _carReviewRepository.GetByIdAsync(id);
                if (carReviewDto == null)
                {
                    _logger.LogWarning($"Car review with ID: {id} not found.");
                    return NotFound();
                }
                return Ok(carReviewDto);
            }
            catch (CarReviewsisNotFoundException)
            {
                _logger.LogError($"Failed to retrieve car review with ID: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CarReviewDTO>> Create(CarReviewDTO carReviewDto)
        {
            try
            {
                _logger.LogInformation("Creating new car review.");
                var createdCarReview = await _carReviewRepository.CreateAsync(carReviewDto);
                return CreatedAtAction(nameof(GetById), new { id = createdCarReview.ReviewId }, createdCarReview);
            }
            catch (Exception)
            {
                _logger.LogError("Failed to create new car review.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CarReviewDTO>> Update(int id, CarReviewDTO carReviewDto)
        {
            try
            {
                _logger.LogInformation($"Updating car review with ID: {id}");
                if (id != carReviewDto.ReviewId)
                {
                    _logger.LogWarning("ID mismatch");
                    return BadRequest("ID mismatch");
                }

                var updatedCarReview = await _carReviewRepository.UpdateAsync(id, carReviewDto);
                if (updatedCarReview == null)
                {
                    _logger.LogWarning($"Car review with ID: {id} not found.");
                    return NotFound();
                }
                return Ok(updatedCarReview);
            }
            catch (CarReviewsisNotFoundException)
            {
                return NotFound("CarReview not found.");
            }
            catch (Exception)
            {
                _logger.LogError($"Failed to update car review with ID: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting car review with ID: {id}");
                var success = await _carReviewRepository.DeleteAsync(id);
                if (!success) return NotFound();
                return NoContent();
            }
            catch (Exception)
            {
                _logger.LogError($"Failed to delete car review with ID: {id}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
