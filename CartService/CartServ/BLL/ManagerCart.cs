using CartService.DAL;
using CartService.DAL.Models;

namespace CartService.BLL
{
    public class ManagerCart
    {
        private readonly IGatewayCart _gatewayCart;
        public ManagerCart(IGatewayCart gatewayCart)
        {
            _gatewayCart = gatewayCart;
        }
        public void AddItem(Guid cartId, Item item)
        {
            _gatewayCart.AddItem(cartId, item);
        }
        public void RemoveItem(Guid cartId, Guid itemId)
        {
            _gatewayCart.RemoveItem(cartId, itemId);
        }
        public List<Item> GetItems(Guid cartId)
        {
            return _gatewayCart.GetItems(cartId).ToList();
        }
    }
}
