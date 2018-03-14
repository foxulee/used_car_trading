using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using project_vega.Core;

namespace project
{
    public class FileSystemPhotoStorage : IPhotoStorage
    {
        public async Task<string> StorePhoto(string uploadsFolderPath, IFormFile file)
        {
            //create a folder to store the uploaded photes
            //_host.WebRootPath returns path of wwwroot

            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            //change photo name
            var fileName = Guid.NewGuid().ToString()
                           + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            //storing photos in wwwroot
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }
    }
}