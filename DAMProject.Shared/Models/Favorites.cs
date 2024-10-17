using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAMProject.Shared.Models
{
    public class Favorites
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public required string Name { get; set; }
    }
}
