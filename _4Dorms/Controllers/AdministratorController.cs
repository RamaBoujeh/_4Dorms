using _4Dorms.Models;
using _4Dorms.Repositories.implementation;
using _4Dorms.Repositories.Interfaces;
using _4Dorms.Resources;
using Microsoft.AspNetCore.Mvc;

namespace _4Dorms.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AdministratorController : ControllerBase
    {
        private readonly IAdministratorService _administratorService;
        private readonly IDormitoryService _dormitoryService;
        private readonly IUserService _userService;
        public AdministratorController(IAdministratorService administratorService, IDormitoryService dormitoryService, IUserService userService)
        {
            _administratorService = administratorService;
            _dormitoryService = dormitoryService;
            _userService = userService;
        }

        

        [HttpPut("review/{dormitoryId}")]
        public async Task<IActionResult> ReviewDormitory(int dormitoryId, bool approved, [FromBody] int administratorId)
        {
            try
            {
                await _administratorService.ReviewDormitoryAsync(dormitoryId, approved, administratorId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while reviewing the dormitory: {ex.Message}");
            }
        }

        [HttpPost("updateStatus")]
        public async Task<IActionResult> UpdateDormStatus([FromBody] DormStatusUpdateDTO dto)
        {
            try
            {
                Console.WriteLine($"Received request to update dorm status: DormId={dto.DormId}, Status={dto.Status}");
                var success = await _administratorService.UpdateDormStatusAsync(dto.DormId, (DormitoryStatus)dto.Status);
                if (success)
                {
                    return Ok("Dormitory status updated successfully.");
                }
                else
                {
                    Console.WriteLine($"Dormitory not found or update failed for DormId={dto.DormId}");
                    return NotFound("Dormitory not found or update failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpDelete("{UserId}")]
        public async Task<IActionResult> DeleteUserProfileByAdmin(int UserId, UserType userType)
        {
            var result = await _administratorService.DeleteUserProfileAsync(UserId, userType);
            if (result)
                return Ok();
            else
                return NotFound();
        }

        [HttpDelete("{ownerId}")]
        public async Task<IActionResult> DeleteOwner(int ownerId)
        {
            var result = await _userService.DeleteUserProfileAsync(ownerId, UserType.DormitoryOwner);

            if (result)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

    }
}
