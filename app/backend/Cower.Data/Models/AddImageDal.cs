using Cower.Domain.Models;

namespace Cower.Data.Models;

public sealed record AddImageDal(
    string Extension,
    long Size,
    ImageType Type);