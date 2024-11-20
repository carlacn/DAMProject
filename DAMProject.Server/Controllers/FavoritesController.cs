using DAMProject.Server.Auth;
using DAMProject.Server.Services;
using DAMProject.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace DAMProject.Server.Controllers
{
    //[Authorize(Policy = "UserPolicy")]
    [ApiController]
    [Route("api/[controller]")]
    public class FavoritesController(IFavoritesService favoritesService, IAuthService authService) : ControllerBase
    {
        private readonly IFavoritesService _favoritesService = favoritesService;
        private readonly IAuthService _authService = authService;

        [HttpGet]
        public async Task<IActionResult> GetFavorites()
        {
            var response = await _favoritesService.GetFavorites();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFavoriteById(int id)
        {
            var favorite = await _favoritesService.GetFavoriteById(id);
            return favorite == null ? NotFound() : Ok(favorite);
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUserFavorite()
        {
            var userId = _authService.GetUserId() ?? 0;
            if (userId == 0)
            {
                NotFound();
            }
            var favorites = await _favoritesService.GetUserFavorites(userId);
            return favorites == null ? NotFound() : Ok(favorites);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFavorite([FromBody] Favorites favorite)
        {
            if (favorite.UserId == 0)
            {
                favorite.UserId = _authService.GetUserId() ?? 0;
            }
            await _favoritesService.CreateFavorite(favorite);
            return Created();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFavorite(int id, [FromBody] Favorites favorite)
        {
            if (id != favorite.Id) return BadRequest();
            if (favorite.UserId == 0)
            {
                favorite.UserId = _authService.GetUserId() ?? 0;
            }
            await _favoritesService.UpdateFavorite(favorite);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFavorite(int id)
        {
            await _favoritesService.DeleteFavorite(id);
            return NoContent();
        }

        [HttpDelete("user")]
        public async Task<IActionResult> DeleteUserFavorite()
        {
            var userId = _authService.GetUserId() ?? 0;
            if (userId == 0)
            {
                NotFound();
            }
            await _favoritesService.DeleteFavorite(userId);
            return NoContent();
        }
    }
}
