using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using project_vega.Core.Models;

namespace project_vega.Core
{
    public interface IPhotoService
    {
        Task<Photo> UploadPhoto(Vehicle vehicle, IFormFile file, string uploadsFolderPath);
    }
}