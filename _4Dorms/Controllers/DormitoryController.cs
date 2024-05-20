using _4Dorms.Models;
using _4Dorms.Repositories.Interfaces;
using _4Dorms.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _4Dorms.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class DormitoryController : ControllerBase
    {
        private readonly IDormitoryService _dormitoryService;
        public DormitoryController(IDormitoryService dormitoryService)
        {
            _dormitoryService = dormitoryService;  
        }

        [HttpPost("select")]
        public async Task<IActionResult> SelectDormitory(int dormitoryId)
        {
            bool success = await _dormitoryService.SelectDormitoryAsync(dormitoryId);

            if (success)
            {
                return Ok("Dormitory selected successfully.");
            }
            else
            {
                return NotFound("Dormitory not found or selection failed.");
            }
        }

        [HttpGet("pending")]
        public async Task<ActionResult<List<Dormitory>>> GetPendingDormsAsync()
        {
            try
            {
                var pendingDorms = await _dormitoryService.GetDormsByStatusAsync(DormitoryStatus.Pending);
                return Ok(pendingDorms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("search")]
        public async Task<ActionResult<List<Dormitory>>> SearchDormitoriesAsync([FromQuery] string keywords, [FromQuery] string city, [FromQuery] string nearbyUniversity, [FromQuery] string genderType)
        {
            try
            {
                var dormitories = await _dormitoryService.SearchDormitoriesAsync(keywords, city, nearbyUniversity, genderType);
                return Ok(dormitories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
