using System.Security.Authentication;
using MassTransit;
using MassTransit.Audit;
using MDA.Restaraunt.Booking.Consumers;
using MDA.Restaraunt.Booking.Entities;
using MDA.Restaraunt.Booking.Schedules;
using MDA.Restaraunt.Messages;
using MDA.Restaraunt.Messages.DbData;
using MDA.Restaraunt.Messages.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Prometheus;

namespace MDA.Restaraunt.Booking
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<AppDbContext>();
            services.AddSingleton<IMessageAuditStore, AuditStore>();

            var serviceProvider = services.BuildServiceProvider();
            var auditStore = serviceProvider.GetService<IMessageAuditStore>();
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
                    cfg.UsePrometheusMetrics(serviceName: "booking_service");
                    cfg.UseMessageScheduler(schedulerEndpoint);

                    cfg.UseInMemoryOutbox();
                    cfg.ConfigureEndpoints(context);

                    cfg.ConnectSendAuditObservers(auditStore);
                    cfg.ConnectConsumeAuditObserver(auditStore);
                });

            });

            services.AddTransient<RestaurantBooking>();
            services.AddTransient<RestaurantBookingSaga>();
            services.AddTransient<Restaurant>();
            services.AddTransient<BookingExpire>();
            services.AddTransient<DbDeleteSchedule>();
            services.AddSingleton<IBookingRequestRepository, BookingRequestRepository>();
            services.AddHostedService<Worker>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapMetrics();
                endpoints.MapControllers();
            });
        }
    }
}
