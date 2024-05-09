namespace Cower.Domain.Models.Coworking;

public sealed record CoworkingSeatPosition(
    int X,
    int Y,
    int Width,
    int Height,
    double Angle);