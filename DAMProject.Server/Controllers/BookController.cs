using DAMProject.Server.Services;
using DAMProject.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DAMProject.Server.Controllers
{
    //[Authorize(Policy = "AdminPolicy")]
    [ApiController]
    [Route("api/[controller]")]
    public class BookController(IBookService bookService) : ControllerBase
    {
        private readonly IBookService _bookService = bookService;

        //[Authorize(Policy = "UserPolicy")]
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var response = await _bookService.GetBooks();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _bookService.GetBookById(id);
            return book == null ? NotFound() : Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook(Book book)
        {
            await _bookService.CreateBook(book);
            return Created();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, Book book)
        {
            if (id != book.Id) return BadRequest();
            await _bookService.UpdateBook(book);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            await _bookService.DeleteBook(id);
            return NoContent();
        }
    }
}
