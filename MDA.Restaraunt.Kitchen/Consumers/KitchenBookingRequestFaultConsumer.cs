using MassTransit;
using MDA.Restaraunt.Messages;
using Microsoft.Extensions.Logging;

namespace MDA.Restaraunt.Kitchen.Consumers
{
    public class KitchenBookingRequestFaultConsumer : IConsumer<Fault<IBookingRequest>>
    {
        private readonly ILogger _logger;

        public KitchenBookingRequestFaultConsumer(ILogger<KitchenBookingRequestFaultConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<Fault<IBookingRequest>> context)
        {
            _logger.Log(LogLevel.Error, $"[OrderId {context.Message.Message.OrderId}] Отмена на кухне");
            return Task.CompletedTask;
        }
    }
}