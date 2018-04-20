using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace project_vega.Core
{
    // In production mode, photo could be stored on cloud storage, instead of local storage
    public interface IPhotoStorage
    {
        Task<string> StorePhoto(string uploadsFolderPath, IFormFile file);
    }
}