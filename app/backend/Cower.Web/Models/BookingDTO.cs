using Cower.Domain.Models.Booking;

namespace Cower.Web.Models;

public class BookingDTO
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long SeatId { get; set; }
    public string CreatedAt { get; set; }
    public string BookingDate { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public BookingStatus Status { get; set; }
    public string? PaymentUrl { get; set; }
    public string? PaymentExpireAt { get; set; }
    public decimal Price { get; set; }
    public int SeatNumber { get; set; }
    public int Floor { get; set; }
    public string CoworkingAddress { get; set; }
}