using DAMProject.Server.Services;
using DAMProject.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DAMProject.Server.Controllers
{
    //[Authorize(Policy = "AdminPolicy")]
    [ApiController]
    [Route("api/[controller]")]
    public class SerieController(ISeriesService seriesService) : ControllerBase
    {
        private readonly ISeriesService _seriesService = seriesService;

        [HttpGet]
        public async Task<IActionResult> GetSeries()
        {
            var response = await _seriesService.GetSeries();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSeriesById(int id)
        {
            var series = await _seriesService.GetSeriesById(id);
            return series == null ? NotFound() : Ok(series);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSeries([FromBody] Serie series)
        {
            await _seriesService.CreateSeries(series);
            return Created();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeries(int id, [FromBody] Serie series)
        {
            if (id != series.Id) return BadRequest();
            await _seriesService.UpdateSeries(series);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeries(int id)
        {
            await _seriesService.DeleteSeries(id);
            return NoContent();
        }
    }
}
