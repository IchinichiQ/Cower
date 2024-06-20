namespace Cower.Domain.Models;

public record Image(
    long Id,
    string Url,
    string Extension,
    long Size,
    ImageType Type);