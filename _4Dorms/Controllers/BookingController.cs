using _4Dorms.Repositories.Interfaces;
using _4Dorms.Resources;
using Microsoft.AspNetCore.Mvc;

namespace _4Dorms.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestBooking(BookingDTO bookingDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _bookingService.RequestBookingAsync(bookingDTO);
            if (result)
            {
                return Ok("Booking request submitted successfully.");
            }
            else
            {
                return StatusCode(500, "Failed to submit booking request.");
            }
        }

        [HttpPost("confirm/{bookingId}")]
        public async Task<IActionResult> ConfirmBooking(int bookingId, bool isApproved, int dormitoryOwnerId)
        {
            var result = await _bookingService.ConfirmBookingAsync(bookingId, isApproved, dormitoryOwnerId);
            if (result)
            {
                return Ok(isApproved ? "Booking confirmed." : "Booking rejected.");
            }
            else
            {
                return StatusCode(500, "Failed to confirm booking.");
            }
        }
    }
}
