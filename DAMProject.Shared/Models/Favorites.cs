using System.Text.Json.Serialization;

namespace DAMProject.Shared.Models
{
    public class Favorites
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public Format Format { get; set; } 
        public Status Status { get; set; }
        public DateTime? ReadDate { get; set; }

        public static bool IsFormatValid(Format format)
        {
            return Enum.IsDefined(typeof(Format), format);
        }

        public static bool IsStatusValid(Status status)
        {
            return Enum.IsDefined(typeof(Status), status);
        }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Format
    {
        Digital,
        Physical
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Status
    {
        Pending,
        Read,
        CurrentRead,
        Abandoned
    }

}
