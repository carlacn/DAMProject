using DAMProject.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAMProject.Shared.DTOs
{
    public class FavoriteBookDTO
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string? Image { get; set; }
        public string Series { get; set; }
        public string Genre { get; set; }
        public string Publisher { get; set; }
        public int FavoriteId { get; set; }
        public Format Format { get; set; }
        public Status Status { get; set; }
        public DateTime? ReadDate { get; set; }

    }
}
