using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using _4Dorms.GenericRepo;
using _4Dorms.Models;
using _4Dorms.Repositories.implementation;
using _4Dorms.Repositories.Interfaces;
using _4Dorms.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace _4Dorms.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class DormitoryOwnerController : ControllerBase
    {
        private readonly IDormitoryOwnerService _dormitoryOwnerService;
        private readonly IUserService _userService;
        private readonly ILogger<DormitoryOwnerController> _logger;
        private readonly FileService _fileService;

        public DormitoryOwnerController(
            IDormitoryOwnerService dormitoryOwnerService,
            ILogger<DormitoryOwnerController> logger,
            IUserService userService,
            FileService fileService)
        {
            _dormitoryOwnerService = dormitoryOwnerService;
            _userService = userService;
            _logger = logger;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<ActionResult<List<DormitoryOwner>>> GetAllOwners()
        {
            try
            {
                var owners = await _dormitoryOwnerService.GetAllOwnersAsync();
                return Ok(owners);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error fetching dormitory owners");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{ownerId}")]
        public async Task<ActionResult<DormitoryOwnerDTO>> GetOwner(int ownerId)
        {
            try
            {
                var owner = await _dormitoryOwnerService.GetOwnerByIdAsync(ownerId);
                if (owner == null)
                {
                    return NotFound();
                }
                return Ok(owner);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error fetching owner");
                return StatusCode(500, "Internal server error");
            }
        }


        [Authorize(Roles = "DormitoryOwner")]
        [HttpPost("submit-dormitory")]
        public async Task<IActionResult> SubmitDormitoryForApproval([FromForm] DormitorySubmitDTO dormitoryDTO, [FromForm] List<IFormFile> images)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirst("DormitoryOwnerId");
            if (userIdClaim == null)
            {
                _logger.LogError("DormitoryOwnerId claim not found in token");
                return Unauthorized("DormitoryOwnerId claim not found");
            }

            var userId = int.Parse(userIdClaim.Value);

            _logger.LogInformation("Submitting dormitory for approval. UserId: {UserId}, Dormitory: {Dormitory}", userId, dormitoryDTO);

            await _dormitoryOwnerService.SubmitDormitoryForApprovalAsync(dormitoryDTO, userId, images);

            return Ok("Dormitory submitted for approval successfully");
        }

        [HttpPost("upload")]
        public IActionResult UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Read the file bytes
            byte[] fileBytes;
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            // Save the file
            var fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var result = _fileService.SaveFile(fileBytes, fileName);

            if (result)
            {
                return Ok("File uploaded successfully.");
            }
            else
            {
                return StatusCode(500, "Error saving file.");
            }
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
                _logger.LogError(ex, "An error occurred while deleting the dormitory.");
                return StatusCode(500, $"An error occurred while deleting the dormitory: {ex.Message}");
            }
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