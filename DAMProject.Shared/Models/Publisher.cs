using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAMProject.Shared.Models
{
    public class Publisher
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Edition { get; set; }
    }
}
