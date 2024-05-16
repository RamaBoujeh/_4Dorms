using _4Dorms.Repositories.Interfaces;
using _4Dorms.Resources;
using Microsoft.AspNetCore.Mvc;

namespace _4Dorms.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentGateController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentGateController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayment(PaymentGateDTO paymentDto)
        {
            var result = await _paymentService.ProcessPayment(paymentDto.CardNumber, paymentDto.ExpirationDate, paymentDto.CVV, paymentDto.Amount);
            if (result)
            {
                return Ok(new { Message = "Payment processed successfully." });
            }
            return BadRequest(new { Message = "Payment processing failed." });
        }
    }
}
