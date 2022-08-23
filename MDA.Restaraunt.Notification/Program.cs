using MassTransit;
using MDA.Restaraunt.Notification;
using MDA.Restaraunt.Notification.Consumers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Authentication;


CreateHostBuilder(args).Build().Run();



IHostBuilder CreateHostBuilder(string[] args) =>
   Host.CreateDefaultBuilder(args)
       .ConfigureServices((hostContext, services) =>
       {

           services.AddMassTransit(x =>
           {
               x.AddConsumer<NotifyConsumer>();

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

               //x.UsingInMemory((context, cfg) =>
               //{
               //    cfg.UseMessageRetry(r =>
               //    {
               //        r.Exponential(5,
               //            TimeSpan.FromSeconds(1),
               //            TimeSpan.FromSeconds(100),
               //            TimeSpan.FromSeconds(5));
               //        r.Ignore<StackOverflowException>();
               //        r.Ignore<ArgumentNullException>(x => x.Message.Contains("Consumer"));
               //    });
               //    cfg.UseDelayedMessageScheduler();
               //    cfg.UseInMemoryOutbox();
               //    cfg.ConfigureEndpoints(context);
               //});


           });
           services.AddSingleton<Notifier>();
           services.AddMassTransitHostedService(true);
       });