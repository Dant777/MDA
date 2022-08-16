using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MDA.Restaraunt.Booking.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MDA.Restaraunt.Booking
{
    public static class Program
    {
        public static void Main(string[] args)
        {

            Console.OutputEncoding = System.Text.Encoding.UTF8;
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(x =>
                    {
                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host("rattlesnake-01.rmq.cloudamqp.com", 5671, "alrbgpxt", h =>
                            {
                                h.Username("alrbgpxt");
                                h.Password("HurWX2E_jcjs3hhBjnFCZYwGQnB-689P");
                                h.UseSsl(s =>
                                {
                                    s.Protocol = SslProtocols.Tls12;
                                });
                            });
                            cfg.ConfigureEndpoints(context);
                        });
                    });
                    services.AddMassTransitHostedService(true);

                    services.AddTransient<Entities.Restaurant>();

                    services.AddHostedService<Worker>();
                });
    }
}
