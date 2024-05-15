using Cower.Data.Models.Entities;
using Cower.Domain.Models.Coworking;

namespace Cower.Data.Models;

public record SeatDAL(
    long Id,
    long CoworkingId,
    int Floor,
    decimal Price,
    string ImageFilename,
    string? Description,
    int Number,
    CoworkingEntity Coworking);