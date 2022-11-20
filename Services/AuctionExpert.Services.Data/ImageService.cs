namespace AuctionExpert.Services.Data
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore.Query;
    using Microsoft.EntityFrameworkCore.Query.Internal;

    public class ImageService : IImageService
    {
        private readonly Cloudinary cloudinary;

        public ImageService(Cloudinary cloudinary)
        {
            this.cloudinary = cloudinary;
        }

        public async Task<IEnumerable<Image>> UploadImages(IFormFileCollection images)
        {
            var imageList = new List<Image>();

            foreach (var imageFile in images)
            {
                using (var stream = imageFile.OpenReadStream())
                {
                    var parameters = new ImageUploadParams()
                    {
                        File = new FileDescription(imageFile.FileName, stream),
                        Transformation = new Transformation().Width(444).Height(352),
                    };

                    var result = await this.cloudinary.UploadAsync(parameters);

                    var image = new Image()
                    {
                        UrlPath = result.Url.ToString(),
                    };

                    imageList.Add(image);
                }
            }

            return imageList;
        }
    }
}
