using _4Dorms.Repositories.Interfaces;
using _4Dorms.Resources;
using Microsoft.AspNetCore.Mvc;

namespace _4Dorms.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class FavoriteListController : ControllerBase
    {
        private readonly IFavoriteListService _favoriteListService;

        public FavoriteListController(IFavoriteListService favoriteListService)
        {
            _favoriteListService = favoriteListService;
        }

        [HttpPost("add-to-favorites")]
        public async Task<IActionResult> AddToFavorites([FromBody] FavoriteListDTO favoriteData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _favoriteListService.AddDormitoryToFavoritesAsync(favoriteData.FavoriteListId, favoriteData.DormitoryId);

            if (result)
            {
                return Ok("Dormitory added to favorites successfully.");
            }
            else
            {
                return StatusCode(500, "Failed to add dormitory to favorites.");
            }
        }

        [HttpDelete("{favoriteListId}/remove")]
        public async Task<IActionResult> RemoveDormitoryFromFavorites(int favoriteListId, [FromBody] int dormitoryId)
        {
            var result = await _favoriteListService.RemoveDormitoryFromFavoritesAsync(favoriteListId, dormitoryId);

            if (result)
            {
                return Ok(new { Message = "Dormitory removed from favorites successfully." });
            }

            return BadRequest(new { Message = "Failed to remove dormitory from favorites." });
        }
    }
}
