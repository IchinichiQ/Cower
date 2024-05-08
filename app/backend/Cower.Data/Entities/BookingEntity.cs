using System.ComponentModel.DataAnnotations.Schema;

namespace Cower.Data.Entities;

public class BookingEntity
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long SeatId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public decimal Price { get; set; }
    public int SeatNumber { get; set; }
    public int Floor { get; set; }
    public string CoworkingAddress { get; set; } = default!;
    public string Status { get; set; } = default!;
    
    [ForeignKey("SeatId")]
    public CoworkingSeatEntity Seat { get; set; }
    
    [ForeignKey("UserId")]
    public UserEntity User { get; set; }
}