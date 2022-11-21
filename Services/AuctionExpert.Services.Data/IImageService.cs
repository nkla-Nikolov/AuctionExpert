namespace AuctionExpert.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Models;
    using Microsoft.AspNetCore.Http;

    public interface IImageService
    {
        Task<IEnumerable<Image>> UploadImages(IFormFileCollection images);
    }
}
