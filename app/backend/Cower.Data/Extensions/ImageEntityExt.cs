using Cower.Data.Models;
using Cower.Data.Models.Entities;

namespace Cower.Data.Extensions;

internal static class ImageEntityExt
{
    public static ImageDal ToImageDal(this ImageEntity entity)
    {
        return new ImageDal(
            entity.Id,
            entity.Extension,
            entity.Type,
            entity.Size);
    }
}