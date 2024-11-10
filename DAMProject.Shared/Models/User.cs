using System.Text.Json.Serialization;

namespace DAMProject.Shared.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required UserRole Role { get; set; }
        public DateTime CreatedAt { get; set; }

        public static bool IsRoleValid(UserRole role)
        {
            return Enum.IsDefined(typeof(UserRole), role);
        }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UserRole
    {
        User,
        Admin
    }
}
