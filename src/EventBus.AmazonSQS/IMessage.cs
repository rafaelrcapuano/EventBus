namespace EventBus.AmazonSQS
{
    public interface IMessage
    {
        string Text { get; }
    }
}