using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace senai.spmg.webAPI.Model
{
    public class FileUpload
    {
        public IFormFile files { get; set; }
    }
}
