using MassTransit;
using MDA.Restaraunt.Messages;

namespace MDA.Restaraunt.Kitchen.Consumers
{
    internal class KitchenTableBookedConsumer : IConsumer<IBookingRequest>
    {
        private readonly Manager _manager;

        public KitchenTableBookedConsumer(Manager manager)
        {
            _manager = manager;
        }

        public async Task Consume(ConsumeContext<IBookingRequest> context)
        {
            //var rnd = new Random().Next(1000, 10000);
            //if (rnd > 8000)
            //{
            //    throw new Exception($"Поломка на кухне! - {DateTime.Now}");
            //}

            Console.WriteLine($"[OrderId: {context.Message.OrderId} CreationDate: {context.Message.CreationDate}]");
            Console.WriteLine("Trying time: " + DateTime.Now);
            
            await Task.Delay(5000);

            if (_manager.CheckKitchenReady(context.Message.OrderId, context.Message.PreOrder))
                await context.Publish<IKitchenReady>(new KitchenReady(context.Message.OrderId));
        }
    }
}
