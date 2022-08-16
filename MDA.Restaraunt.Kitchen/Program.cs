using System.Security.Authentication;
using MassTransit;
using MDA.Restaraunt.Kitchen;
using MDA.Restaraunt.Kitchen.Consumers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

CreateHostBuilder(args).Build().Run();

IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<KitchenTableBookedConsumer>();

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

            services.AddSingleton<Manager>();

            services.AddMassTransitHostedService(true);
        });