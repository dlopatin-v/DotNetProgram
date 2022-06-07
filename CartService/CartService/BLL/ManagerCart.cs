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
        public void AddItem(int cartId, Item item)
        {
            _gatewayCart.AddItem(cartId, item);
        }
        public void RemoveItem(int cartId, int itemId)
        {
            _gatewayCart.RemoveItem(cartId, itemId);
        }
        public List<Item> GetItems(int cartId)
        {
            return _gatewayCart.GetItems(cartId).ToList();
        }
    }
}
