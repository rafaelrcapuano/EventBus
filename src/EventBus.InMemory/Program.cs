using EventBus.InMemory.Consumers;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventBus.InMemory
{
    static class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(s =>
                    {
                        s.AddConsumer<ConsumerOne>();
                        s.AddConsumer<ConsumerTwo>();

                        s.UsingInMemory((context, cfg) =>
                        {
                            cfg.ConfigureEndpoints(context);
                        });
                    });

                    services.AddMassTransitHostedService(true);
                    services.AddHostedService<Worker>();
                });
    }
}