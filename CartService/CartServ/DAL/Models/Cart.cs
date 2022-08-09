using LiteDB;

namespace CartService.DAL.Models
{
    [BsonRef("carts")]
    public class Cart
    {
        public Cart()
        {
            Items = new List<Item>();
        }
        [BsonId]
        public int Id { get; set; }
        public List<Item> Items { get; set; }
    }
}
