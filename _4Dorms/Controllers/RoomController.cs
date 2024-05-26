using Microsoft.AspNetCore.Mvc;
using _4Dorms.Models;
using _4Dorms.GenericRepo;
using System.Threading.Tasks;

namespace _4Dorms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IGenericRepository<Room> _roomRepository;

        public RoomController(IGenericRepository<Room> roomRepository)
        {
            _roomRepository = roomRepository;
        }

        [HttpGet("{dormitoryId}")]
        public async Task<IActionResult> GetRoomDetails(int dormitoryId)
        {
            var room = await _roomRepository.FindByConditionAsync(r => r.DormitoryId == dormitoryId);
            if (room == null)
            {
                return NotFound();
            }
            return Ok(room);
        }

        [HttpPost("{dormitoryId}/decrement")]
        public async Task<IActionResult> DecrementRoomCount(int dormitoryId, [FromBody] DecrementRoomDTO decrementRoomDTO)
        {
            var room = await _roomRepository.FindByConditionAsync(r => r.DormitoryId == dormitoryId);
            if (room == null)
            {
                return NotFound();
            }

            if (decrementRoomDTO.IsPrivate && room.NumOfPrivateRooms > 0)
            {
                room.NumOfPrivateRooms--;
            }
            else if (!decrementRoomDTO.IsPrivate && room.NumOfSharedRooms > 0)
            {
                room.NumOfSharedRooms--;
            }
            else
            {
                return BadRequest("No rooms available to decrement.");
            }

            _roomRepository.Update(room);
            await _roomRepository.SaveChangesAsync();

            return Ok(new { success = true, room });
        }
    }

    public class DecrementRoomDTO
    {
        public bool IsPrivate { get; set; }
    }
}
