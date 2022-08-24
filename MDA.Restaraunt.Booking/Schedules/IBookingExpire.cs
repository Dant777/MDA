namespace MDA.Restaraunt.Booking.Schedules;

public interface IBookingExpire
{
    public Guid OrderId { get; }
}

public class BookingExpire : IBookingExpire
{


    public BookingExpire()
    {

    }

    public Guid OrderId { get; set; }
}
