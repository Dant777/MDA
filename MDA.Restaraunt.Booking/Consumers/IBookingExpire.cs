namespace MDA.Restaraunt.Booking.Consumers;

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
