using DAMProject.Server.Repositories;
using DAMProject.Shared.Models;

namespace DAMProject.Server.Services
{
    public interface IAuthorService
    {
        Task<IEnumerable<Author>> GetAuthors();
        Task<Author> GetAuthorById(int id);
        Task CreateAuthor(Author author);
        Task UpdateAuthor(Author author);
        Task DeleteAuthor(int id);
    }

    public class AuthorService(IAuthorRepository authorRepository) : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository = authorRepository;

        public async Task<IEnumerable<Author>> GetAuthors() => await _authorRepository.GetAuthors();

        public async Task<Author> GetAuthorById(int id) => await _authorRepository.GetAuthorById(id);

        public async Task CreateAuthor(Author author) => await _authorRepository.CreateAuthor(author);

        public async Task UpdateAuthor(Author author) => await _authorRepository.UpdateAuthor(author);

        public async Task DeleteAuthor(int id) => await _authorRepository.DeleteAuthor(id);

    }
}
