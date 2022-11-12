namespace AuctionExpert.Services.Data
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;

    public class ImageService : IImageService
    {
        private readonly IDeletableEntityRepository<Image> imageRepository;
        private readonly Cloudinary cloudinary;

        public ImageService(IDeletableEntityRepository<Image> imageRepository, Cloudinary cloudinary)
        {
            this.imageRepository = imageRepository;
            this.cloudinary = cloudinary;
        }

        public async Task<Image> UploadImage(IFormFileCollection images)
        {
            throw new NotImplementedException();
        }
    }
}
