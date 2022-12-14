using MassTransit;
using MassTransit.Audit;
using MDA.Restaraunt.Kitchen;
using MDA.Restaraunt.Kitchen.Consumers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Authentication;

CreateHostBuilder(args).Build().Run();

IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            services.AddSingleton<IMessageAuditStore, AuditStore>();

            var serviceProvider = services.BuildServiceProvider();
            var auditStore = serviceProvider.GetService<IMessageAuditStore>();
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
                    cfg.UsePrometheusMetrics(serviceName: "booking_service");
                    cfg.UseMessageScheduler(schedulerEndpoint);

                    cfg.UseInMemoryOutbox();
                    cfg.ConfigureEndpoints(context);

                    cfg.ConnectSendAuditObservers(auditStore);
                    cfg.ConnectConsumeAuditObserver(auditStore);
                });

            });

            services.AddSingleton<Manager>();
        });