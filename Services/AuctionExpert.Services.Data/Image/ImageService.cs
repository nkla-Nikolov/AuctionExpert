namespace AuctionExpert.Services.Data.Image
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Mapping;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;

    public class ImageService : IImageService
    {
        private readonly Cloudinary cloudinary;
        private readonly IDeletableEntityRepository<Image> imageRepository;

        public ImageService(
            Cloudinary cloudinary,
            IDeletableEntityRepository<Image> imageRepository)
        {
            this.cloudinary = cloudinary;
            this.imageRepository = imageRepository;
        }

        public IQueryable<T> GetAllImages<T>(int auctionId)
        {
            return imageRepository
                .AllAsNoTracking()
                .Where(x => x.AuctionId == auctionId)
                .To<T>();
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

                    var result = await cloudinary.UploadAsync(parameters);

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
