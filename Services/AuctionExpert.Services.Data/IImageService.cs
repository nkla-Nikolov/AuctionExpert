namespace AuctionExpert.Services.Data
{
    using System.Threading.Tasks;

    using AuctionExpert.Data.Models;
    using Microsoft.AspNetCore.Http;

    public interface IImageService
    {
        Task<Image> UploadImage(IFormFileCollection images);
    }
}
