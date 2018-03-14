using System.Collections.Generic;
using System.Threading.Tasks;
using project_vega.Core.Models;

namespace project_vega.Core
{
    public interface IVehicleRepository
    {
        Task<Vehicle> GetVehicle(int id, bool includeRelated = true);
        Task<QueryResult<Vehicle>> GetVehicles(VehicleQuery vehicleQuery);

        void Add(Vehicle vehicle);
        void Remove(Vehicle vehicle);
    }
}

