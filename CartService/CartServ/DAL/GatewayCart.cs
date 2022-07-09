using CartService.DAL.Models;
using LiteDB;

namespace CartService.DAL
{
    public class GatewayCart :IGatewayCart
    {
        const string dbName = "Cart.db";
        static GatewayCart()
        {
            var mapper = BsonMapper.Global;
            mapper.Entity<Cart>()
                .DbRef(x => x.Items, "items");
        }
        public void AddItem(Guid cartId, Item item)
        {
            using (var db = new LiteDatabase(dbName))
            {
                var carts = db.GetCollection<Cart>("carts");
                var items = db.GetCollection<Item>("items");
                var cart = carts
                    .Include(x => x.Items)
                    .FindById(cartId);
                if (cart is null)
                {
                    cart = new Cart() { Id = cartId };
                    carts.Insert(cart);
                }
                items.Insert(item);
                cart.Items.Add(item);
                carts.Update(cart);
                var test = carts
                    .Include(x => x.Items)
                    .FindById(cartId);
            }
        }
        public void RemoveItem(Guid cartId, Guid itemId)
        {
            using (var db = new LiteDatabase(dbName))
            {
                var carts = db.GetCollection<Cart>("carts");
                var cart = carts
                    .Include(x => x.Items)
                    .FindById(cartId);
                Item? item = cart.Items.FirstOrDefault(x => x.Id == itemId);
                if (item is null) return;
                cart.Items.Remove(item);
                carts.Update(cart);
            }
        }
        public IList<Item> GetItems(Guid cartId)
        {
            using (var db = new LiteDatabase(dbName))
            {
                var carts = db.GetCollection<Cart>("carts");
                var cart = carts
                    .Include(x => x.Items)
                    .FindById(cartId);
                return cart.Items;
            }
        }
    }
}
