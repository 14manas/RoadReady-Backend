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
    public class PaymentDetailsController : ControllerBase
    {
        private readonly IPaymentDetailRepository _paymentDetailRepository;
        private readonly ILogger<PaymentDetailsController> _logger;

        public PaymentDetailsController(IPaymentDetailRepository paymentDetailRepository, ILogger<PaymentDetailsController> logger)
        {
            _paymentDetailRepository = paymentDetailRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDetailDTO>>> GetAll()
        {
            try
            {
                return Ok(await _paymentDetailRepository.GetAllAsync());
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDetailDTO>> GetById(int id)
        {
            try
            {
                var paymentDetailDto = await _paymentDetailRepository.GetByIdAsync(id);
                if (paymentDetailDto == null) return NotFound();
                return Ok(paymentDetailDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        //[HttpPost]
        //public async Task<ActionResult<PaymentDetailDTO>> Create(PaymentDetailDTO paymentDetailDto)
        //{
        //    try
        //    {
        //        var createdPaymentDetail = await _paymentDetailRepository.CreateAsync(paymentDetailDto);
        //        return CreatedAtAction(nameof(GetById), new { id = createdPaymentDetail.PaymentId }, createdPaymentDetail);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<PaymentDetailDTO>> Update(int id, PaymentDetailDTO paymentDetailDto)
        {
            try
            {
                if (id != paymentDetailDto.PaymentId)
                {
                    return BadRequest("ID mismatch");
                }

                var updatedPaymentDetail = await _paymentDetailRepository.UpdateAsync(id, paymentDetailDto);
                if (updatedPaymentDetail == null) return NotFound();

                return Ok(updatedPaymentDetail);
            }
            catch (PaymentNotFoundException)
            {
                return NotFound("PaymentDetail not found.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _paymentDetailRepository.DeleteAsync(id);
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
