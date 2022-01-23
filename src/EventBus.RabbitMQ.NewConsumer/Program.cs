using MassTransit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventBus.RabbitMQ.NewConsumer
{
    static class Program
    {
        public static async Task Main()
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost", settings =>
                {
                    settings.Username("guest");
                    settings.Password("guest");
                });

                //Configura um receiver endpoint associando-o ao consumer
                cfg.ReceiveEndpoint("consumer-example", e =>
                {
                    e.Consumer<ConsumerExample>();
                });
            });

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);
            try
            {
                Console.WriteLine("Pressione enter para sair");

                await Task.Run(() => Console.ReadLine());
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }
}
