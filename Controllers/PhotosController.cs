using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using project_vega.Controllers.Resources;
using project_vega.Core;
using project_vega.Core.Models;
using project_vega.Persistence;

namespace project_vega.Controllers
{
    [Produces("application/json")]
    [Route("api/vehicles/{vehicleId}/Photos")]
    public class PhotosController : Controller
    {
        //move these settings to appsettings.json, and inject in Startup.cs, retrieve in IOptionsSnapshot
        //private readonly int MAX_BYTES = 10 * 1024 * 1024;
        //private readonly string[] ACCEPTED_FILE_TYPES = new[]{".jpg", ".jpeg", ".png"};
        private readonly PhotoSettings _photoSettings;

        private readonly IHostingEnvironment _host;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoRepository _photoRepository;
        private readonly IPhotoService _photoService;

        public PhotosController(IHostingEnvironment host, IVehicleRepository vehicleRepository, IMapper mapper, IOptionsSnapshot<PhotoSettings> options, IPhotoRepository photoRepository, IPhotoService photoService)
        {
            this._host = host;
            this._vehicleRepository = vehicleRepository;
            this._mapper = mapper;
            this._photoRepository = photoRepository;
            this._photoService = photoService;
            this._photoSettings = options.Value;
        }


        [HttpPost]
        public async Task<IActionResult> Upload(int vehicleId, IFormFile file)
        {
            //IFormFile is for uploading/downloading one file
            //IFormCollection is for multiple files

            var vehicle = await _vehicleRepository.GetVehicle(vehicleId, includeRelated: false);

            if (vehicle == null) return NotFound();
            if (file == null) return BadRequest("Null file");
            if (file.Length == 0) return BadRequest("Empty file");
            if (file.Length > _photoSettings.MaxBytes) return BadRequest("Max file size exceeded");
            if (!_photoSettings.IsSupported(file.FileName)) return BadRequest("Invalid file type.");

            var uploadsFolderPath = Path.Combine(_host.WebRootPath, "upload");
            var photo = await _photoService.UploadPhoto(vehicle, file, uploadsFolderPath);

            return Ok(_mapper.Map<Photo, PhotoResource>(photo));
        }

        public async Task<IEnumerable<PhotoResource>> GetPhotos(int vehicleId)
        {
            var photos = await _photoRepository.GetPhotos(vehicleId);
            return _mapper.Map<IEnumerable<Photo>, IEnumerable<PhotoResource>>(photos);
        }
    }
}