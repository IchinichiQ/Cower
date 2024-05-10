using System.ComponentModel.DataAnnotations;

namespace Cower.Web.Models;

public class CoworkingSeatAvailabiltiyResponseDTO
{
    public string Date { get; set; }
    public IDictionary<long, List<CoworkingSeatTimeSlotResponseDTO>> Availability { get; set; }
}