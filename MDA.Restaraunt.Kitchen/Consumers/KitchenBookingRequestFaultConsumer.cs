using MassTransit;
using MDA.Restaraunt.Messages;

namespace MDA.Restaraunt.Kitchen.Consumers
{
    public class KitchenBookingRequestFaultConsumer : IConsumer<Fault<IBookingRequest>>
    {
        public Task Consume(ConsumeContext<Fault<IBookingRequest>> context)
        {
            Console.WriteLine($"[OrderId {context.Message.Message.OrderId}] Отмена в зале");
            return Task.CompletedTask;
        }
    }
}