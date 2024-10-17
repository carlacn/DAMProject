using DAMProject.Shared.Models;
using Dapper;
using System.Data;

namespace DAMProject.Server.Repositories
{
    public interface IRatingRepository
    {
        Task<IEnumerable<Rating>> GetRatings();
        Task<Rating> GetRatingById(int id);
        Task<int> CreateRating(Rating rating);
        Task<int> UpdateRating(Rating rating);
        Task DeleteRating(int id);
    }

    public class RatingRepository(IDbConnection localDbConnection) : IRatingRepository
    {
        private readonly IDbConnection _localDbConnection = localDbConnection;


        public async Task<IEnumerable<Rating>> GetRatings()
        {
            var query = "SELECT * FROM ratings";
            return await _localDbConnection.QueryAsync<Rating>(query);
        }

        public async Task<Rating> GetRatingById(int id)
        {
            var query = "SELECT * FROM ratings WHERE id = @Id";
            return await _localDbConnection.QuerySingleOrDefaultAsync<Rating>(query, new { Id = id });
        }

        public async Task<int> CreateRating(Rating rating)
        {
            var query = "INSERT INTO ratings (user_id, book_id, user_rating, comment) VALUES (@UserId, @BookId, @UserRating, @Comment)";
            var result = await _localDbConnection.ExecuteAsync(query, new { rating.UserId, rating.BookId, rating.UserRating, rating.Comment });

            return result > 0 ? result : throw new Exception("Error creating rating.");
        }

        public async Task<int> UpdateRating(Rating newRating)
        {
            var currentRating = await _localDbConnection.QuerySingleOrDefaultAsync<Rating>(
                                "SELECT * FROM ratings WHERE id = @Id", new { newRating.Id });

            if (currentRating == null)
            {
                return 0;
            }

            var updatedRating = MapRating(newRating, currentRating);

            var query = "UPDATE ratings SET user_id = @UserId, book_id = @BookId, user_rating = @UserRating, comment = @Comment WHERE id = @Id";
            var result = await _localDbConnection.ExecuteAsync(query, new
            {
                updatedRating.UserId,
                updatedRating.BookId,
                updatedRating.UserRating,
                updatedRating.Comment,
                updatedRating.Id
            });

            return result > 0 ? result : throw new Exception("Error updating rating.");
        }

        public async Task DeleteRating(int id)
        {
            var query = "DELETE FROM ratings WHERE id = @Id";
            await _localDbConnection.ExecuteAsync(query, new { Id = id });
        }

        private Rating MapRating(Rating newRating, Rating currentRating)
        {
            return new Rating
            {
                Id = currentRating.Id,
                UserId = newRating.UserId > 0 ? newRating.UserId : currentRating.UserId,
                BookId = newRating.BookId > 0 ? newRating.BookId : currentRating.BookId,
                UserRating = newRating.UserRating > 0 ? newRating.UserRating : currentRating.UserRating,
                Comment = !string.IsNullOrEmpty(newRating.Comment) ? newRating.Comment : currentRating.Comment
            };
        }
    }
}
