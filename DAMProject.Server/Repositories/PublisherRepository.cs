using DAMProject.Shared.Models;
using Dapper;
using System.Data;

namespace DAMProject.Server.Repositories
{
    public interface IPublisherRepository
    {
        Task<IEnumerable<Publisher>> GetPublishers();
        Task<Publisher> GetPublisherById(int id);
        Task<int> CreatePublisher(Publisher publisher);
        Task<int> UpdatePublisher(Publisher publisher);
        Task DeletePublisher(int id);
    }

    public class PublisherRepository(IDbConnection localDbConnection) : IPublisherRepository
    {
        private readonly IDbConnection _localDbConnection = localDbConnection;

        public async Task<IEnumerable<Publisher>> GetPublishers()
        {
            var query = @"SELECT id, name, edition FROM publishers";
            return await _localDbConnection.QueryAsync<Publisher>(query);
        }
        public async Task<Publisher> GetPublisherById(int id)
        {
            var query = @"SELECT id, name, edition FROM publishers WHERE id = @Id";
            return await _localDbConnection.QuerySingleOrDefaultAsync<Publisher>(query, new { Id = id });
        }

        public async Task<int> CreatePublisher(Publisher publisher)
        {
            var query = @"INSERT INTO publishers (name, edition) VALUES (@Name, @Edition)";
            var result = await _localDbConnection.ExecuteAsync(query, new { publisher.Name, publisher.Edition });

            return result > 0 ? result : throw new Exception("Error al insertar el editor.");
        }

        public async Task<int> UpdatePublisher(Publisher newPublisher)
        {
            var currentPublisher = await _localDbConnection.QuerySingleOrDefaultAsync<Publisher>(
                    "SELECT * FROM publishers WHERE id = @Id", new { newPublisher.Id });

            if (currentPublisher == null)
            {
                return 0;
            }

            var updatedPublisher = MapPublisher(newPublisher, currentPublisher);

            var query = "UPDATE publishers SET name = @Name, edition = @Edition WHERE id = @Id";
            var result = await _localDbConnection.ExecuteAsync(query, new
            {
                updatedPublisher.Name,
                updatedPublisher.Edition,
                updatedPublisher.Id
            });

            return result > 0 ? result : throw new Exception("Error al actualizar el editor.");
        }

        public async Task DeletePublisher(int id)
        {
            var query = @"DELETE FROM publishers WHERE id = @Id";
            await _localDbConnection.ExecuteAsync(query, new { Id = id });
        }
        private Publisher MapPublisher(Publisher newPublisher, Publisher currentPublisher)
        {
            return new Publisher
            {
                Id = currentPublisher.Id,
                Name = !string.IsNullOrEmpty(newPublisher.Name) ? newPublisher.Name : currentPublisher.Name,
                Edition = !string.IsNullOrEmpty(newPublisher.Edition) ? newPublisher.Edition : currentPublisher.Edition
            };
        }
    }
}
