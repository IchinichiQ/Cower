namespace Cower.Web.Models;

public class BookingsResponseDto
{
    public IReadOnlyCollection<BookingDto> Bookings { get; set; }
}