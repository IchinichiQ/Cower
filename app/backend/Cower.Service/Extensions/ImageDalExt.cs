using Cower.Data.Models;
using Cower.Domain.Models;
using Cower.Service.Models;
using Cower.Service.Services;

namespace Cower.Service.Extensions;

public static class ImageDalExt
{
    public static Image ToImage(this ImageDal dal, IImageLinkGenerator linkGenerator)
    {
        return new Image(
            dal.Id,
            linkGenerator.GetImageLink(dal.Id),
            dal.Extension,
            dal.Size,
            dal.Type);
    }
}