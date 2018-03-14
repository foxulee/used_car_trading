using System.Collections.Generic;
using System.Threading.Tasks;
using project_vega.Core.Models;

namespace project_vega.Core
{
    public interface IPhotoRepository
    {
        Task<IEnumerable<Photo>> GetPhotos(int vehicleId);
    }
}