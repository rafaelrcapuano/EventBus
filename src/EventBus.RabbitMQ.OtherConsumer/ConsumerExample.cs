using MassTransit;
using System;
using System.Threading.Tasks;

namespace EventBus.RabbitMQ.OtherConsumer
{
    public class ConsumerExample : IConsumer<IMessage>
    {
        public Task Consume(ConsumeContext<IMessage> context)
        {
            Console.WriteLine("Mensagem recebida: {0}", context.Message.Text);
            return Task.CompletedTask;
        }
    }
}