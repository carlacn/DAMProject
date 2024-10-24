using DAMProject.Shared.Models;
using Dapper;
using System.Data;

namespace DAMProject.Server.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserById(int id);
        Task<int> CreateUser(User user);
        Task<int> UpdateUser(User user);
        Task DeleteUser(int id);
    }

    public class UserRepository(IDbConnection localDbConnection) : IUserRepository
    {
        private readonly IDbConnection _localDbConnection = localDbConnection;

        public async Task<IEnumerable<User>> GetUsers()
        {
            var query = @"SELECT id, name, email, password, role, created_at AS CreatedAt FROM users";
            return await _localDbConnection.QueryAsync<User>(query);
        }
        public async Task<User> GetUserById(int id)
        {
            var query = "SELECT id, name, email, password, role, created_at AS CreatedAt FROM users WHERE id = @Id";
            return await _localDbConnection.QuerySingleOrDefaultAsync<User>(query, new { Id = id });
        }

        public async Task<int> CreateUser(User user)
        {
            var query = "INSERT INTO users (name, email, password, role, created_at) VALUES (@Name, @Email, @Password, @Role, @CreatedAt)";
            var result = await _localDbConnection.ExecuteAsync(query, new { user.Name, user.Email, user.Password, Role = user.Role.ToString(), user.CreatedAt });

            return result > 0 ? result : throw new Exception("Error al insertar el usuario.");
        }

        public async Task<int> UpdateUser(User newUser)
        {
            var currentUser = await _localDbConnection.QuerySingleOrDefaultAsync<User>(
                    "SELECT * FROM users WHERE id = @Id", new { newUser.Id });

            if (currentUser == null)
            {
                return 0;
            }

            var updatedUser = MapUser(newUser, currentUser);

            var query = "UPDATE users SET name = @Name, email = @Email, password = @Password, role = @Role WHERE id = @Id";
            var result = await _localDbConnection.ExecuteAsync(query, new
            {
                updatedUser.Name,
                updatedUser.Email,
                updatedUser.Password,
                Role = updatedUser.Role.ToString(),
                updatedUser.Id
            });

            return result > 0 ? result : throw new Exception("Error al actualizar el usuario.");
        }

        public async Task DeleteUser(int id)
        {
            var query = "DELETE FROM users WHERE id = @Id";
            await _localDbConnection.ExecuteAsync(query, new { Id = id });
        }

        private User MapUser(User newUser, User currentUser)
        {
            return new User
            {
                Id = currentUser.Id,
                Name = !string.IsNullOrEmpty(newUser.Name) ? newUser.Name : currentUser.Name,
                Email = !string.IsNullOrEmpty(newUser.Email) ? newUser.Email : currentUser.Email,
                Password = !string.IsNullOrEmpty(newUser.Password) ? newUser.Password : currentUser.Password,
                Role = newUser.Role != default ? newUser.Role : currentUser.Role,
                CreatedAt = newUser.CreatedAt != default ? newUser.CreatedAt : currentUser.CreatedAt
            };
        }
    }
}
