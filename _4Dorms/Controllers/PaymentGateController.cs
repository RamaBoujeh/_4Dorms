using _4Dorms.Repositories.Interfaces;
using _4Dorms.Resources;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentGateDTO paymentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var result = await _paymentService.ProcessPaymentAsync(paymentDto);
            if (result)
            {
                return Ok(new { success = true, message = "Payment processed successfully." });
            }
            return BadRequest(new { success = false, message = "Payment processing failed. Please check your payment information." });
        }
    }
}
