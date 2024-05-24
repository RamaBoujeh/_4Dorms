using _4Dorms.Repositories.Interfaces;
using _4Dorms.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace _4Dorms.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class FavoriteListController : ControllerBase
    {
        private readonly IFavoriteListService _favoriteListService;
        private readonly ILogger<FavoriteListController> _logger;

        public FavoriteListController(IFavoriteListService favoriteListService, ILogger<FavoriteListController> logger)
        {
            _favoriteListService = favoriteListService;
            _logger = logger;
        }

        [HttpPost("add-to-favorites")]
        public async Task<IActionResult> AddToFavorites([FromBody] FavoriteListDTO favoriteData)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state: {ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Adding dormitory to favorites. FavoriteListId: {FavoriteListId}, DormitoryId: {DormitoryId}", favoriteData.FavoriteListId, favoriteData.DormitoryId);

                var result = await _favoriteListService.AddDormitoryToFavoritesAsync(favoriteData.FavoriteListId, favoriteData.DormitoryId);

                if (result)
                {
                    _logger.LogInformation("Dormitory added to favorites successfully.");
                    return Ok("Dormitory added to favorites successfully.");
                }
                else
                {
                    _logger.LogError("Failed to add dormitory to favorites.");
                    return StatusCode(500, "Failed to add dormitory to favorites.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while adding dormitory to favorites.");
                return StatusCode(500, "An error occurred while adding dormitory to favorites.");
            }
        }

        [HttpDelete("{favoriteListId}/remove")]
        public async Task<IActionResult> RemoveDormitoryFromFavorites(int favoriteListId, [FromBody] int dormitoryId)
        {
            try
            {
                _logger.LogInformation("Removing dormitory from favorites. FavoriteListId: {FavoriteListId}, DormitoryId: {DormitoryId}", favoriteListId, dormitoryId);

                var result = await _favoriteListService.RemoveDormitoryFromFavoritesAsync(favoriteListId, dormitoryId);

                if (result)
                {
                    _logger.LogInformation("Dormitory removed from favorites successfully.");
                    return Ok(new { Message = "Dormitory removed from favorites successfully." });
                }

                _logger.LogWarning("Failed to remove dormitory from favorites.");
                return BadRequest(new { Message = "Failed to remove dormitory from favorites." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while removing dormitory from favorites.");
                return StatusCode(500, "An error occurred while removing dormitory from favorites.");
            }
        }
    }
}
