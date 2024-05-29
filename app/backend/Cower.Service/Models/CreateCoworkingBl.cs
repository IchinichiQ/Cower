namespace Cower.Service.Models;

public sealed record CreateCoworkingBl(
    string Address,
    IReadOnlyCollection<CreateCoworkingWorkingTimeBl> WorkingTimes);