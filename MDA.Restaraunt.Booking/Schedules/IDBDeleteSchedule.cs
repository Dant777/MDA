namespace MDA.Restaraunt.Booking.Schedules
{
    public interface IDBDeleteSchedule
    {
        public Guid OrderId { get; set; }
    }

    public class DbDeleteSchedule : IDBDeleteSchedule
    {
        public Guid OrderId { get; set; }
    }
}
