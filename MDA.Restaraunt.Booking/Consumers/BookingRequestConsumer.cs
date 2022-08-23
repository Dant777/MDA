using MassTransit;
using MDA.Restaraunt.Booking.Entities;
using MDA.Restaraunt.Messages;

namespace MDA.Restaraunt.Booking.Consumers
{
    public class BookingRequestConsumer : IConsumer<IBookingRequest>
    {

        private readonly Restaurant _restaurant;

        public BookingRequestConsumer(Restaurant restaurant)
        {
            _restaurant = restaurant;
        }

        public async Task Consume(ConsumeContext<IBookingRequest> context)
        {
            throw new Exception("Ошибка при бронировании !!!!!!");
            Console.WriteLine($"[OrderId: {context.Message.OrderId}]");
            var result = await _restaurant.BookFreeTableAsync(1);

            await context.Publish<ITableBooked>(new TableBooked(context.Message.OrderId, result ?? false));
        }


    }
}
