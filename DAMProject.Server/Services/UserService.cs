using DAMProject.Server.Repositories;
using DAMProject.Shared.Models;

namespace DAMProject.Server.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsers();
    }

    public class UserService(IUserRepository userRepository) : IUserService
    {
        private IUserRepository _userRepository = userRepository;

        public async Task<IEnumerable<User>> GetUsers() => await _userRepository.GetUsers();
    }
}
