using System;
using System.Security.Authentication;
using MassTransit;
using MDA.Restaraunt.Notification;
using MDA.Restaraunt.Notification.Consumers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;


CreateHostBuilder(args).Build().Run();



 IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
           
            services.AddMassTransit(x =>
            {
                x.AddConsumer<NotifierTableBookedConsumer>();
                x.AddConsumer<KitchenReadyConsumer>();

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

                    cfg.UseMessageRetry(r =>
                    {
                        r.Exponential(5,
                            TimeSpan.FromSeconds(1),
                            TimeSpan.FromSeconds(100),
                            TimeSpan.FromSeconds(5));
                        r.Ignore<StackOverflowException>();
                        r.Ignore<ArgumentNullException>(x => x.Message.Contains("Consumer"));
                    });


                    cfg.ConfigureEndpoints(context);
                });



            });
            services.AddSingleton<Notifier>();
            services.AddMassTransitHostedService(true);
        });