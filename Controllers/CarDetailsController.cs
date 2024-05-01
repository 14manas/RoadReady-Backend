using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoadReady.DTO;
using RoadReady.Exceptions;
using RoadReady.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoadReady.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarDetailsController : ControllerBase
    {
        private readonly ICarDetailRepository _carDetailRepository;
        private readonly ILogger<CarDetailsController> _logger;

        public CarDetailsController(ICarDetailRepository carDetailRepository, ILogger<CarDetailsController> logger)
        {
            _carDetailRepository = carDetailRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarDetailDTO>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Retrieving all car details.");
                return Ok(await _carDetailRepository.GetAllAsync());
            }
            catch (CarDetailsisNotFoundException)
            {
                _logger.LogError("Failed to retrieve all car details.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarDetailDTO>> GetById(int id)
        {
            try
            {
                _logger.LogInformation($"Retrieving car detail with ID: {id}");
                var carDetailDto = await _carDetailRepository.GetByIdAsync(id);
                if (carDetailDto == null) return NotFound();
                return Ok(carDetailDto);
            }
            catch (CarDetailsisNotFoundException)
            {
                _logger.LogWarning($"Car detail with ID: {id} not found.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
       // [Authorize(Roles = "admin, caragent")]
        public async Task<ActionResult<CarDetailDTO>> Create(CarDetailDTO carDetailDto)
        {
            try
            {
                _logger.LogInformation("Creating new car detail.");
                var createdCarDetail = await _carDetailRepository.CreateAsync(carDetailDto);
                return CreatedAtAction(nameof(GetById), new { id = createdCarDetail.CarDetailId }, createdCarDetail);
            }
            catch (Exception)
            {
                _logger.LogError("Failed to create new car detail.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
      //  [Authorize(Roles = "admin, caragent")]
        public async Task<ActionResult<CarDetailDTO>> Update(int id, CarDetailDTO carDetailDto)
        {
            try
            {
                _logger.LogInformation($"Updating car detail with ID: {id}");
                if (id != carDetailDto.CarDetailId)
                {
                    _logger.LogWarning("ID mismatch");
                    return BadRequest("ID mismatch");
                }

                var updatedCarDetail = await _carDetailRepository.UpdateAsync(id, carDetailDto);
                if (updatedCarDetail == null) return NotFound();
                {
                    _logger.LogWarning($"Car detail with ID: {id} not found.");

                    return Ok(updatedCarDetail);
                }
            }
            catch (KeyNotFoundException)
            {
                _logger.LogError($"Failed to update car detail with ID: {id}");
                return NotFound("CarDetail not found.");
            }
            catch (Exception)
            {
                _logger.LogError($"Failed to update car detail with ID: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
      //  [Authorize(Roles = "admin, caragent")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting car detail with ID: {id}");
                var success = await _carDetailRepository.DeleteAsync(id);
                if (!success) return NotFound();
                return NoContent();
            }
            catch (Exception)
            {
                _logger.LogError($"Failed to delete car detail with ID: {id}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
