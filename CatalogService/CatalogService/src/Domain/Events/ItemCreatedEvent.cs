namespace CatalogService.Domain.Events;

public class ItemCreatedEvent : BaseEvent
{
    public ItemCreatedEvent(Item item)
    {
        Item = item;
    }

    public Item Item { get; }
}