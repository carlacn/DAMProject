using DAMProject.Server.Repositories;
using DAMProject.Shared.Models;

namespace DAMProject.Server.Services
{

    public interface IGenreService
    {
        Task<IEnumerable<Genre>> GetGenres();
        Task<Genre> GetGenreById(int id);
        Task CreateGenre(Genre genre);
        Task UpdateGenre(Genre genre);
        Task DeleteGenre(int id);
    }

    public class GenreService(IGenreRepository genreRepository) : IGenreService
    {
        private readonly IGenreRepository _genreRepository = genreRepository;

        public async Task<IEnumerable<Genre>> GetGenres() => await _genreRepository.GetGenres();
        public async Task<Genre> GetGenreById(int id) => await _genreRepository.GetGenreById(id);
        public async Task CreateGenre(Genre genre) => await _genreRepository.CreateGenre(genre);
        public async Task UpdateGenre(Genre genre) => await _genreRepository.UpdateGenre(genre);
        public async Task DeleteGenre(int id) => await _genreRepository.DeleteGenre(id);
    }
    
}
