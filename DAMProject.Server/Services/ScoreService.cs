using DAMProject.Server.Repositories;
using DAMProject.Shared.Models;

namespace DAMProject.Server.Services
{

    public interface IScoreService
    {
        Task<IEnumerable<Score>> GetScores();
        Task<Score> GetScoreById(int id);
        Task CreateScore(Score score);
        Task UpdateScore(Score score);
        Task DeleteScore(int id);
    }

    public class ScoreService(IScoreRepository scoreRepository) : IScoreService
    {
        private readonly IScoreRepository _scoreRepository = scoreRepository;

        public async Task<IEnumerable<Score>> GetScores() => await _scoreRepository.GetScore();
        public async Task<Score> GetScoreById(int id) => await _scoreRepository.GetScoreById(id);
        public async Task CreateScore(Score score) => await _scoreRepository.CreateScore(score);
        public async Task UpdateScore(Score score) => await _scoreRepository.UpdateScore(score);
        public async Task DeleteScore(int id) => await _scoreRepository.DeleteScore(id);
    }
}
