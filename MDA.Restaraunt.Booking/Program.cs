using MassTransit;
using MDA.Restaraunt.Booking.Consumers;
using MDA.Restaraunt.Booking.Entities;
using MDA.Restaraunt.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Authentication;
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

                        x.AddDelayedMessageScheduler();

                        //x.UsingRabbitMq((context, cfg) =>
                        //{
                        //    cfg.Host("rattlesnake-01.rmq.cloudamqp.com", 5671, "alrbgpxt", h =>
                        //    {
                        //        h.Username("alrbgpxt");
                        //        h.Password("HurWX2E_jcjs3hhBjnFCZYwGQnB-689P");
                        //        h.UseSsl(s =>
                        //        {
                        //            s.Protocol = SslProtocols.Tls12;
                        //        });
                        //    });

                        //    cfg.UseDelayedMessageScheduler();
                        //    cfg.UseInMemoryOutbox();
                        //    cfg.ConfigureEndpoints(context);
                        //});
                        x.UsingInMemory((context, cfg) =>
                        {

                            cfg.UseDelayedMessageScheduler();
                            cfg.UseInMemoryOutbox();
                            cfg.ConfigureEndpoints(context);
                        });
                    });

                    services.AddTransient<RestaurantBooking>();
                    services.AddTransient<RestaurantBookingSaga>();
                    services.AddTransient<Restaurant>();

                    services.AddHostedService<Worker>();
                });
    }
}
