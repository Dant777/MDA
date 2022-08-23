using System.Security.Authentication;
using MassTransit;
using MDA.Restaraunt.Booking.Consumers;
using MDA.Restaraunt.Booking.Entities;
using MDA.Restaraunt.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;

namespace MDA.Restaraunt.Booking
{
    public static class Program
    {
        public static void Main(string[] args)
        {

            Console.OutputEncoding = Encoding.UTF8;
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<BookingRequestConsumer>()
                            .Endpoint(e =>
                            {
                                e.Temporary = true;
                            });

                        x.AddConsumer<BookingRequestFaultConsumer>()
                            .Endpoint(e =>
                            {
                                e.Temporary = true;
                            });

                        x.AddSagaStateMachine<RestaurantBookingSaga, RestaurantBooking>()
                            .Endpoint(e => e.Temporary = true)
                            .InMemoryRepository();
                        Uri schedulerEndpoint = new Uri("queue:scheduler");
                        x.AddMessageScheduler(schedulerEndpoint);

                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host("rattlesnake-01.rmq.cloudamqp.com", 5671, "zynnruxw", h =>
                            {
                                h.Username("zynnruxw");
                                h.Password("U-4FGwT3LH9rsTJdfTPlmDaZPX69bJbC");
                                h.UseSsl(s =>
                                {
                                    s.Protocol = SslProtocols.Tls12;
                                });
                            });

                            cfg.UseMessageScheduler(schedulerEndpoint);
                            cfg.UseInMemoryOutbox();
                            cfg.ConfigureEndpoints(context);
                        });

                    });

                    services.AddTransient<RestaurantBooking>();
                    services.AddTransient<RestaurantBookingSaga>();
                    services.AddTransient<Restaurant>();
                    services.AddTransient<BookingExpire>();

                    services.AddHostedService<Worker>();
                });
    }
}
