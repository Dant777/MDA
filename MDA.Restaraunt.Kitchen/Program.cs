using System.Security.Authentication;
using MassTransit;
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
                    cfg.Host("rattlesnake-01.rmq.cloudamqp.com", 5671, "cswkixly", h =>
                    {
                        h.Username("cswkixly");
                        h.Password("MHvokiAUUQ-30kep2zd6bxRMS8Dy6XSx");
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