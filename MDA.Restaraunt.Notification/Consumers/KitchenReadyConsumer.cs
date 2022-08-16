﻿using MassTransit;
using MDA.Restaraunt.Messages;

namespace MDA.Restaraunt.Notification.Consumers
{
    public class KitchenReadyConsumer : IConsumer<IKitchenReady>
    {

        private readonly Notifier _notifier;

        public KitchenReadyConsumer(Notifier notifier)
        {
            _notifier = notifier;
        }

        public Task Consume(ConsumeContext<IKitchenReady> context)
        {
            _notifier.Accept(context.Message.OrderId, Accepted.Kitchen);

            return Task.CompletedTask;
        }
    }
}