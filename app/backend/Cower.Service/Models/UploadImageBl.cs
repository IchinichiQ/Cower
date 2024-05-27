using Cower.Domain.Models;

namespace Cower.Service.Models;

public sealed record UploadImageBl(
    string Extension,
    ImageType Type,
    byte[] Image);