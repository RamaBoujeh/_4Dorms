using _4Dorms.Models;
using _4Dorms.Repositories.Interfaces;
using _4Dorms.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        [Authorize(Roles = "DormitoryOwner")]
        [HttpPost("submit-dormitory")]
        public async Task<IActionResult> SubmitDormitoryForApproval([FromBody] DormitoryDTO dormitoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = int.Parse(User.FindFirst("UserId").Value);
            await _dormitoryOwnerService.SubmitDormitoryForApprovalAsync(dormitoryDTO, userId);
            return Ok("Dormitory submitted for approval successfully");
        }

        [Authorize(Roles = "DormitoryOwner")]
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

        [Authorize(Roles = "DormitoryOwner")]
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
