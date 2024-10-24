﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
