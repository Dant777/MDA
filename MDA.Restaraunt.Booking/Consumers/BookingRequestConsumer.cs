using MassTransit;
using MDA.Restaraunt.Booking.Entities;
using MDA.Restaraunt.Messages;
using MDA.Restaraunt.Messages.Repository;
using Microsoft.Extensions.Logging;

namespace MDA.Restaraunt.Booking.Consumers
{
    public class BookingRequestConsumer : IConsumer<IBookingRequest>
    {

        private readonly Restaurant _restaurant;
        private readonly IBookingRequestRepository _bookingRequestRepository;
        private readonly ILogger _logger;

        public BookingRequestConsumer(Restaurant restaurant, IBookingRequestRepository bookingRequestRepository, ILogger<BookingRequestConsumer> logger)
        {
            _restaurant = restaurant;
            _bookingRequestRepository = bookingRequestRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IBookingRequest> context)
        {

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

            _logger.Log(LogLevel.Information, $"[OrderId: {context.Message.OrderId}]");
            var isAdd = await _bookingRequestRepository.AddAsync(requestModel);
            if (!isAdd)
            {
                _logger.Log(LogLevel.Debug, "Копирование в DB не произведено");
            }
            var result = await _restaurant.BookFreeTableAsync(1);

            await context.Publish<ITableBooked>(new TableBooked(context.Message.OrderId, result ?? false));
        }


    }
}
