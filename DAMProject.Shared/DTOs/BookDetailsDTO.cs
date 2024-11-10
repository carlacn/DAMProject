namespace DAMProject.Shared.DTOs
{
    public class BookDetailsDTO
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string Genre { get; set; }
        public string Publisher { get; set; }
        public string Series { get; set; }
        public string? Comments { get; set; }
        public string? Image { get; set; }

    }
}
