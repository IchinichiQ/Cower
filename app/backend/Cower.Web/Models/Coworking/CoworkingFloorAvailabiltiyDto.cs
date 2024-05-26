using System.ComponentModel.DataAnnotations;

namespace Cower.Web.Models;

public class CoworkingFloorAvailabiltiyDto
{
    public string Date { get; set; }
    public IDictionary<long, List<CoworkingSeatTimeSlotResponseDto>> Availability { get; set; }
}