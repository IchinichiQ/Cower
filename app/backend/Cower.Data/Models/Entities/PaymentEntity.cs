using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cower.Data.Models.Entities;

[Index(nameof(Label), IsUnique = true)]
public class PaymentEntity
{
    public long Id { get; set; }
    public long BookingId { get; set; }
    public string Label { get; set; }
    public bool IsCompleted { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    
    [ForeignKey("BookingId")]
    public BookingEntity Booking { get; set; }
}