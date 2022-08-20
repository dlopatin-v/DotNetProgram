using CatalogService.Domain.Entities;
using GraphAPI.Services;
using GraphQL.Types;

namespace GraphAPI.Schema
{
    public class ItemType : ObjectGraphType<Item>
    {
        public ItemType(ICategoryService categories)
        {
            Field(i => i.Id);
            Field(i => i.Name);
            Field(i => i.Description);
            Field<CategoryType>("category",
                resolve: context => context.Source.Category);
        }
    }
}
