namespace Cower.Data.Models;

public sealed record AddCoworkingDal(
    string Address,
    IReadOnlyCollection<AddCoworkingWorkingTimeDal> WorkingTimes);