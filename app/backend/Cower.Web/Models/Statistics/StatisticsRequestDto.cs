using System.ComponentModel.DataAnnotations;
using Cower.Domain.Models.Statistics;
using Cower.Web.Attributes;

namespace Cower.Web.Models.Statistics;

public class StatisticsRequestDto
{
    [Required(ErrorMessage = "Дата начала не указана")]
    [DateOnly(ErrorMessage = "Дата начала передана в неверном формате")]
    public string StartDate { get; set; }
    
    [Required(ErrorMessage = "Дата окончания не указана")]
    [DateOnly(ErrorMessage = "Дата окончания передана в неверном формате")]
    public string EndDate { get; set; }
    
    [RequiredEnum(ErrorMessage = "Шаг статистики не указан")]
    public StatisticsStep Step { get; set; }
}