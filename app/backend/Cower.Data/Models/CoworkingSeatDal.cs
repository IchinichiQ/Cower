using Cower.Data.Models.Entities;
using Cower.Domain.Models.Coworking;

namespace Cower.Data.Models;

public record CoworkingSeatDal(
    long Id,
    long FloorId,
    int Number,
    decimal Price,
    string ImageFilename,
    string? Description,
    int X,
    int Y,
    int Width,
    int Height,
    double Angle);