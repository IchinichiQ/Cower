namespace Cower.Service.Models;

public sealed record CreateSeatBl(
    long FloorId,
    int Number,
    decimal Price,
    string? Description,
    long ImageId,
    int X,
    int Y,
    int Width,
    int Height,
    double Angle);