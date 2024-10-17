using DAMProject.Shared.Models;
using Dapper;
using System.Data;

namespace DAMProject.Server.Repositories
{
    public interface ISeriesRepository
    {
        Task<IEnumerable<Serie>> GetSeries();
        Task<Serie> GetSeriesById(int id);
        Task<int> CreateSeries(Serie series);
        Task<int> UpdateSeries(Serie series);
        Task DeleteSeries(int id);
    }

    public class SerieRepository(IDbConnection localDbConnection) : ISeriesRepository
    {
        private readonly IDbConnection _localDbConnection = localDbConnection;

        public async Task<IEnumerable<Serie>> GetSeries()
        {
            var query = @"SELECT id, name, description FROM series";
            return await _localDbConnection.QueryAsync<Serie>(query);
        }
        public async Task<Serie> GetSeriesById(int id)
        {
            var query = @"SELECT id, name, description FROM series WHERE id = @Id";
            return await _localDbConnection.QuerySingleOrDefaultAsync<Serie>(query, new { Id = id });
        }

        public async Task<int> CreateSeries(Serie series)
        {
            var query = @"INSERT INTO series (name, description) VALUES (@Name, @Description)";
            var result = await _localDbConnection.ExecuteAsync(query, new { series.Name, series.Description });

            return result > 0 ? result : throw new Exception("Error al insertar la serie.");
        }

        public async Task<int> UpdateSeries(Serie newSeries)
        {
            var currentSeries = await _localDbConnection.QuerySingleOrDefaultAsync<Serie>(
                    "SELECT * FROM series WHERE id = @Id", new { newSeries.Id });

            if (currentSeries == null)
            {
                return 0;
            }

            var updatedSeries = MapSeries(newSeries, currentSeries);

            var query = "UPDATE series SET name = @Name, description = @Description WHERE id = @Id";
            var result = await _localDbConnection.ExecuteAsync(query, new { updatedSeries.Name, updatedSeries.Description, updatedSeries.Id });

            return result > 0 ? result : throw new Exception("Error al actualizar la serie.");
        }

        public async Task DeleteSeries(int id)
        {
            var query = @"DELETE FROM series WHERE id = @Id";
            await _localDbConnection.ExecuteAsync(query, new { Id = id });
        }
        private Serie MapSeries(Serie newSeries, Serie currentSeries)
        {
            return new Serie
            {
                Id = currentSeries.Id,
                Name = !string.IsNullOrEmpty(newSeries.Name) ? newSeries.Name : currentSeries.Name,
                Description = !string.IsNullOrEmpty(newSeries.Description) ? newSeries.Description : currentSeries.Description
            };
        }
    }
}
