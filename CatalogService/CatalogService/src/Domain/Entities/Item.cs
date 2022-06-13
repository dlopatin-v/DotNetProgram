namespace CatalogService.Domain.Entities;

public class Item : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Image { get; set; }
    public Category Category { get; set; } = null!;
    public decimal Price { get; set; }
    public uint Amount { get; set; }
}