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

        [HttpGet("search")]
        public async Task<IActionResult> SearchDormitories(string dormName = null, string location = null, decimal? price = null, bool sortByLowToHigh = true)
        {
            try
            {
                var dormitories = await _dormitoryService.SearchDormitoriesAsync(dormName, location, price, sortByLowToHigh);
                return Ok(dormitories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

    }
}
