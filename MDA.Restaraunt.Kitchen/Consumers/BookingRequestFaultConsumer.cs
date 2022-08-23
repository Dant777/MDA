using MassTransit;
using MDA.Restaraunt.Messages;

namespace MDA.Restaraunt.Kitchen.Consumers
{
    public class KitchenBookingRequestFaultConsumer : IConsumer<Fault<IBookingRequest>>
    {
        public Task Consume(ConsumeContext<Fault<IBookingRequest>> context)
        {
            
            return Task.CompletedTask;
        }
    }
}