namespace Cower.Web.Models;

public class BookingsResponseDTO
{
    public IReadOnlyCollection<BookingDTO> Bookings { get; set; }
}