using MassTransit;
using MDA.Restaraunt.Messages;
using Microsoft.Extensions.Logging;

namespace MDA.Restaraunt.Notification.Consumers
{
    public class NotifyConsumer : IConsumer<INotify>
    {
        private readonly Notifier _notifier;
        private readonly ILogger _logger;
        public NotifyConsumer(Notifier notifier, ILogger<NotifyConsumer> logger)
        {
            _notifier = notifier;
            _logger = logger;
        }

        public Task Consume(ConsumeContext<INotify> context)
        {
            _logger.Log(LogLevel.Information, $"Notify: {context.Message.OrderId} -{DateTime.Now} ");
            _notifier.Notify(context.Message.OrderId, context.Message.ClientId, context.Message.Message);

            return context.ConsumeCompleted;
        }
    }
}