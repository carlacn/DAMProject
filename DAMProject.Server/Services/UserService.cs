using DAMProject.Server.Repositories;
using DAMProject.Shared.Models;
using System.Net.Mail;

namespace DAMProject.Server.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserById(int id);
        Task CreateUser(User user);
        Task UpdateUser(User user);
        Task DeleteUser(int id);
    }

    public class UserService(IUserRepository userRepository) : IUserService
    {
        private IUserRepository _userRepository = userRepository;

        public async Task<IEnumerable<User>> GetUsers() => await _userRepository.GetUsers();
        public async Task<User> GetUserById(int id) => await _userRepository.GetUserById(id);
        public async Task CreateUser(User user)
        {
            ValidateEmail(user.Email);

            var existingUser = (await _userRepository.GetUsers()).FirstOrDefault(u => u.Email == user.Email);
            if (existingUser != null)
            {
                throw new Exception("A user with this email already exists.");
            }

            await _userRepository.CreateUser(user);
        }
        public async Task UpdateUser(User user)
        {
            ValidateEmail(user.Email);
            var existingUser = (await _userRepository.GetUsers())
                .FirstOrDefault(u => u.Email == user.Email && u.Id != user.Id);
            if (existingUser != null)
            {
                throw new Exception("Another user with this email already exists.");
            }

            await _userRepository.UpdateUser(user);
        }
        public async Task DeleteUser(int id) => await _userRepository.DeleteUser(id);

        private void ValidateEmail(string email)
        {
            try
            {
                var mail = new MailAddress(email);
            }
            catch (FormatException)
            {
                throw new Exception("The email format is invalid.");
            }
        }
    }
}
