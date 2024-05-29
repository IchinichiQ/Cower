namespace Cower.Data.Models;

public sealed record AddCoworkingSeatDal(
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