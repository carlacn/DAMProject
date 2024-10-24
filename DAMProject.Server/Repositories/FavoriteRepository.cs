using DAMProject.Shared.Models;
using Dapper;
using System.Data;

namespace DAMProject.Server.Repositories
{
    public interface IFavoritesRepository
    {
        Task<IEnumerable<Favorites>> GetFavorites();
        Task<Favorites> GetFavoriteById(int id);
        Task<int> CreateFavorite(Favorites favorite);
        Task<int> UpdateFavorite(Favorites favorite);
        Task DeleteFavorite(int id);
    }
        public class FavoritesRepository(IDbConnection localDbConnection) : IFavoritesRepository
    {
        private readonly IDbConnection _localDbConnection = localDbConnection;

        public async Task<IEnumerable<Favorites>> GetFavorites()
        {
            var query = @"SELECT id, user_id AS UserId, book_id AS BookId, format, status, read_date AS ReadDate 
                  FROM favorites";
            return await _localDbConnection.QueryAsync<Favorites>(query);
        }

        public async Task<Favorites> GetFavoriteById(int id)
        {
            var query = @"SELECT id, user_id, book_id, format, status, read_date 
                  FROM favorites WHERE id = @Id";
            return await _localDbConnection.QuerySingleOrDefaultAsync<Favorites>(query, new { Id = id });
        }

        public async Task<int> CreateFavorite(Favorites favorite)
        {
            var query = @"INSERT INTO favorites (user_id, book_id, format, status, read_date) 
                  VALUES (@UserId, @BookId, @Format, @Status, @ReadDate)";

            var format = favorite.Format.ToString();
            var status = favorite.Status.ToString();
            var result = await _localDbConnection.ExecuteAsync(query, new
            {
                favorite.UserId,
                favorite.BookId,
                format,
                status,
                favorite.ReadDate
            });

            return result > 0 ? result : throw new Exception("Error creating favorite.");
        }

        public async Task<int> UpdateFavorite(Favorites newFavorite)
        {
            var currentFavorite = await _localDbConnection.QuerySingleOrDefaultAsync<Favorites>(
                "SELECT * FROM favorites WHERE id = @Id", new { newFavorite.Id });

            if (currentFavorite == null)
            {
                return 0;
            }

            var updatedFavorite = MapFavorite(newFavorite, currentFavorite);

            var query = @"UPDATE favorites 
                  SET user_id = @UserId, book_id = @BookId, format = @Format, status = @Status, read_date = @ReadDate
                  WHERE id = @Id";
            var format = updatedFavorite.Format.ToString();
            var status = updatedFavorite.Status.ToString();
            var result = await _localDbConnection.ExecuteAsync(query, new
            {
                updatedFavorite.UserId,
                updatedFavorite.BookId,
                format,
                status,
                updatedFavorite.ReadDate,
                updatedFavorite.Id
            });

            return result > 0 ? result : throw new Exception("Error updating favorite.");
        }


        public async Task DeleteFavorite(int id)
        {
            var query = @"DELETE FROM favorites WHERE id = @Id";
            await _localDbConnection.ExecuteAsync(query, new { Id = id });
        }
        private Favorites MapFavorite(Favorites newFavorite, Favorites currentFavorite)
        {
            return new Favorites
            {
                Id = currentFavorite.Id,
                UserId = newFavorite.UserId > 0 ? newFavorite.UserId : currentFavorite.UserId,
                BookId = newFavorite.BookId > 0 ? newFavorite.BookId : currentFavorite.BookId,
                Format = !string.IsNullOrEmpty(newFavorite.Format.ToString()) ? newFavorite.Format : currentFavorite.Format,
                Status = !string.IsNullOrEmpty(newFavorite.Status.ToString()) ? newFavorite.Status : currentFavorite.Status,
                ReadDate = newFavorite.ReadDate != default ? newFavorite.ReadDate : currentFavorite.ReadDate
            };
        }

    }
}
