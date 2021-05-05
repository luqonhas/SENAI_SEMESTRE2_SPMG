using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace senai.spmg.webAPI.Interfaces
{
    public class ImagemUpload
    {
        public long idImageUploaded { get; set; }
        public string imagePath { get; set; }
        public DateTime insertedOn { get; set; }
    }
}
