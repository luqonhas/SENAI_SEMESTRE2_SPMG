using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using senai.spmg.webAPI.Model;
using System.IO;

namespace senai.spmg.webAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadsController : ControllerBase
    {
        private IWebHostEnvironment _webHostEnvironment;

        public FileUploadsController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        /*[HttpPost]
        public string Upload([FromForm] FileUpload objectFile)
        {
            if (objectFile.files.Length > 0)
            {
                string path = _webHostEnvironment.WebRootPath + "\\uploads\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using (FileStream fileStream = System.IO.File.Create(path + objectFile.files.FileName))
                {
                    objectFile.files.CopyTo(fileStream);
                    fileStream.Flush();
                    return "Uploaded!";
                }
            }
            return "Not Uploaded!";
        } */

        // EXTRA - Método que permite salvar imagens 
        [HttpPost]
        public string Upload([FromForm] FileUpload objectFile)
        {
            if (objectFile.files.Length > 0)
            {
                string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string path = _webHostEnvironment.WebRootPath + "\\uploads\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using (FileStream fileStream = System.IO.File.Create(path + objectFile.files.FileName))
                {
                    objectFile.files.CopyTo(fileStream);
                    fileStream.Flush();
                    return "Uploaded!";
                }
            }
            return "Not Uploaded!";
        }

    }
}
