﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAMProject.Shared.Request
{
    public class LoginRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
