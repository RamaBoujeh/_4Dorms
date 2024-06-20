using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using _4Dorms.Repositories.Interfaces;
using System.Security.Claims;
using _4Dorms.Resources;

namespace _4Dorms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        public async Task<IActionResult> AddToFavorites([FromBody] FavoriteRequest model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid");
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirst("StudentId") ?? User.FindFirst("DormitoryOwnerId");
            if (userIdClaim == null)
            {
                _logger.LogWarning("User ID claim not found in token");
                return Unauthorized("User ID claim not found in token");
            }
            var userId = int.Parse(userIdClaim.Value);
            _logger.LogInformation("User ID from token: {UserId}", userId);

            var userTypeClaim = User.FindFirst(ClaimTypes.Role);
            if (userTypeClaim == null)
            {
                _logger.LogWarning("User role claim not found in token");
                return Unauthorized("User role claim not found in token");
            }
            var userType = userTypeClaim.Value;
            _logger.LogInformation("User type from token: {UserType}", userType);

            var favoriteListId = await _favoriteListService.GetFavoriteDormsAsync(userId, userType);

            if (favoriteListId == null)
            {
                _logger.LogError("Failed to retrieve favorite list ID for user ID {UserId} and type {UserType}", userId, userType);
                return StatusCode(500, "Failed to retrieve favorite list ID");
            }

            _logger.LogInformation("Favorite list ID: {FavoriteListId}", favoriteListId);

            var result = await _favoriteListService.AddDormitoryToFavoritesAsync(favoriteListId.Value, model.DormitoryId);

            if (result)
            {
                _logger.LogInformation("Dormitory ID {DormitoryId} added to favorite list ID {FavoriteListId}", model.DormitoryId, favoriteListId.Value);
                return Ok("Dormitory added to favorites successfully.");
            }
            else
            {
                _logger.LogError("Failed to add dormitory ID {DormitoryId} to favorite list ID {FavoriteListId}", model.DormitoryId, favoriteListId.Value);
                return StatusCode(500, "Failed to add dormitory to favorites.");
            }
        }

        [HttpPost("remove-from-favorites")]
        public async Task<IActionResult> RemoveFromFavorites([FromBody] FavoriteRequest model)
        {
            var userIdClaim = User.FindFirst("StudentId") ?? User.FindFirst("DormitoryOwnerId");
            if (userIdClaim == null)
            {
                _logger.LogWarning("User ID claim not found in token");
                return Unauthorized("User ID claim not found in token");
            }
            var userId = int.Parse(userIdClaim.Value);
            _logger.LogInformation("User ID from token: {UserId}", userId);

            var userTypeClaim = User.FindFirst(ClaimTypes.Role);
            if (userTypeClaim == null)
            {
                _logger.LogWarning("User role claim not found in token");
                return Unauthorized("User role claim not found in token");
            }
            var userType = userTypeClaim.Value;
            _logger.LogInformation("User type from token: {UserType}", userType);

            int? favoriteListId = await _favoriteListService.GetFavoriteDormsAsync(userId, userType);
            if (!favoriteListId.HasValue)
            {
                _logger.LogWarning("Favorite list not found for user ID {UserId} and type {UserType}", userId, userType);
                return NotFound();
            }

            bool result = await _favoriteListService.RemoveDormitoryFromFavoritesAsync(favoriteListId.Value, model.DormitoryId);
            if (!result)
            {
                _logger.LogError("Failed to remove dormitory with ID {DormitoryId} from favorite list with ID {FavoriteListId}", model.DormitoryId, favoriteListId.Value);
                return BadRequest("Failed to remove dormitory from favorites.");
            }

            return Ok("Dormitory removed from favorites.");
        }

        [HttpGet("user-favorites")]
        public async Task<IActionResult> GetUserFavorites()
        {
            var userClaims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            _logger.LogInformation("User claims: {@UserClaims}", userClaims);

            var roleClaim = User.FindFirst(ClaimTypes.Role);
            if (roleClaim == null)
            {
                _logger.LogWarning("Role claim not found in token");
                return Unauthorized("Role claim not found in token");
            }
            var role = roleClaim.Value;
            _logger.LogInformation("User role from token: {UserRole}", role);

            Claim userIdClaim = null;
            switch (role)
            {
                case "Student":
                    userIdClaim = User.FindFirst("StudentId");
                    break;
                case "DormitoryOwner":
                    userIdClaim = User.FindFirst("DormitoryOwnerId");
                    break;
                case "Administrator":
                    userIdClaim = User.FindFirst("AdministratorId");
                    break;
            }

            if (userIdClaim == null)
            {
                _logger.LogWarning("User ID claim not found in token for role {Role}", role);
                return Unauthorized("User ID claim not found in token");
            }

            var userId = int.Parse(userIdClaim.Value);
            _logger.LogInformation("User ID from token: {UserId}", userId);

            var favoriteListId = await _favoriteListService.GetFavoriteDormsAsync(userId, role);
            if (favoriteListId == null)
            {
                return StatusCode(500, "Failed to retrieve favorite list ID");
            }

            var favoriteList = await _favoriteListService.GetFavoriteListByIdAsync(favoriteListId.Value);
            if (favoriteList == null || !favoriteList.Dormitories.Any())
            {
                return NotFound("No favorite dormitories found");
            }

            var favoriteDormitories = favoriteList.Dormitories.Select(d => new
            {
                d.DormitoryId,
                d.DormitoryName,
                d.GenderType,
                d.City,
                d.NearbyUniversity,
                d.phone,
                d.Email,
                d.DormitoryDescription,
                d.Location,
                d.PriceHalfYear,
                d.PriceFullYear,
                d.Status,
                d.DormitoryOwnerId,
                d.AdministratorId,
                d.ImageUrls
            }).ToList();

            return Ok(favoriteDormitories);
        }


    }
}
