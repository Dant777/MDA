//CreateHostBuilder(args).Build().Run();


using MDA.Restaraunt.Notification;

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args).
        ConfigureServices((hostContext, services) =>
        {
            services.AddHostedService<Worker>();
        });