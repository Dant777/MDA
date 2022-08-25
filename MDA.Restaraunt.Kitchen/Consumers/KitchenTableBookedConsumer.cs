using MassTransit;
using MDA.Restaraunt.Messages;
using Microsoft.Extensions.Logging;

namespace MDA.Restaraunt.Kitchen.Consumers
{
    internal class KitchenTableBookedConsumer : IConsumer<IBookingRequest>
    {
        private readonly Manager _manager;
        private readonly ILogger _logger;
        public KitchenTableBookedConsumer(Manager manager, ILogger<KitchenTableBookedConsumer> logger)
        {
            _manager = manager;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IBookingRequest> context)
        {
            _logger.Log(LogLevel.Information, $"[OrderId: {context.Message.OrderId} CreationDate: {context.Message.CreationDate}]");
            _logger.Log(LogLevel.Information, "Trying time: " + DateTime.Now);

            Console.WriteLine();
            
            await Task.Delay(5000);

            if (_manager.CheckKitchenReady(context.Message.OrderId, context.Message.PreOrder))
                await context.Publish<IKitchenReady>(new KitchenReady(context.Message.OrderId));
        }
    }
}
