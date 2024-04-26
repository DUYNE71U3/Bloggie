using Microsoft.AspNetCore.Http;

namespace Bloggie.Web.Repositories
{
    public interface IImageRepository
    {
        Task<string> UploadAsync(IFormFile file);
    }
}
