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
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ILogger<ReservationsController> _logger;

        public ReservationsController(IReservationRepository reservationRepository, ILogger<ReservationsController> logger)
        {
            _reservationRepository = reservationRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationDTO>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Retrieving all reservations.");
                return Ok(await _reservationRepository.GetAllAsync());
            }
            catch (ReservationNotFoundException)
            {
                _logger.LogError("Failed to retrieve all reservations.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReservationDTO>> GetById(int id)
        {
            try
            {
                _logger.LogInformation($"Retrieving reservation with ID: {id}");
                var reservationDto = await _reservationRepository.GetByIdAsync(id);
                if (reservationDto == null)
                {
                    _logger.LogWarning($"Reservation with ID: {id} not found.");
                    return NotFound();
                }
                return Ok(reservationDto);
            }
            catch (ReservationNotFoundException)
            {
                _logger.LogError($"Failed to retrieve reservation with ID: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        //[HttpPost]
        //public async Task<ActionResult<ReservationDTO>> Create(ReservationDTO reservationDto)
        //{
        //    try
        //    {
        //        var createdReservation = await _reservationRepository.CreateAsync(reservationDto);
        //        return CreatedAtAction(nameof(GetById), new { id = createdReservation.ReservationId }, createdReservation);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ReservationDTO>> Update(int id, ReservationDTO reservationDto)
        {
            try
            {
                _logger.LogInformation($"Updating reservation with ID: {id}");
                if (id != reservationDto.ReservationId)
                {
                    _logger.LogWarning("ID mismatch");
                    return BadRequest("ID mismatch");
                }

                var updatedReservation = await _reservationRepository.UpdateAsync(id, reservationDto);
                if (updatedReservation == null)
                {
                    _logger.LogWarning($"Reservation with ID: {id} not found.");
                    return NotFound();
                }

                return Ok(updatedReservation);
            }
            catch (ReservationNotFoundException)
            {
                _logger.LogWarning($"Reservation with ID: {id} not found.");
                return NotFound("Reservation not found.");
            }
            catch (Exception)
            {
                _logger.LogError($"Failed to update reservation with ID: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting reservation with ID: {id}");
                var success = await _reservationRepository.DeleteAsync(id);
                if (!success)
                {
                    _logger.LogWarning($"Reservation with ID: {id} not found.");
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception)
            {
                _logger.LogError($"Failed to delete reservation with ID: {id}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

