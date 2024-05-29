using System.ComponentModel.DataAnnotations;
using Cower.Service.Models;

namespace Cower.Web.Models.Coworking;

public class UpdateCoworkingDto
{
    public string? Address { get; set; }
    public IReadOnlyCollection<CreateCoworkingWorkingTimeDto>? WorkingTimes { get; set; }
}