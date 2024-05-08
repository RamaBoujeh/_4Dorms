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
        public async Task<IActionResult> AddRoom(RoomDTO roomDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _roomService.AddRoomAsync(roomDTO);
            if (result)
            {
                return Ok("Room added successfully.");
            }
            else
            {
                return StatusCode(500, "Failed to add room.");
            }
        }
    }
}
