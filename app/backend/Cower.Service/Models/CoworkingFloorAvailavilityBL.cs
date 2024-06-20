namespace Cower.Service.Models;

public record CoworkingFloorAvailavilityBL(
    DateOnly Date,
    IDictionary<long, IReadOnlyCollection<CoworkingSeatsAvailavilityTimeSlotBL>> Availability);
    
public record CoworkingSeatsAvailavilityTimeSlotBL(
    TimeOnly From,
    TimeOnly To);