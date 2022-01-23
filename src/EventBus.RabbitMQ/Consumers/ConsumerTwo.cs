using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EventBus.RabbitMQ.Consumers
{
    public class ConsumerTwo : IConsumer<IMessage>
    {
        private readonly ILogger<ConsumerTwo> _logger;

        public ConsumerTwo(ILogger<ConsumerTwo> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<IMessage> context)
        {
            _logger.LogInformation("Mensagem recebida: {Text}", context.Message.Text);
            return Task.CompletedTask;
        }
    }
}
