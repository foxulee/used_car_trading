using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using project_vega.Core;
using project_vega.Core.Models;

namespace project
{
    public class PhotoService : IPhotoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoStorage _photoStorage;

        public PhotoService(IUnitOfWork unitOfWork, IPhotoStorage photoStorage)
        {
            _unitOfWork = unitOfWork;
            _photoStorage = photoStorage;
        }
        public async Task<Photo> UploadPhoto(Vehicle vehicle, IFormFile file, string uploadsFolderPath)
        {
            //generate thumbnails

            //update the database
            var photo = new Photo { FileName = await _photoStorage.StorePhoto(uploadsFolderPath, file) };


            vehicle.Photos.Add(photo);
            await _unitOfWork.CompleteAsync();

            return photo;
        }
    }
}