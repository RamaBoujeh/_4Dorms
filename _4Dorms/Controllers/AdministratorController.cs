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
        public AdministratorController(IAdministratorService administratorService)
        {
            _administratorService = administratorService;
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

        [HttpDelete("{UserId}")]
        public async Task<IActionResult> DeleteUserProfileByAdmin(int UserId, UserType userType)
        {
            var result = await _administratorService.DeleteUserProfileAsync(UserId, userType);
            if (result)
                return Ok();
            else
                return NotFound();
        }
    }
}
