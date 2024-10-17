using DAMProject.Server.Repositories;
using DAMProject.Shared.Models;

namespace DAMProject.Server.Services
{

    public interface IRatingService
    {
        Task<IEnumerable<Score>> GetRatings();
        Task<Score> GetRatingById(int id);
        Task CreateRating(Score rating);
        Task UpdateRating(Score rating);
        Task DeleteRating(int id);
    }

    public class RatingService(IRatingRepository ratingRepository) : IRatingService
    {
        private readonly IRatingRepository _ratingRepository = ratingRepository;


        public async Task<IEnumerable<Score>> GetRatings() => await _ratingRepository.GetRatings();
        public async Task<Score> GetRatingById(int id) => await _ratingRepository.GetRatingById(id);
        public async Task CreateRating(Score rating) => await _ratingRepository.CreateRating(rating);
        public async Task UpdateRating(Score rating) => await _ratingRepository.UpdateRating(rating);
        public async Task DeleteRating(int id) => await _ratingRepository.DeleteRating(id);
    }
}

