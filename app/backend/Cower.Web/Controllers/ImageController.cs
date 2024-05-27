using Cower.Domain.Models;
using Cower.Service.Exceptions;
using Cower.Service.Models;
using Cower.Service.Services;
using Cower.Web.Extensions;
using Cower.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cower.Web.Controllers;

[Route("api/v1/images")]
public class ImageController : ControllerBase
{
    private readonly ILogger<ImageController> _logger;
    private readonly IImageService _imageService;

    private static readonly Dictionary<string, string> ExtenstionToMimeType = new()
    {
        { "jpg", "image/jpeg" },
        { "jpeg", "image/jpeg" },
        { "png", "image/png" },
        { "gif", "image/gif" },
        { "bmp", "image/bmp" },
        { "svg", "image/svg+xml" }
    };
    
    public ImageController(
        ILogger<ImageController> logger,
        IImageService imageService)
    {
        _logger = logger;
        _imageService = imageService;
    }

    [HttpGet("{id}")]
    [EndpointName("GetImage")]
    public async Task<ActionResult> GetImage([FromRoute] long id)
    {
        var image = await _imageService.GetImage(id);
        if (image == null)
        {
            var error = new ErrorDto(
                ErrorCodes.NOT_FOUND,
                "Изображения с таким id не существует");
            return NotFound(error);
        }
        
        return File(image.Image, ExtenstionToMimeType[image.Extension]);
    }

    [HttpGet]
    [Authorize(Roles = AppRoleNames.Admin)]
    public async Task<ActionResult<ImagesDto>> GetImages()
    {
        var images = await _imageService.GetImages();

        return Ok(new ImagesDto
        {
            Images = images
                .Select(x => x.ToImageDto())
                .ToArray()
        });
    }
    
    [HttpPost]
    [Authorize(Roles = AppRoleNames.Admin)]
    public async Task<ActionResult<ImageDto>> UploadImage(
        [FromForm] UploadImageDto dto)
    {
        var ext = Path.GetExtension(dto.Image.FileName).ToLowerInvariant().TrimStart('.');
        if (string.IsNullOrEmpty(ext) || !ExtenstionToMimeType.Keys.Contains(ext))
        {
            var error = new ErrorDto(
                ErrorCodes.INVALID_FILE_TYPE,
                $"Недопустимый формат изображения. Допустимые форматы: {string.Join(", ", ExtenstionToMimeType.Keys)}");
            return BadRequest(error);
        }
        
        if (!ExtenstionToMimeType.Values.Contains(dto.Image.ContentType.ToLowerInvariant()))
        {
            var error = new ErrorDto(
                ErrorCodes.INVALID_FILE_TYPE,
                $"Недопустимый MIME-тип изображения. Допустимые типы: {string.Join(", ", ExtenstionToMimeType.Values)}");
            return BadRequest(error);
        }

        var imageBytes = await dto.Image.GetBytes();
        var bl = new UploadImageBl(
            ext,
            dto.Type,
            imageBytes);
        
        var image = await _imageService.UploadImage(bl);
        
        return Ok(image.ToImageDto());
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = AppRoleNames.Admin)]
    public async Task<ActionResult> DeleteImage([FromRoute] long id)
    {
        bool isDeleted;
        try
        {
            isDeleted = await _imageService.DeleteImage(id);
        }
        catch (EntityUsedByOthersException)
        {
            var error = new ErrorDto(
                ErrorCodes.ENTITY_USED_BY_OTHERS,
                "Изображение используется другими сущностями");
            return BadRequest(error);
        }

        if (!isDeleted)
        {
            var error = new ErrorDto(
                ErrorCodes.NOT_FOUND,
                "Изображения с таким ID не существует");
            return NotFound(error);
        }
        
        return NoContent();
    }
}