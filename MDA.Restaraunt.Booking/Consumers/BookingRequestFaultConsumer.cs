using MassTransit;

namespace MDA.Restaraunt.Messages;

public class BookingRequestFaultConsumer : IConsumer<Fault<IBookingRequest>>
{
    public Task Consume(ConsumeContext<Fault<IBookingRequest>> context)
    {
        Console.WriteLine($"[OrderId {context.Message.Message.OrderId}] Отмена в зале");
        return Task.CompletedTask;
    }
}