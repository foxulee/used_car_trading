using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace project_vega.Core
{
    public interface IPhotoStorage
    {
        Task<string> StorePhoto(string uploadsFolderPath, IFormFile file);
    }
}