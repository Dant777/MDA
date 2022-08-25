using MassTransit;
using Microsoft.Extensions.Logging;

namespace MDA.Restaraunt.Messages;

public class BookingRequestFaultConsumer : IConsumer<Fault<IBookingRequest>>
{
    private readonly ILogger _logger;

    public BookingRequestFaultConsumer(ILogger<BookingRequestFaultConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<Fault<IBookingRequest>> context)
    {
        _logger.Log(LogLevel.Error, $"[OrderId {context.Message.Message.OrderId}] Отмена в зале");
        Console.WriteLine();
        return Task.CompletedTask;
    }
}