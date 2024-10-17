using DAMProject.Server.Repositories;
using DAMProject.Shared.Models;

namespace DAMProject.Server.Services
{
    public interface ISeriesService
    {
        Task<IEnumerable<Serie>> GetSeries();
        Task<Serie> GetSeriesById(int id);
        Task CreateSeries(Serie series);
        Task UpdateSeries(Serie series);
        Task DeleteSeries(int id);
    }

    public class SeriesService(ISeriesRepository seriesRepository) : ISeriesService
    {
        private readonly ISeriesRepository _seriesRepository = seriesRepository;

        public async Task<IEnumerable<Serie>> GetSeries() => await _seriesRepository.GetSeries();
        public async Task<Serie> GetSeriesById(int id) => await _seriesRepository.GetSeriesById(id);
        public async Task CreateSeries(Serie series) => await _seriesRepository.CreateSeries(series);
        public async Task UpdateSeries(Serie series) => await _seriesRepository.UpdateSeries(series);
        public async Task DeleteSeries(int id) => await _seriesRepository.DeleteSeries(id);
    }
}
