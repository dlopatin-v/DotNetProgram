using GraphAPI.Services;
using GraphQL.Types;

namespace GraphAPI.Schema
{
    public class Queries : ObjectGraphType<object>
    {
        public Queries(ICategoryService categories, IItemService items)
        {
            Name = "Query";
            FieldAsync<ListGraphType<CategoryType>>(
                "categories",
                resolve: async context => await categories.GetCategories());
            FieldAsync<CategoryType>(
                "category",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "categoryId" }),
                resolve: async context => 
                {
                    var categoryId = context.Arguments["categoryId"].Value as int?;
                    if (categoryId == null) return null;
                    return await categories.GetCategoryById((int)categoryId);
                });
            FieldAsync<ListGraphType<ItemType>>(
                "items",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "categoryId" }),
                resolve: async context =>
                {
                    var categoryId = context.Arguments["categoryId"].Value as int?;
                    if (categoryId == null) return null;
                    return await items.GetItemsByCategoryId((int)categoryId);
                });
        }
    }
}
