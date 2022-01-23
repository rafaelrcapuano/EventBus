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

                            //Configura os receivers endpoints (abstra��o de exchanges e queues) para todos os consumers
                            cfg.ConfigureEndpoints(context);
                        });
                    });

                    //Define o Kebab Case para cria��o dos receiver endpoints.
                    //O prefixo garante que classes com o mesmo nome, mas em microservices diferentes, tenham a sua pr�pria
                    //queue. Dessa forma, ambas receber�o a mensagem.
                    //Ex: "test-consumer-one"
                    services.AddSingleton<IEndpointNameFormatter>(new KebabCaseEndpointNameFormatter("myApp", false));

                    //Host para o bus do MassTransit
                    services.AddMassTransitHostedService();

                    //Host para o job respons�vel por publicar as mensagens
                    services.AddHostedService<Worker>();
                });
    }
}
