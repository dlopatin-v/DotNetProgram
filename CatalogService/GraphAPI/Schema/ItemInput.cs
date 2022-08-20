namespace GraphAPI.Schema
{
    public class ItemInput
    {
        public int? Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Image { get; set; }
        public int? CategoryId { get; set; }
        public decimal Price { get; set; }
        public uint Amount { get; set; }
    }
}
