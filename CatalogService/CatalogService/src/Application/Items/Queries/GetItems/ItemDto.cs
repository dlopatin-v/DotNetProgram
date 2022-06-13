using CatalogService.Application.Common.Mappings;
using CatalogService.Domain.Entities;

namespace CatalogService.Application.Items.Queries.GetItems;

public class ItemDto : IMapFrom<Item> //automapper, there is no custom mapping
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Image { get; set; }
    public decimal Price { get; set; }
    public uint Amount { get; set; }
}