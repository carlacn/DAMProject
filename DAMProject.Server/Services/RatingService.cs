using DAMProject.Server.Repositories;
using DAMProject.Shared.Models;

namespace DAMProject.Server.Services
{

    public interface IRatingService
    {
        Task<IEnumerable<Rating>> GetRatings();
        Task<Rating> GetRatingById(int id);
        Task CreateRating(Rating rating);
        Task UpdateRating(Rating rating);
        Task DeleteRating(int id);
    }

    public class RatingService(IRatingRepository ratingRepository) : IRatingService
    {
        private readonly IRatingRepository _ratingRepository = ratingRepository;


        public async Task<IEnumerable<Rating>> GetRatings() => await _ratingRepository.GetRatings();
        public async Task<Rating> GetRatingById(int id) => await _ratingRepository.GetRatingById(id);
        public async Task CreateRating(Rating rating) => await _ratingRepository.CreateRating(rating);
        public async Task UpdateRating(Rating rating) => await _ratingRepository.UpdateRating(rating);
        public async Task DeleteRating(int id) => await _ratingRepository.DeleteRating(id);
    }
}

