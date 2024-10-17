using DAMProject.Shared.Models;
using Dapper;
using System.Data;

namespace DAMProject.Server.Repositories
{
    public interface IGenreRepository
    {
        Task<IEnumerable<Genre>> GetGenres();
        Task<Genre> GetGenreById(int id);
        Task<int> CreateGenre(Genre genre);
        Task<int> UpdateGenre(Genre genre);
        Task DeleteGenre(int id);
    }

    public class GenreRepository(IDbConnection localDbConnection) : IGenreRepository
    {
        private readonly IDbConnection _localDbConnection = localDbConnection;

        public async Task<IEnumerable<Genre>> GetGenres()
        {
            var query = @"SELECT id, name FROM genres";
            return await _localDbConnection.QueryAsync<Genre>(query);
        }
        public async Task<Genre> GetGenreById(int id)
        {
            var query = @"SELECT id, name FROM genres WHERE id = @Id";
            return await _localDbConnection.QuerySingleOrDefaultAsync<Genre>(query, new { Id = id });
        }

        public async Task<int> CreateGenre(Genre genre)
        {
            var query = @"INSERT INTO genres (name) VALUES (@Name)";
            var result = await _localDbConnection.ExecuteAsync(query, new { genre.Name });

            return result > 0 ? result : throw new Exception("Error creating new genre.");
        }

        public async Task<int> UpdateGenre(Genre newGenre)
        {
            var currentGenre = await _localDbConnection.QuerySingleOrDefaultAsync<Genre>(
                    "SELECT * FROM genres WHERE id = @Id", new { newGenre.Id });

            if (currentGenre == null)
            {
                return 0;
            }

            var updatedGenre = MapGenre(newGenre, currentGenre);

            var query = "UPDATE genres SET name = @Name WHERE id = @Id";
            var result = await _localDbConnection.ExecuteAsync(query, new { updatedGenre.Name, updatedGenre.Id });

            return result > 0 ? result : throw new Exception("Error updating genre.");
        }

        public async Task DeleteGenre(int id)
        {
            var query = @"DELETE FROM genres WHERE id = @Id";
            await _localDbConnection.ExecuteAsync(query, new { Id = id });
        }
        private Genre MapGenre(Genre newGenre, Genre currentGenre)
        {
            return new Genre
            {
                Id = currentGenre.Id,
                Name = !string.IsNullOrEmpty(newGenre.Name) ? newGenre.Name : currentGenre.Name
            };
        }
    }
}
