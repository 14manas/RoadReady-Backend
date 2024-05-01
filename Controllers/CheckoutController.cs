using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Razorpay.Api;
using Newtonsoft;
using Microsoft.AspNetCore.Mvc;
using Razorpay.Api.Errors;

using Newtonsoft.Json.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Collections.Generic;
using RoadReady.Repositories;
using RoadReady.DTO;
namespace RoadReady.Controllers
{
    [Route("api/checkout")]
    public class CheckoutController : ControllerBase
    {

        private readonly string apiKey = "rzp_live_eSbRcu4kQn8Sm3";
        private readonly string apiSecret = "AJcGwBS3OuidpfkPs5effJqW";
        private readonly IPaymentDetailRepository _paymentDetailRepository;
        private readonly IReservationRepository _reservationsRepository;

        public CheckoutController(IReservationRepository reservation, IPaymentDetailRepository paymentDetailRepository)
        {
            _reservationsRepository = reservation;
            _paymentDetailRepository = paymentDetailRepository;

        }


        [HttpPost("create-order")]
        //[Authorize]
        public IActionResult CreateOrder([FromBody] ra data)
        {
            try
            {
                int userId = data.userId;
                int carId = data.carId;
                DateTime startDate = data.startDate;
                DateTime endDate = data.endDate;

                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", data.amount * 100); // Razorpay expects amount in paisa/cent
                options.Add("currency", data.currency);
                options.Add("payment_capture", 1); // Auto capture payment

                RazorpayClient client = new RazorpayClient(apiKey, apiSecret);
                Razorpay.Api.Order order = client.Order.Create(options);
                string orderId = order["id"].ToString();
                var key = apiKey;
                var amount = data.amount;
                var currency = data.currency;
                var name = "RoadReady";
                var callbackurl = "https://localhost:7250/api/checkout/verify-payment/?userId=" + userId + "&carid=" + carId + "&startDate=" + startDate + "&endDate=" + endDate;



                return Ok(new { order_id = orderId, amount = amount, key = key, currency = currency, name = name, callback_url = callbackurl });
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost("verify-payment")]
        public async Task<IActionResult> VerifyPayment()
        {
            try
            {
                int userId = int.Parse(Request.Query["userId"]);
                int carId = int.Parse(Request.Query["carid"]);
                DateTime startDate = DateTime.Parse(Request.Query["startDate"]);
                DateTime returnDate = DateTime.Parse(Request.Query["endDate"]);

                Dictionary<string, string> attributes = new Dictionary<string, string>();

                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    string requestBody = await reader.ReadToEndAsync();

                    var formData = ParseFormData(requestBody);

                    string razorpayOrderId = formData["razorpay_order_id"];
                    string razorpayPaymentId = formData["razorpay_payment_id"];
                    string razorpaySignature = formData["razorpay_signature"];

                    attributes.Add("razorpay_order_id", razorpayOrderId);
                    attributes.Add("razorpay_payment_id", razorpayPaymentId);
                    attributes.Add("razorpay_signature", razorpaySignature);
                }

                Utils.verifyPaymentSignature(attributes);

                BookedDTO bookedDTO = new BookedDTO()
                {
                    UserId = userId,
                    CarId = carId,
                    StartDate = startDate,
                    ReturnDate = returnDate,
                };

                var res = await _reservationsRepository.BookReservationAsync(bookedDTO, userId);

                //we have to have an refind in that i need to populate and then mark the payment need to refund
                var parts = res.Split(':');
                int reservationId = int.Parse(parts[1].Split('.')[0].Trim());
                decimal totalCost = decimal.Parse(parts[2].Trim());

                // Create payment detail (simplifying payment creation)
                var paymentDetail = new PaymentDetailDTO
                {
                    ReservationId = reservationId,
                    Amount = totalCost,
                    PaymentStatus = "Completed",
                    PaymentMethod = "Online"
                };
                paymentDetail.PaymentStatus = "Paid";
                paymentDetail.PaymentDate = DateTime.Now;
                await _paymentDetailRepository.CreateAsync(paymentDetail);
                return Ok("Online payment successful and booking confirmed.");

               // return Ok(res);
            }
            catch (SignatureVerificationError ex)
            {

                return BadRequest("Payment verification failed");
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private Dictionary<string, string> ParseFormData(string formData)
        {
            var pairs = formData.Split('&');
            var dict = new Dictionary<string, string>();
            foreach (var pair in pairs)
            {
                var keyValue = pair.Split('=');
                dict.Add(Uri.UnescapeDataString(keyValue[0]), Uri.UnescapeDataString(keyValue[1]));
            }
            return dict;
        }


    }
}