using MassTransit;
using MDA.Restaraunt.Booking.Entities;
using MDA.Restaraunt.Messages;
using MDA.Restaraunt.Messages.Repository;

namespace MDA.Restaraunt.Booking.Consumers
{
    public class BookingRequestConsumer : IConsumer<IBookingRequest>
    {

        private readonly Restaurant _restaurant;
        private readonly IBookingRequestRepository _bookingRequestRepository;
        public BookingRequestConsumer(Restaurant restaurant, IBookingRequestRepository bookingRequestRepository)
        {
            _restaurant = restaurant;
            _bookingRequestRepository = bookingRequestRepository;
        }

        public async Task Consume(ConsumeContext<IBookingRequest> context)
        {
            //var rnd = new Random().Next(1000, 10000);
            //if (rnd > 8000)
            //{
            //    throw new Exception("Ошибка при бронировании !!!!!!");
            //}

            BookingRequestModel model = await _bookingRequestRepository.GetByOrderIdAsync(context.Message.OrderId);
            if (model != null)
            {
                return;
            }
            var requestModel = new BookingRequestModel()
            {
                OrderId = context.Message.OrderId,
                ClientId = context.Message.ClientId,
                PreOrder = context.Message.PreOrder,
                CreationDate = context.Message.CreationDate
            };

            Console.WriteLine($"[OrderId: {context.Message.OrderId}]");
            var isAdd = await _bookingRequestRepository.AddAsync(requestModel);
            if (!isAdd)
            {
                Console.WriteLine("Копирование в DB не произведено");
            }
            var result = await _restaurant.BookFreeTableAsync(1);

            await context.Publish<ITableBooked>(new TableBooked(context.Message.OrderId, result ?? false));
        }


    }
}
