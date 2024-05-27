using Cower.Data.Models;
using Cower.Domain.Models;

namespace Cower.Data.Repositories;

public interface IImageRepository
{
    Task<ImageDal> AddImage(AddImageDal dal);
    Task<ImageDal?> GetImage(long id);
    Task<ImageType?> GetImageType(long id);
    Task<IReadOnlyCollection<ImageDal>> GetImages();
    Task<bool> DeleteImage(long id);
}