using Cower.Data.Models;
using Cower.Domain.Models;
using Cower.Service.Models;

namespace Cower.Service.Services;

public interface IImageService
{
    Task<Image> UploadImage(UploadImageBl image);
    Task<ImageBl?> GetImage(long id);
    Task<IReadOnlyCollection<Image>> GetImages();
    Task<bool> DeleteImage(long id);
}