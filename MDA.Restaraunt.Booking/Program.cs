using System.Security.Authentication;
using MassTransit;
using MDA.Restaraunt.Booking.Consumers;
using MDA.Restaraunt.Booking.Entities;
using MDA.Restaraunt.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using MassTransit.Audit;
using MDA.Restaraunt.Messages.Repository;
using MDA.Restaraunt.Booking.Schedules;
using MDA.Restaraunt.Messages.DbData;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
