using _4Dorms.Repositories.Interfaces;
using _4Dorms.Resources;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace _4Dorms.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddRoom(RoomDTO room)
        {
            try
            {
                await _roomService.AddRoomAsync(room);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{dormitoryId}")]
        public async Task<IActionResult> GetRoomDetails(int dormitoryId)
        {
            try
            {
                var roomDetails = await _roomService.GetRoomDetailsByDormitoryIdAsync(dormitoryId);
                if (roomDetails == null)
                {
                    return NotFound();
                }
                return Ok(roomDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("{dormitoryId}/decrement")]
        public async Task<IActionResult> DecrementRoomAvailability(int dormitoryId, bool isPrivate)
        {
            try
            {
                var success = await _roomService.DecrementRoomAvailabilityAsync(dormitoryId, isPrivate);
                if (success)
                {
                    return Ok(new { success = true, message = "Room availability updated." });
                }
                return BadRequest(new { success = false, message = "Failed to update room availability." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
