﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAMProject.Shared.Request
{
    public class FileUploadRequest
    {
        public IFormFile File { get; set; }
    }
}
