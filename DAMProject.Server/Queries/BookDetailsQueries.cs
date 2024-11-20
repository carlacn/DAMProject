using DAMProject.Shared.DTOs;
using DAMProject.Shared.Models;
using Dapper;
using System.Data;

namespace DAMProject.Server.Queries
{
    public interface IBookDetailsQueries
    {
        Task<IEnumerable<BookDetailsDTO>> GetBookDetails();
        Task<IEnumerable<FavoriteBookDTO>> GetUserFavorites(int userId);
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

        public async Task<IEnumerable<FavoriteBookDTO>> GetUserFavorites(int userId)
        {
            var query = @"SELECT f.book_id as BookId, title, g.name as Genre, p.name as Publisher, s.name as Series, image, f.id as FavoriteId, format, status, read_date as ReadDate
                        from books b 
                        join favorites f on b.id = f.book_id
                        join genres g on b.genre_id = g.id
                        join publishers p on b.publisher_id = p.id
                        join series s on b.series_id = s.id
                        WHERE f.user_id = @userId";

            return await _localDbConnection.QueryAsync<FavoriteBookDTO>(query, new {userId});

        }

    }
}
