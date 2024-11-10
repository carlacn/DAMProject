namespace DAMProject.Shared.Models
{
    public class Publisher
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Edition { get; set; }
    }
}
