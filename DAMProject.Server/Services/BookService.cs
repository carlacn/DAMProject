using DAMProject.Server.Repositories;
using DAMProject.Shared.Models;

namespace DAMProject.Server.Services
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetBooks();
        Task<Book> GetBookById(int id);
        Task CreateBook(Book book);
        Task UpdateBook(Book book);
        Task DeleteBook(int id);
    }

    public class BookService(IBookRepository bookRepository) : IBookService
    {
        private readonly IBookRepository _bookRepository = bookRepository;

        public async Task<IEnumerable<Book>> GetBooks() => await _bookRepository.GetBooks();
        
        public async Task<Book> GetBookById(int id) => await _bookRepository.GetBookById(id);
        
        public async Task CreateBook(Book book) => await _bookRepository.CreateBook(book);
        
        public async Task UpdateBook(Book book) => await _bookRepository.UpdateBook(book);
        
        public async Task DeleteBook(int id) => await _bookRepository.DeleteBook(id);
    }
}