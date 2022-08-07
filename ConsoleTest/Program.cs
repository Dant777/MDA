using MDA.Messenger.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

//var producer = new Producer();
//producer.SendToQueue(Encoding.UTF8.GetBytes("test"), "RabbitTestQueue");

CreateHostBuilder(args).Build().Run();



static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args).
    ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<Worker>();
    });