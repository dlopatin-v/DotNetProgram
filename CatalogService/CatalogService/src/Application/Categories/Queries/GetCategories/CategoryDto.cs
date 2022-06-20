using CatalogService.Application.Common.Mappings;
using CatalogService.Application.Common.Models;
using CatalogService.Domain.Entities;

namespace CatalogService.Application.Categories.Queries.GetCategories;
public class CategoryDto : LinkResourceBase, IMapFrom<Category> //automapper, there is no custom mapping
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public string? Image { get; set; }
}
