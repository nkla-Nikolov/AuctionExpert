namespace AuctionExpert.Services.Data.Image
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Models;
    using Microsoft.AspNetCore.Http;

    public interface IImageService
    {
        Task<IEnumerable<Image>> UploadImages(IFormFileCollection images);

        IQueryable<T> GetAllImages<T>(int auctionId);
    }
}
