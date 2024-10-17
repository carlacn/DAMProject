using DAMProject.Server.Services;
using DAMProject.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DAMProject.Server.Controllers
{
    //[Authorize(Policy = "UserPolicy")]
    [ApiController]
    [Route("api/[controller]")]
    public class FavoritesController(IFavoritesService favoritesService) : ControllerBase
    {
        private readonly IFavoritesService _favoritesService = favoritesService;

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

        [HttpPost]
        public async Task<IActionResult> CreateFavorite(Favorites favorite)
        {
            await _favoritesService.CreateFavorite(favorite);
            return Created();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFavorite(int id, Favorites favorite)
        {
            if (id != favorite.Id) return BadRequest();
            await _favoritesService.UpdateFavorite(favorite);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFavorite(int id)
        {
            await _favoritesService.DeleteFavorite(id);
            return NoContent();
        }
    }
}
