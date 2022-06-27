using LiteDB;

namespace CartService.DAL.Models
{
    [BsonRef("items")]
    public class Item
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Image { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
