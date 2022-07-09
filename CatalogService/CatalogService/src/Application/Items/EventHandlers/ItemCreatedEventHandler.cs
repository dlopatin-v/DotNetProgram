using CatalogService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CatalogService.Application.Items.EventHandlers;

public class ItemCreatedEventHandler : INotificationHandler<ItemCreatedEvent>
{
    private readonly ILogger<ItemCreatedEventHandler> _logger;

    public ItemCreatedEventHandler(ILogger<ItemCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ItemCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("CatalogService Domain Event: {DomainEvent}", notification.GetType().Name);
        //do smth else. this is an example how to use domain events
        return Task.CompletedTask;
    }
}