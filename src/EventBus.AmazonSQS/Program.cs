using Amazon.SimpleNotificationService;
using Amazon.SQS;
using EventBus.AmazonSQS.Consumers;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventBus.AmazonSQS
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

                        //Este exemplo utiliza a imagem 0.11.2 do container do Amazon LocalStack
                        //docker run --rm -it -p 4566:4566 -p 4571:4571 -e "SERVICES=sqs,sns" localstack/localstack:0.11.2
                        s.UsingAmazonSqs((context, cfg) =>
                        {
                            var serviceUrl = "http://localhost:4566";

                            cfg.Host("sa-east-1", h =>
                            {
                                h.Config(new AmazonSQSConfig { ServiceURL = serviceUrl });
                                h.Config(new AmazonSimpleNotificationServiceConfig { ServiceURL = serviceUrl });
                                h.AccessKey("test");
                                h.SecretKey("test");
                            });

                            cfg.ConfigureEndpoints(context);
                        });
                    });

                    services.AddSingleton<IEndpointNameFormatter>(new KebabCaseEndpointNameFormatter("test", false));
                    services.AddMassTransitHostedService();
                    services.AddHostedService<Worker>();
                });
    }
}