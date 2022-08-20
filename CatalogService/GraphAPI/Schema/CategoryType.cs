using CatalogService.Domain.Entities;
using GraphAPI.Services;
using GraphQL.Types;

namespace GraphAPI.Schema
{
    public class CategoryType : ObjectGraphType<Category>
    {
        public CategoryType()
        {
            Field(c => c.Id);
            Field(c => c.Name);
        }
    }
}
