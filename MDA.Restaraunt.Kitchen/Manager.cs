using MassTransit;
using MDA.Restaraunt.Messages;

namespace MDA.Restaraunt.Kitchen;

internal class Manager
{
    private readonly IBus _bus;

    public Manager(IBus bus)
    {
        _bus = bus;
    }
    public bool CheckKitchenReady(Guid orderId, Dish? dish)
    {
        return true;
    }
}