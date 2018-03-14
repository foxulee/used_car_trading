using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_vega.Controllers.Resources;
using project_vega.Core;
using project_vega.Core.Models;
using project_vega.Persistence;

namespace project_vega.Controllers
{
    [Produces("application/json")]
    [Route("api/Vehicles")]
    public class VehicleController : Controller
    {
        private readonly IMapper _iMapper;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public VehicleController(IMapper iMapper, IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork)
        {
            _iMapper = iMapper;
            _vehicleRepository = vehicleRepository;
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatVehicle([FromBody]SaveVehicleResource vehicleResource)
        {
           //server side validation (input validation, checking required, stringlength...)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicle = _iMapper.Map<SaveVehicleResource, Vehicle>(vehicleResource);
            vehicle.LastUpdate = DateTime.Now;

            _vehicleRepository.Add(vehicle);
            await _unitOfWork.CompleteAsync();

            //load the complete vehicle
            vehicle = await _vehicleRepository.GetVehicle(vehicle.Id);

            var result = _iMapper.Map<Vehicle, VehicleResource>(vehicle);

            return Ok(result);
        }

        [Authorize]
        [HttpPut("{id}")]  // url: /api/vehicles/{id}
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody]SaveVehicleResource vehicleResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var vehicle = await _vehicleRepository.GetVehicle(id);

            if (vehicle == null)
            {
                return NotFound();
            }

            _iMapper.Map<SaveVehicleResource, Vehicle>(vehicleResource, vehicle);
            vehicle.LastUpdate = DateTime.Now;

            //_context.Vehicles.Update(vehicle);
            await _unitOfWork.CompleteAsync();

            vehicle = await _vehicleRepository.GetVehicle(vehicle.Id);
            var result = _iMapper.Map<Vehicle, VehicleResource>(vehicle);

            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await _vehicleRepository.GetVehicle(id, includeRelated:false);

            if (vehicle == null)
            {
                return NotFound();
            }

            _vehicleRepository.Remove(vehicle);
            await _unitOfWork.CompleteAsync();

            return Ok(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicle(int id)
        {
            var vehicle = await _vehicleRepository.GetVehicle(id);

            if (vehicle == null)
            {
                return NotFound();
            }

            var result = _iMapper.Map<Vehicle, VehicleResource>(vehicle);
            return Ok(result);
        }

        [HttpGet]
        public async Task<QueryResultResource<VehicleResource>> GetVehicles([FromQuery]VehicleQueryResource vehicleQueryResource)
        {
            
            var queryResult = await _vehicleRepository.GetVehicles(_iMapper.Map<VehicleQueryResource, VehicleQuery>(vehicleQueryResource));
            
            return _iMapper.Map<QueryResult<Vehicle>, QueryResultResource<VehicleResource>>(queryResult);
        }
    }
}