using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAMProject.Shared.Models
{
    public class Book
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public int GenreId { get; set; }
        public int PublisherId { get; set; }
        public int? SeriesId { get; set; }
        public DateTime? ReadDate { get; set; }
        public string? Status { get; set; }
        public double? AverageRating { get; set; }
        public string? Comments { get; set; }
        public int UserId { get; set; }
        public string? Format { get; set; }
    }
}
