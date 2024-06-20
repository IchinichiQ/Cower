using Cower.Data.Extensions;
using Cower.Data.Models;
using Cower.Data.Models.Entities;
using Cower.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cower.Data.Repositories.Implementation;

public class ImageRepository : IImageRepository
{
    private readonly ILogger<ImageRepository> _logger;
    private readonly ApplicationContext _db;

    public ImageRepository(
        ILogger<ImageRepository> logger,
        ApplicationContext db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<ImageDal> AddImage(AddImageDal dal)
    {
        var entity = new ImageEntity
        {
            Type = dal.Type,
            Extension = dal.Extension,
            Size = dal.Size
        };
        
        var image = await _db.Images.AddAsync(entity);
        await _db.SaveChangesAsync();

        return image.Entity.ToImageDal();
    }

    public async Task<ImageDal?> GetImage(long id)
    {
        return await _db.Images
            .Where(x => x.Id == id)
            .Select(x => x.ToImageDal())
            .FirstOrDefaultAsync();
    }

    public async Task<ImageType?> GetImageType(long id)
    {
        return await _db.Images
            .Where(x => x.Id == id)
            .Select(x => (ImageType?)x.Type)
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<ImageDal>> GetImages()
    {
        return await _db.Images
            .Select(x => x.ToImageDal())
            .ToArrayAsync();
    }

    public async Task<bool> DeleteImage(long id)
    {
        var deleted = await _db.Images
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();

        return deleted > 0;
    }
}