using System.ComponentModel.DataAnnotations;
using Cower.Web.Attributes;

namespace Cower.Web.Models;

public class GetCoworkingFloorAvailabilityDto
{
    [Required(ErrorMessage = "Дата не указана")]
    [DateOnly(ErrorMessage = "Дата передана в неверном формате")]
    public string Date { get; set; }
}