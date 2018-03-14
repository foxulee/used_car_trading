using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using project_vega.Core.Models;
using project_vega.Core;
using project_vega.Extensions;

namespace project_vega.Persistence
{

    public class VehicleRepository : IVehicleRepository
    {
        private readonly VegaDbContext _context;

        public VehicleRepository(VegaDbContext context)
        {
            _context = context;
        }
        public async Task<Vehicle> GetVehicle(int id, bool includeRelated = true)
        {
            if (!includeRelated)
            {
                return await _context.Vehicles.FindAsync(id);
            }
            return await _context.Vehicles
                .Include(v => v.Model)
                .ThenInclude(mk => mk.Make)
                .Include(v => v.Features)
                .ThenInclude(vf => vf.Feature)
                .SingleOrDefaultAsync(v => v.Id == id);
        }

        public async Task<QueryResult<Vehicle>> GetVehicles(VehicleQuery vehicleQuery)
        {
            QueryResult<Vehicle> result = new QueryResult<Vehicle>();

            var query = _context.Vehicles
                .Include(v => v.Model)
                .ThenInclude(mk => mk.Make)
                .AsQueryable();

            //filtering
            query.ApplyFiltering(vehicleQuery);

            //sorting
            var columnsMap = new Dictionary<string, Expression<Func<Vehicle, object>>>
            {
                ["make"] = v => v.Model.Make.Name,
                ["model"] = v => v.Model.Name,
                ["contactName"] = v => v.ContactName,
            };
            query = query.ApplyOrdering(vehicleQuery, columnsMap);

            //count
            result.TotalItems = await query.CountAsync();

            //paging
            query = query.ApplyPaging(vehicleQuery);
            
            
            result.Items = await query.ToListAsync();
            

            return result;
        }



        public void Add(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
        }

        public void Remove(Vehicle vehicle)
        {
            _context.Vehicles.Remove(vehicle);
        }
    }
}