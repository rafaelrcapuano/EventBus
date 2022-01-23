using EventBus.RabbitMQ.Consumers;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventBus.RabbitMQ
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

                        s.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host("localhost", settings =>
                            {
                                settings.Username("guest");
                                settings.Password("guest");
                            });

                            //Configura os receivers endpoints (abstração de exchanges e queues) para todos os consumers
                            cfg.ConfigureEndpoints(context);
                        });
                    });

                    //Define o Kebab Case para criação dos receiver endpoints.
                    //O prefixo garante que classes com o mesmo nome, mas em microservices diferentes, tenham a sua própria
                    //queue. Dessa forma, ambas receberão a mensagem.
                    //Ex: "test-consumer-one"
                    services.AddSingleton<IEndpointNameFormatter>(new KebabCaseEndpointNameFormatter("myApp", false));

                    //Host para o bus do MassTransit
                    services.AddMassTransitHostedService();

                    //Host para o job responsável por publicar as mensagens
                    services.AddHostedService<Worker>();
                });
    }
}
