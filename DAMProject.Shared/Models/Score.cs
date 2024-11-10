namespace DAMProject.Shared.Models
{
    public class Score
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int UserRating { get; set; }
        public required string Comment { get; set; }
    }
}
