using _4Dorms.Repositories.Interfaces;
using _4Dorms.Resources;
using Microsoft.AspNetCore.Mvc;

namespace _4Dorms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly ILogger<ReviewController> _logger;

        public ReviewController(IReviewService reviewService, ILogger<ReviewController> logger)
        {
            _reviewService = reviewService;
            _logger = logger;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddReview([FromBody] ReviewDTO reviewDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid review model state.");
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _reviewService.AddReviewAsync(reviewDTO.DormitoryId, reviewDTO.StudentId, reviewDTO.Rating, reviewDTO.Comment);
                if (result)
                {
                    _logger.LogInformation("Review added successfully.");
                    return Ok("Review added successfully.");
                }
                else
                {
                    _logger.LogError("Failed to add review.");
                    return StatusCode(500, "Failed to add review. Please try again later.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception while adding review: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
