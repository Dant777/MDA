using System.ComponentModel;
using System.Text;
using Microsoft.Extensions.Hosting;

namespace MDA.Messenger.RabbitMQ
{
    public class Worker:BackgroundService
    {
        private readonly Consumer _consumer;

        public Worker()
        {
            _consumer = new Consumer("RabbitTestQueue");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Receive((sender, args) =>
            {
                var body = args.Body.ToArray();
                var msg = Encoding.UTF8.GetString(body);
                Console.WriteLine(msg);
            });
        }
    }
}
