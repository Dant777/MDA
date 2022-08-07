using System.Text;
using MDA.Messenger.RabbitMQ;
using Microsoft.Extensions.Hosting;

namespace MDA.Restaraunt.Notification
{
    public class Worker : BackgroundService
    {
        private readonly Consumer _consumer;

        public Worker()
        {
            _consumer = new Consumer();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Receive((sender, args) =>
            {
                var body = args.Body.ToArray();
                var msg = Encoding.UTF8.GetString(body);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(msg);
                Console.ResetColor();
            });
        }
    }
}
