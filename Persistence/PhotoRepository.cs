using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using project_vega.Core;
using project_vega.Core.Models;

namespace project_vega.Persistence
{
    public class PhotoRepository: IPhotoRepository
    {
        private readonly VegaDbContext _context;

        public PhotoRepository(VegaDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Photo>> GetPhotos(int vehicleId)
        {
            return await _context.Photos
                .Where(p => p.VehicleId == vehicleId)
                .ToListAsync();
        }
    }
}