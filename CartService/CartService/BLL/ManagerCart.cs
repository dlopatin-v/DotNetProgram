using CartService.DAL;
using CartService.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.BLL
{
    public class ManagerCart
    {
        private readonly GatewayCart _gatewayCart;
        public ManagerCart()
        {
            _gatewayCart = new GatewayCart();
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
            return _gatewayCart.GetItems(cartId);
        }
    }
}
