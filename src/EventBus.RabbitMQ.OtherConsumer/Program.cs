using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventBus.RabbitMQ.OtherConsumer
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
                        s.AddConsumer<ConsumerExample>();

                        s.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host("localhost", settings =>
                            {
                                settings.Username("guest");
                                settings.Password("guest");
                            });

                            cfg.ConfigureEndpoints(context);
                        });
                    });

                    services.AddSingleton<IEndpointNameFormatter>(new KebabCaseEndpointNameFormatter("test", false));
                    services.AddMassTransitHostedService();
                });
    }
}
