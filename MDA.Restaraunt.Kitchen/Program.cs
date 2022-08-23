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
                x.AddConsumer<KitchenTableBookedConsumer>()
                    .Endpoint(e =>
                    {
                        e.Temporary = true;
                    }); 
                x.AddConsumer<KitchenBookingRequestFaultConsumer>()
                    .Endpoint(e =>
                    {
                        e.Temporary = true;
                    });
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

                //x.UsingInMemory((context, cfg) =>
                //{

                //    cfg.UseDelayedMessageScheduler();
                //    cfg.UseInMemoryOutbox();
                //    cfg.ConfigureEndpoints(context);
                //});
            });

            services.AddSingleton<Manager>();

            services.AddMassTransitHostedService(true);
        });