using CartService.DAL.Models;

namespace CartService.DAL
{
    public interface IGatewayCart
    {
        void AddItem(Guid cartId, Item item);
        void RemoveItem(Guid cartId, Guid itemId);
        IList<Item> GetItems(Guid cartId);
    }
}
