using DAMProject.Server.Repositories;
using DAMProject.Shared.Models;

namespace DAMProject.Server.Services
{
    public interface IPublisherService
    {
        Task<IEnumerable<Publisher>> GetPublishers();
        Task<Publisher> GetPublisherById(int id);
        Task CreatePublisher(Publisher publisher);
        Task UpdatePublisher(Publisher publisher);
        Task DeletePublisher(int id);
    }

    public class PublisherService(IPublisherRepository publisherRepository) : IPublisherService
    {
        private readonly IPublisherRepository _publisherRepository = publisherRepository;

        public async Task<IEnumerable<Publisher>> GetPublishers() => await _publisherRepository.GetPublishers();
        
        public async Task<Publisher> GetPublisherById(int id) => await _publisherRepository.GetPublisherById(id);
        
        public async Task CreatePublisher(Publisher publisher) => await _publisherRepository.CreatePublisher(publisher);
        
        public async Task UpdatePublisher(Publisher publisher) => await _publisherRepository.UpdatePublisher(publisher);
        
        public async Task DeletePublisher(int id) => await _publisherRepository.DeletePublisher(id);
    }
}
