using Cower.Domain.Models;
using Cower.Web.Models;

namespace Cower.Web.Extensions;

internal static class ImageExt
{
    public static ImageDto ToImageDto(this Image image)
    {
        return new ImageDto
        {
            Id = image.Id,
            Url = image.Url,
            Extension = image.Extension,
            Size = image.Size,
            Type = image.Type
        };
    }
}