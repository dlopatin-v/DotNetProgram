using System.Text.Json;
using Azure.Messaging.ServiceBus;
using CatalogService.Application.Common.Interfaces;

namespace CatalogService.Infrastructure.MessageSender;
public class MessageSender : IMessageSender
{
    private readonly string connectionString = "Endpoint=sb://cartservice-ns.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=qeZHoxIplD9PtNXOlNRc8iFpQDm8G7D08/joCOKtswQ=";
    private readonly string queueName = "itemupdated";

    // the client that owns the connection and can be used to create senders and receivers
    private ServiceBusClient? client;

    // the sender used to publish messages to the queue
    private ServiceBusSender? sender;
    public async Task SendAsync<T>(T message)
    {
        client = new ServiceBusClient(connectionString);
        sender = client.CreateSender(queueName);

        using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();
        string jsonString = JsonSerializer.Serialize(message);
        if (!messageBatch.TryAddMessage(new ServiceBusMessage(jsonString)))
        {
            throw new Exception($"The message is too large to fit in the batch.");
        }

        try
        {
            await sender.SendMessagesAsync(messageBatch);
            Console.WriteLine($"The message has been published to the queue.");
        }
        finally
        {
            await sender.DisposeAsync();
            await client.DisposeAsync();
        }
    }
}
