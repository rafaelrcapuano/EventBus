using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EventBus.InMemory.Consumers
{
    public class ConsumerOne : IConsumer<IMessage>
    {
        private readonly ILogger<ConsumerOne> _logger;

        public ConsumerOne(ILogger<ConsumerOne> logger)
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
