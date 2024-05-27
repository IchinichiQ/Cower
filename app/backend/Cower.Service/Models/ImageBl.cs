using Cower.Domain.Models;

namespace Cower.Service.Models;

public sealed record ImageBl(
    long Id,
    string Url,
    string Extension,
    long Size,
    ImageType Type,
    byte[] Image);