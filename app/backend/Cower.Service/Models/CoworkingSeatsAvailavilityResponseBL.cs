namespace Cower.Service.Models;

public record CoworkingSeatsAvailavilityResponseBL(
    DateOnly Date,
    IDictionary<long, IReadOnlyCollection<CoworkingSeatsAvailavilityTimeSlotBL>> Availability);
    
public record CoworkingSeatsAvailavilityTimeSlotBL(
    TimeOnly From,
    TimeOnly To);