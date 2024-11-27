using DAMProject.Server.Services;
using DAMProject.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace DAMProject.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScoreController : ControllerBase
    {
        private readonly IScoreService _scoreService;

        public ScoreController(IScoreService scoreService)
        {
            _scoreService = scoreService;
        }

        [HttpGet]
        public async Task<IActionResult> GetScores()
        {
            var response = await _scoreService.GetScores();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetScoreById(int id)
        {
            var score = await _scoreService.GetScoreById(id);
            return score == null ? NotFound() : Ok(score);
        }

        [HttpPost]
        public async Task<IActionResult> CreateScore([FromBody] Score score)
        {
            await _scoreService.CreateScore(score);
            return CreatedAtAction(nameof(GetScoreById), new { id = score.Id }, score);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateScore(int id, [FromBody] Score score)
        {
            if (id != score.Id) return BadRequest();
            await _scoreService.UpdateScore(score);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScore(int id)
        {
            await _scoreService.DeleteScore(id);
            return NoContent();
        }
    }
}
