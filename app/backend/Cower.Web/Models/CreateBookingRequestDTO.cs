using System.ComponentModel.DataAnnotations;
using Cower.Web.Attributes;

namespace Cower.Web.Models;

public class CreateBookingRequestDTO
{
    [Required(ErrorMessage = "Id рабочего места не указан")]
    public long SeatId { get; set; }
    
    [Required(ErrorMessage = "Дата бронирования не указана")]
    [DateOnly(ErrorMessage = "Некорректный формат даты бронирования")]
    public string BookingDate { get; set; }
    
    [Required(ErrorMessage = "Время начала бронирования не указано")]
    [TimeOnly(ErrorMessage = "Некорректный формат времени начала бронирования")]
    public string StartTime { get; set; }
    
    [Required(ErrorMessage = "Время окончания бронирования не указано")]
    [TimeOnly(ErrorMessage = "Некорректный формат времени окончания бронирования")]
    public string EndTime { get; set; }
}