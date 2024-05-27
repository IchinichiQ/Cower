using Cower.Domain.Models;

namespace Cower.Data.Models;

public sealed record ImageDal(
    long Id,
    string Extension,
    ImageType Type,
    long Size);