using CartService.DAL;
using CartService.DAL.Models;

namespace CartServ.BLL;

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
    public void UpdateItem(Item item)
    {
        _gatewayCart.UpdateItem(item);
    }
}
