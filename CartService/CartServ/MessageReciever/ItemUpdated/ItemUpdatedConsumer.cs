using Azure.Messaging.ServiceBus;
using CartServ.BLL;

namespace CartServ.MessageReciever.ItemUpdated;

public class ItemUpdatedConsumer : IItemUpdatedConsumer
{
    private readonly string connectionString = "Endpoint=sb://cartservice-ns.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=qeZHoxIplD9PtNXOlNRc8iFpQDm8G7D08/joCOKtswQ=";
    private readonly string queueName = "itemupdated";
    private readonly ManagerCart _managerCart;
    private readonly IConfiguration _configuration;
    private readonly ServiceBusClient _client;
    private readonly ILogger _logger;
    private ServiceBusProcessor _processor;

    public ItemUpdatedConsumer(
        ManagerCart managerCart,
        IConfiguration configuration,
        ILogger<ItemUpdatedConsumer> logger)
    {
        _managerCart=managerCart;
        _configuration = configuration;
        _logger = logger;
        _client = new ServiceBusClient(connectionString);
    }

    public async Task RegisterOnMessageHandlerAndReceiveMessages()
    {
        ServiceBusProcessorOptions _serviceBusProcessorOptions = new()
        {
            MaxConcurrentCalls = 1,
            AutoCompleteMessages = false,
        };

        _processor = _client.CreateProcessor(queueName, _serviceBusProcessorOptions);
        _processor.ProcessMessageAsync += ProcessMessagesAsync;
        _processor.ProcessErrorAsync += ProcessErrorAsync;
        await _processor.StartProcessingAsync().ConfigureAwait(false);
    }

    private Task ProcessErrorAsync(ProcessErrorEventArgs arg)
    {
        _logger.LogError(arg.Exception, "Message handler encountered an exception");
        _logger.LogDebug($"- ErrorSource: {arg.ErrorSource}");
        _logger.LogDebug($"- Entity Path: {arg.EntityPath}");
        _logger.LogDebug($"- FullyQualifiedNamespace: {arg.FullyQualifiedNamespace}");

        return Task.CompletedTask;
    }

    private async Task ProcessMessagesAsync(ProcessMessageEventArgs args)
    {
        var item = args.Message.Body.ToObjectFromJson<Item>();
        _managerCart.UpdateItem(new CartService.DAL.Models.Item() { Id = item.Id, Name = item.Name, Price = item.Price });
        await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
    }

    public async ValueTask DisposeAsync()
    {
        if (_processor != null)
        {
            await _processor.DisposeAsync().ConfigureAwait(false);
        }

        if (_client != null)
        {
            await _client.DisposeAsync().ConfigureAwait(false);
        }
    }

    public async Task CloseQueueAsync()
    {
        await _processor.CloseAsync().ConfigureAwait(false);
    }
}
