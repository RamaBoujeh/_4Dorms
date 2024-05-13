using _4Dorms.Repositories.Interfaces;
using _4Dorms.Resources;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> AddRoom(bool? isPrivate, bool? isShared, int? numOfPrivateRooms, int? numOfSharedRooms, int? dormitoryId)
        {
            try
            {
                await _roomService.AddRoomAsync(isPrivate, isShared, numOfPrivateRooms, numOfSharedRooms, dormitoryId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
