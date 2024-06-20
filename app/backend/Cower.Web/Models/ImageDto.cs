using Cower.Domain.Models;

namespace Cower.Web.Models;

public class ImageDto
{
    public long Id { get; set; }
    public string Url { get; set; }
    public string Extension { get; set; }
    public long Size { get; set; }
    public ImageType Type { get; set; }
}