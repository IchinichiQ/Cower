namespace Cower.Domain.Models;

public record Image(
    int Id,
    string Url,
    string Extension,
    ImageType Type);