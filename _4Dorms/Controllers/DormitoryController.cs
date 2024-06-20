using _4Dorms.Models;
using _4Dorms.Repositories.Interfaces;
using _4Dorms.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace _4Dorms.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class DormitoryController : ControllerBase
    {
        private readonly IDormitoryService _dormitoryService;
        private readonly IFileService _fileService;
        private readonly ILogger<DormitoryController> _logger;

        public DormitoryController(IDormitoryService dormitoryService, IFileService fileService , ILogger<DormitoryController> logger)
        {
            _dormitoryService = dormitoryService;
            _logger = logger;
            _fileService = fileService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllDormitories()
        {
            var dormitories = await _dormitoryService.GetAllDormitoriesAsync();
            return Ok(dormitories);
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadDormitoryImage(IFormFile imageFile, int dormitoryId)
        {
            if (imageFile == null || imageFile.Length == 0)
                return BadRequest("Image file is required.");

            var uploadPath = Path.Combine("wwwroot", "uploads", "dormitoryImages");
            Directory.CreateDirectory(uploadPath);
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            var imageUrl = $"/uploads/dormitoryImages/{fileName}";

            var dormitoryImage = new DormitoryImage
            {
                DormitoryId = dormitoryId,
                Url = imageUrl
            };

            await _dormitoryService.AddDormitoryImageAsync(dormitoryImage);

            return Ok(new { imageUrl });
        }

        [HttpGet("dormitory/{id}")]
        public async Task<IActionResult> GetDormitoryById(int id)
        {
            var dormitory = await _dormitoryService.GetDormitoryByIdAsync(id);
            if (dormitory == null)
            {
                _logger.LogWarning("Dormitory not found with id {DormitoryId}", id);
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
                _logger.LogError(ex, "Error fetching pending dormitories");
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
            catch (System.Exception ex)
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
                _logger.LogWarning("Dormitory details not found with id {DormitoryId}", dormitoryId);
                return NotFound();
            }
            return Ok(dormitory);
        }

        [HttpGet("owner/{ownerId}")]
        public async Task<ActionResult<List<Dormitory>>> GetDormsByOwnerId(int ownerId)
        {
            try
            {
                var dorms = await _dormitoryService.GetDormsByOwnerIdAsync(ownerId);
                if (dorms == null)
                {
                    return NotFound("No dormitories found for this owner.");
                }
                return Ok(dorms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching dormitories by owner ID");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("search")]
        public async Task<ActionResult<List<Dormitory>>> SearchDormitoriesAsync(
           [FromQuery] string keywords = null,
           [FromQuery] string city = null,
           [FromQuery] string nearbyUniversity = null,
           [FromQuery] string genderType = null)
        {
            try
            {
                var dormitories = await _dormitoryService.SearchDormitoriesAsync(keywords, city, nearbyUniversity, genderType);
                return Ok(dormitories);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
