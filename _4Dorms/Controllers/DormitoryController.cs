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

        [HttpGet("dormitory/{id}")]
        public async Task<IActionResult> GetDormitoryById(int id)
        {
            var dormitory = await _dormitoryService.GetDormitoryByIdAsync(id);
            if (dormitory == null)
            {
                return NotFound("Dormitory not found.");
            }
            return Ok(dormitory);
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

        [HttpGet("approved")]
        public async Task<ActionResult<List<Dormitory>>> GetApprovedDormsAsync()
        {
            try
            {
                var approvedDorms = await _dormitoryService.GetDormsByStatusAsync(DormitoryStatus.Approved);
                return Ok(approvedDorms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("dormitoryDetails/{dormitoryId}")]
        public async Task<IActionResult> GetDormitoryDetails(int dormitoryId)
        {
            var dormitory = await _dormitoryService.GetDormitoryDetailsAsync(dormitoryId);
            if (dormitory == null)
            {
                return NotFound();
            }
            return Ok(dormitory);
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
