using DAMProject.Shared.Models;
using Dapper;
using System.Data;

namespace DAMProject.Server.Repositories
{
    public interface IScoreRepository
    {
        Task<IEnumerable<Score>> GetScore();
        Task<Score> GetScoreById(int id);
        Task<int> CreateScore(Score score);
        Task<int> UpdateScore(Score score);
        Task DeleteScore(int id);
    }

    public class ScoreRepository(IDbConnection localDbConnection) : IScoreRepository
    {
        private readonly IDbConnection _localDbConnection = localDbConnection;

        public async Task<IEnumerable<Score>> GetScore()
        {
            var query = "SELECT id, user_id AS UserID, book_id AS BookId, user_rating AS UserRating, comment FROM score";
            return await _localDbConnection.QueryAsync<Score>(query);
        }

        public async Task<Score> GetScoreById(int id)
        {
            var query = "SELECT id, user_id AS UserID, book_id AS BookId, user_rating AS UserRating, comment FROM score WHERE id = @Id";
            return await _localDbConnection.QuerySingleOrDefaultAsync<Score>(query, new { Id = id });
        }

        public async Task<int> CreateScore(Score score)
        {
            var query = "INSERT INTO score (user_id, book_id, user_rating, comment) VALUES (@UserId, @BookId, @UserRating, @Comment)";
            var result = await _localDbConnection.ExecuteAsync(query, new { score.UserId, score.BookId, score.UserRating, score.Comment });

            return result > 0 ? result : throw new Exception("Error creating score.");
        }

        public async Task<int> UpdateScore(Score newScore)
        {
            var currentScore = await _localDbConnection.QuerySingleOrDefaultAsync<Score>(
                                "SELECT id, user_id AS UserID, book_id AS BookId, user_rating AS UserRating, comment FROM score WHERE id = @Id", new { newScore.Id });

            if (currentScore == null)
            {
                return 0;
            }

            var updatedScore = MapScore(newScore, currentScore);

            var query = "UPDATE score SET user_id = @UserId, book_id = @BookId, user_rating = @UserRating, comment = @Comment WHERE id = @Id";
            var result = await _localDbConnection.ExecuteAsync(query, new
            {
                updatedScore.UserId,
                updatedScore.BookId,
                updatedScore.UserRating,
                updatedScore.Comment,
                updatedScore.Id
            });

            return result > 0 ? result : throw new Exception("Error updating score.");
        }

        public async Task DeleteScore(int id)
        {
            var query = "DELETE FROM score WHERE id = @Id";
            await _localDbConnection.ExecuteAsync(query, new { Id = id });
        }

        private Score MapScore(Score newScore, Score currentScore)
        {
            return new Score
            {
                Id = currentScore.Id,
                UserId = newScore.UserId > 0 ? newScore.UserId : currentScore.UserId,
                BookId = newScore.BookId > 0 ? newScore.BookId : currentScore.BookId,
                UserRating = newScore.UserRating > 0 ? newScore.UserRating : currentScore.UserRating,
                Comment = !string.IsNullOrEmpty(newScore.Comment) ? newScore.Comment : currentScore.Comment
            };
        }
    }
}
