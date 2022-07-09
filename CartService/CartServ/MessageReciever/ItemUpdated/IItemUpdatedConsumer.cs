namespace CartServ.MessageReciever.ItemUpdated;

public interface IItemUpdatedConsumer
{
    Task RegisterOnMessageHandlerAndReceiveMessages();
    Task CloseQueueAsync();
    ValueTask DisposeAsync();
}
