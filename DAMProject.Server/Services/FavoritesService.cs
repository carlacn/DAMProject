using DAMProject.Server.Queries;
using DAMProject.Server.Repositories;
using DAMProject.Shared.DTOs;
using DAMProject.Shared.Models;

namespace DAMProject.Server.Services
{
    public interface IFavoritesService
    {
        Task<IEnumerable<Favorites>> GetFavorites();
        Task<Favorites> GetFavoriteById(int id);
        Task CreateFavorite(Favorites favorite);
        Task<IEnumerable<FavoriteBookDTO>> GetUserFavorites(int userId);
        Task UpdateFavorite(Favorites favorite);
        Task DeleteFavorite(int id);
    }

    public class FavoritesService(IFavoritesRepository favoritesRepository, IBookDetailsQueries bookDetailsQueries) : IFavoritesService
    {
        private readonly IFavoritesRepository _favoritesRepository = favoritesRepository;
        private readonly IBookDetailsQueries _bookDetailsQueries = bookDetailsQueries;

        public async Task<IEnumerable<Favorites>> GetFavorites() => await _favoritesRepository.GetFavorites();
        public async Task<Favorites> GetFavoriteById(int id) => await _favoritesRepository.GetFavoriteById(id);
        public async Task CreateFavorite(Favorites favorite) => await _favoritesRepository.CreateFavorite(favorite);
        public async Task<IEnumerable<FavoriteBookDTO>> GetUserFavorites(int userId) => await _bookDetailsQueries.GetUserFavorites(userId);
        public async Task UpdateFavorite(Favorites favorite) => await _favoritesRepository.UpdateFavorite(favorite);
        public async Task DeleteFavorite(int id) => await _favoritesRepository.DeleteFavorite(id);
    }
}
