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
        public async Task<IActionResult> AddReview(ReviewDTO reviewDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _reviewService.AddReviewAsync(reviewDTO.DormitoryId, reviewDTO.StudentId, reviewDTO.Rating, reviewDTO.Comment);
            if (result)
            {
                return Ok("Review added successfully.");
            }
            else
            {
                return StatusCode(500, "Failed to add review. Please try again later.");
            }
        }
    }
}
