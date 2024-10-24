using DAMProject.Shared.Models;
using Dapper;
using System.Data;

namespace DAMProject.Server.Repositories
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAuthors();
        Task<Author> GetAuthorById(int id);
        Task<int> CreateAuthor(Author author);
        Task<int> UpdateAuthor(Author author);
        Task<int> DeleteAuthor(int id);
    }

    public class AuthorRepository(IDbConnection localDbConnection) : IAuthorRepository
    {
        private readonly IDbConnection _localDbConnection = localDbConnection;

        public async Task<IEnumerable<Author>> GetAuthors()
        {
            var query = @"SELECT id, name, biography, image FROM authors";
            return await _localDbConnection.QueryAsync<Author>(query);
        }

        public async Task<Author> GetAuthorById(int id)
        {
            var query = @"SELECT id, name, biography, image FROM authors WHERE id = @Id";
            return await _localDbConnection.QuerySingleOrDefaultAsync<Author>(query, new { Id = id });
        }

        public async Task<int> CreateAuthor(Author author)
        {
            var query = @"INSERT INTO authors (name, biography, image) 
                          VALUES (@Name, @Biography, @Image)";
            var result = await _localDbConnection.ExecuteAsync(query, new { author.Name, author.Biography, author.Image });

            return result;
        }

        public async Task<int> UpdateAuthor(Author newAuthor)
        {
            var currentAuthor = await _localDbConnection.QuerySingleOrDefaultAsync<Author>(
                "SELECT * FROM authors WHERE id = @Id", new { newAuthor.Id });

            if (currentAuthor == null)
            {
                return 0;
            }

            var updatedAuthor = MapAuthor(newAuthor, currentAuthor);

            var query = @"UPDATE authors 
                  SET name = @Name, biography = @Biography, image = @Image
                  WHERE id = @Id";

            var result = await _localDbConnection.ExecuteAsync(query, new
            {
                updatedAuthor.Name,
                updatedAuthor.Biography,
                updatedAuthor.Image,
                updatedAuthor.Id
            });

            return result;
        }

        public async Task<int> DeleteAuthor(int id)
        {
            var query = @"DELETE FROM authors WHERE id = @Id";
            return await _localDbConnection.ExecuteAsync(query, new { Id = id });
        }

        private Author MapAuthor(Author newAuthor, Author currentAuthor)
        {
            return new Author
            {
                Id = currentAuthor.Id,
                Name = !string.IsNullOrEmpty(newAuthor.Name) ? newAuthor.Name : currentAuthor.Name,
                Biography = !string.IsNullOrEmpty(newAuthor.Biography) ? newAuthor.Biography : currentAuthor.Biography,
                Image = !string.IsNullOrEmpty(newAuthor.Image) ? newAuthor.Image : currentAuthor.Image
            };
        }
    }
}
