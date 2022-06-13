namespace CatalogService.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = null!;

    public string? Image { get; set; } 

    public Category? Parent { get; set; }

    public IList<Item> Items { get; private set; } = new List<Item>();
}