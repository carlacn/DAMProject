using DAMProject.Shared.DTOs;
using Dapper;
using System.Data;

namespace DAMProject.Server.Queries
{
    public interface IBookDetailsQueries
    {
        Task<IEnumerable<BookDetailsDTO>> GetBookDetails();
    }

    public class BookDetailsQueries(IDbConnection localDbConnection) : IBookDetailsQueries
    {
        private readonly IDbConnection _localDbConnection = localDbConnection;

        public async Task<IEnumerable<BookDetailsDTO>> GetBookDetails()
        {
            var query = @"
                SELECT b.id, title, comments, image, g.name AS Genre, 
                p.name AS Publisher, s.name AS Series FROM books b JOIN genres 
                g ON b.genre_id = g.id JOIN publishers p ON 
                b.publisher_id = p.id JOIN series s ON b.series_id = s.id
            ";
            return await _localDbConnection.QueryAsync<BookDetailsDTO>(query);

        }
    }
}
