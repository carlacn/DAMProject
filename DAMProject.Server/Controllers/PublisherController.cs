using DAMProject.Server.Services;
using DAMProject.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DAMProject.Server.Controllers
{
    //[Authorize(Policy = "AdminPolicy")]
    [ApiController]
    [Route("api/[controller]")]
    public class PublisherController(IPublisherService publisherService) : ControllerBase
    {
        private readonly IPublisherService _publisherService = publisherService;

        [HttpGet]
        public async Task<IActionResult> GetPublishers()
        {
            var response = await _publisherService.GetPublishers();
            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPublisherById(int id)
        {
            var publisher = await _publisherService.GetPublisherById(id);
            return publisher == null ? NotFound() : Ok(publisher);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePublisher([FromBody] Publisher publisher)
        {
            await _publisherService.CreatePublisher(publisher);
            return Created();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePublisher(int id, [FromBody] Publisher publisher)
        {
            if (id != publisher.Id) return BadRequest();
            await _publisherService.UpdatePublisher(publisher);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            await _publisherService.DeletePublisher(id);
            return NoContent();
        }
    }
}
