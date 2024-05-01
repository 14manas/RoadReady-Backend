using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoadReady.DTO;
using RoadReady.Exceptions;
using RoadReady.Repositories;

namespace RoadReady.Controllers
{
    [Route("api/book/")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IReservationRepository _repository;

        private readonly IPaymentDetailRepository _paymentDetailRepository;
        private readonly ILogger<BookController> _logger;

        // Assuming user ID is obtained through some means, e.g., from the logged-in user's context


        public BookController(IReservationRepository repository,IPaymentDetailRepository paymentDetailRepository, ILogger<BookController> logger)
        {
            _repository = repository;
            _paymentDetailRepository = paymentDetailRepository;
            _logger = logger;
        }

        //[HttpPost("book")]
        //public async Task<IActionResult> Book([FromBody] BookedDTO booked)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }
        //        var result = await _repository.BookReservationAsync(booked, booked.UserId);
        //        if (result.StartsWith("Reservation successful"))
        //        {
        //            return Ok(result);
        //        }
        //        return BadRequest(result);
        //    }
        //    catch (Exception ex)
        //    {

        //        return StatusCode(500, "Internal server error");
        //    }
        //}


        [HttpPost("search")]
        public async Task<IActionResult> SearchAvailableCars([FromBody] SearchDTO search)
        {
            try
            {
                _logger.LogInformation($"Search request received for {search.LocationName} from {search.StartDate} to {search.EndDate}");
                var searchdata = new SearchDTO()
                {
                    StartDate = search.StartDate.Date,
                    EndDate = search.EndDate.Date,
                    LocationName = search.LocationName,
                };
                var availableCars = await _repository.SearchAvailableCarsAsync(searchdata);
                if (availableCars != null && availableCars.Count > 0)
                {
                    return Ok(availableCars);
                }
                return NotFound("No available cars found for the specified location and date range.");
            }
            catch (CarNotisFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while searching available cars");
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

       // [Authorize(Roles ="admin, caragent, customer")]
        [HttpPost("PlaceBooking/{paymentMethod}")]
        public async Task<IActionResult> PlaceBooking([FromBody] BookedDTO bookingDto,[FromRoute] string paymentMethod)
        {
            var bookResult = await _repository.BookReservationAsync(bookingDto, bookingDto.UserId);
            if (!bookResult.StartsWith("Reservation successful"))
                return BadRequest(bookResult);

            // Extract reservation ID and total cost from the result message
            var parts = bookResult.Split(':');
            int reservationId = int.Parse(parts[1].Split('.')[0].Trim());
            decimal totalCost = decimal.Parse(parts[2].Trim());

            // Create payment detail (simplifying payment creation)
            var paymentDetail = new PaymentDetailDTO
            {
                ReservationId = reservationId,
                Amount= totalCost,
                PaymentStatus = "Pending",
                PaymentMethod = paymentMethod
            };

            try
            {
                // Assuming the payment processing differs based on the method
                if (paymentMethod == "Online")
                {
                    _logger.LogInformation("Booking placed successfully");
                    // Simulate online payment processing
                    paymentDetail.PaymentStatus = "Paid";

                    await _paymentDetailRepository.CreateAsync(paymentDetail);
                    return Ok("Online payment successful and booking confirmed.");
                    
                }
                else if (paymentMethod == "Onsite")
                {
                    // Onsite payment might be processed later
                    paymentDetail.PaymentStatus = "Pending";
                    paymentDetail.PaymentDate = DateTime.Now;
                    await _paymentDetailRepository.CreateAsync(paymentDetail);
                    _logger.LogError("Error occurred while placing booking");
                    return Ok("Booking confirmed. Please pay onsite.");
                }
                else
                {
                    _logger.LogError("Error occurred while placing booking");
                    return BadRequest("Invalid payment method.");
                }
            }
            catch
            {
                // Attempt to roll back the reservation
                _logger.LogError("Error occurred while placing booking");
                await _repository.DeleteAsync(reservationId);
                return BadRequest("Payment failed. Booking rolled back.");
            }
        }

    }
}
