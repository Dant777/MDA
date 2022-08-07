using System.Text;
using MDA.Messenger.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

//var producer = new Producer();
//producer.SendToQueue(Encoding.UTF8.GetBytes("test"), "RabbitTestQueue");

var msg = Encoding.UTF8.GetBytes("message");
Console.WriteLine(msg.Length);
msg = Encoding.UTF8.GetBytes("");
Console.WriteLine(msg.Length);
//CreateHostBuilder(args).Build().Run();



static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args).
    ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<Worker>();
    });