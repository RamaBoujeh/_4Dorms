using _4Dorms.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> RequestBooking([FromBody] BookingDTO bookingDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _bookingService.BookingAsync(bookingDTO);
            if (result)
            {
                return Ok(new { bookingId = bookingDTO.RoomId }); // Return booking ID
            }
            else
            {
                return StatusCode(500, "Failed to submit booking request.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("hasBooking/{dormitoryId}/{studentId}")]
    public async Task<IActionResult> HasBooking(int dormitoryId, int studentId)
    {
        var hasBooking = await _bookingService.HasCompletedBookingAsync(dormitoryId, studentId);
        if (hasBooking)
        {
            return Ok(true);
        }
        return Ok(false);
    }
}
