using DAMProject.Server.Repositories;
using DAMProject.Shared.Models;

namespace DAMProject.Server.Services
{
    public interface IFavoritesService
    {
        Task<IEnumerable<Favorites>> GetFavorites();
        Task<Favorites> GetFavoriteById(int id);
        Task CreateFavorite(Favorites favorite);
        Task UpdateFavorite(Favorites favorite);
        Task DeleteFavorite(int id);
    }

    public class FavoritesService(IFavoritesRepository favoritesRepository) : IFavoritesService
    {
        private readonly IFavoritesRepository _favoritesRepository = favoritesRepository;

        public async Task<IEnumerable<Favorites>> GetFavorites() => await _favoritesRepository.GetFavorites();
        public async Task<Favorites> GetFavoriteById(int id) => await _favoritesRepository.GetFavoriteById(id);
        public async Task CreateFavorite(Favorites favorite) => await _favoritesRepository.CreateFavorite(favorite);
        public async Task UpdateFavorite(Favorites favorite) => await _favoritesRepository.UpdateFavorite(favorite);
        public async Task DeleteFavorite(int id) => await _favoritesRepository.DeleteFavorite(id);
    }
}
