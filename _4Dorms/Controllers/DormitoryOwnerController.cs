using _4Dorms.Models;
using _4Dorms.Repositories.Interfaces;
using _4Dorms.Resources;
using Microsoft.AspNetCore.Mvc;

namespace _4Dorms.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class DormitoryOwnerController : ControllerBase
    {
        private readonly IDormitoryOwnerService _dormitoryOwnerService;

        public DormitoryOwnerController(IDormitoryOwnerService dormitoryOwnerService)
        {
            _dormitoryOwnerService = dormitoryOwnerService;
        }

        [HttpPost("submit-dormitory")]
        public async Task<IActionResult> SubmitDormitoryForApproval([FromBody] DormitoryDTO dormitoryDTO, int dormitoryOwnerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _dormitoryOwnerService.SubmitDormitoryForApprovalAsync(dormitoryDTO, dormitoryOwnerId);
            return Ok("Dormitory submitted for approval successfully");
        }

        [HttpPut("update-dormitory/{dormitoryId}")]
        public async Task<IActionResult> UpdateDormitory(int dormitoryId, [FromBody] DormitoryDTO updatedDormitoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _dormitoryOwnerService.UpdateDormitoryAsync(dormitoryId, updatedDormitoryDTO);
                return Ok("Dormitory updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the dormitory: {ex.Message}");
            }
        }

        [HttpDelete("delete-dormitory/{dormitoryId}")]
        public async Task<IActionResult> DeleteDormitory(int dormitoryId)
        {
            try
            {
                await _dormitoryOwnerService.DeleteDormitoryAsync(dormitoryId);
                return Ok("Dormitory deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the dormitory: {ex.Message}");
            }
        }
    }
}
