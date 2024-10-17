using DAMProject.Server.Services;
using DAMProject.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DAMProject.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpGet]
        //[Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetRatings()
        {
            var response = await _ratingService.GetRatings();
            return Ok(response);
        }

        [HttpGet("{id}")]
        //[Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetRatingById(int id)
        {
            var rating = await _ratingService.GetRatingById(id);
            return rating == null ? NotFound() : Ok(rating);
        }

        [HttpPost]
        //[Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> CreateRating([FromBody] Score rating)
        {
            await _ratingService.CreateRating(rating);
            return CreatedAtAction(nameof(GetRatingById), new { id = rating.Id }, rating);
        }

        [HttpPut("{id}")]
        //[Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> UpdateRating(int id, [FromBody] Score rating)
        {
            if (id != rating.Id) return BadRequest();
            await _ratingService.UpdateRating(rating);
            return NoContent();
        }

        [HttpDelete("{id}")]
        //[Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> DeleteRating(int id)
        {
            await _ratingService.DeleteRating(id);
            return NoContent();
        }
    }
}
