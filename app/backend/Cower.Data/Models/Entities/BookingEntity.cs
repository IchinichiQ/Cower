using System.ComponentModel.DataAnnotations.Schema;
using Cower.Domain.Models.Booking;

namespace Cower.Data.Models.Entities;

public class BookingEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public long UserId { get; set; }
    public long SeatId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateOnly BookingDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public decimal Price { get; set; }
    public int SeatNumber { get; set; }
    public int Floor { get; set; }
    public string CoworkingAddress { get; set; } = default!;
    public BookingStatus Status { get; set; }
    
    [ForeignKey("SeatId")]
    public CoworkingSeatEntity Seat { get; set; }
    
    [ForeignKey("UserId")]
    public UserEntity User { get; set; }
    
    [InverseProperty("Booking")]
    public PaymentEntity? Payment { get; set; }
}