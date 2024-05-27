using Cower.Data.Models;
using Cower.Data.Repositories;
using Cower.Domain.Models;
using Cower.Service.Exceptions;
using Cower.Service.Extensions;
using Cower.Service.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Cower.Service.Services.Implementation;

public class ImageService : IImageService
{
    private const string IMAGE_FILE_TEMPLATE = "/cower/files/images/{0}.{1}";
    private const string IMAGE_DIRECTORY = "/cower/files/images";
    
    private readonly ILogger<ImageService> _logger;
    private readonly IImageRepository _imageRepository;
    private readonly IImageLinkGenerator _imageLinkGenerator;

    public ImageService(
        ILogger<ImageService> logger,
        IImageRepository imageRepository,
        IImageLinkGenerator imageLinkGenerator)
    {
        _logger = logger;
        _imageRepository = imageRepository;
        _imageLinkGenerator = imageLinkGenerator;

        if (!Directory.Exists(IMAGE_DIRECTORY))
        {
            Directory.CreateDirectory(IMAGE_DIRECTORY);
        }
    }

    public async Task<Image> UploadImage(UploadImageBl image)
    {
        var dal = new AddImageDal(
            image.Extension,
            image.Image.Length,
            image.Type);

        var imageDal = await _imageRepository.AddImage(dal);

        var path = string.Format(IMAGE_FILE_TEMPLATE, imageDal.Id, imageDal.Extension);
        await File.WriteAllBytesAsync(path, image.Image);

        return imageDal.ToImage(_imageLinkGenerator);
    }

    public async Task<ImageBl?> GetImage(long id)
    {
        var imageDal = await _imageRepository.GetImage(id);
        if (imageDal == null)
        {
            return null;
        }

        var path = string.Format(IMAGE_FILE_TEMPLATE, imageDal.Id, imageDal.Extension);
        if (!File.Exists(path))
        {
            return null;
        }
        
        var imageBytes = await File.ReadAllBytesAsync(path);

        return new ImageBl(
            imageDal.Id,
            _imageLinkGenerator.GetImageLink(id),
            imageDal.Extension,
            imageDal.Size,
            imageDal.Type,
            imageBytes);
    }

    public async Task<IReadOnlyCollection<Image>> GetImages()
    {
        var images = await _imageRepository.GetImages();

        return images
            .Select(x => x.ToImage(_imageLinkGenerator))
            .ToArray();
    }

    public async Task<bool> DeleteImage(long id)
    {
        try
        {
            return await _imageRepository.DeleteImage(id);
        }
        catch (PostgresException pgEx) when (pgEx.SqlState == PostgresErrorCodes.ForeignKeyViolation)
        {
            throw new EntityUsedByOthersException();
        }
    }
}