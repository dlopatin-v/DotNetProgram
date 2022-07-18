using CartService.DAL.Models;

namespace CartService.DAL
{
    public interface IGatewayCart
    {
        void AddItem(int cartId, Item item);
        void UpdateItem(Item newItem);
        void RemoveItem(int cartId, int itemId);
        IList<Item> GetItems(int cartId);
    }
}
