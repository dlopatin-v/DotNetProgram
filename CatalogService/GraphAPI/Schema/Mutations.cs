using CatalogService.Domain.Entities;
using GraphAPI.Services;
using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAPI.Schema
{
    public class Mutations : ObjectGraphType<object>
    {
        public Mutations(ICategoryService categories, IItemService items)
        {
            Name = "Mutation";
            FieldAsync<CategoryType>("addCategory", arguments: new QueryArguments(new QueryArgument<NonNullGraphType<CategoryInputType>> { Name = "category" }),
                resolve: async context =>
                {
                    // Define the cancellation token.
                    CancellationTokenSource source = new CancellationTokenSource();
                    CancellationToken cancellationToken = source.Token;
                    var categoryInput = context.GetArgument<CategoryInput>("category");
                    var category = new Category()
                    {
                        Name = categoryInput.Name,
                        Image = categoryInput.Image
                    };
                    return await categories.Create(category, cancellationToken);
                });
            FieldAsync<CategoryType>("updateCategory", arguments: new QueryArguments(new QueryArgument<NonNullGraphType<CategoryInputType>> { Name = "category" }),
                resolve: async context =>
                {
                    // Define the cancellation token.
                    CancellationTokenSource source = new CancellationTokenSource();
                    CancellationToken cancellationToken = source.Token;
                    var categoryInput = context.GetArgument<CategoryInput>("category");
                    var category = new Category()
                    {
                        Id = categoryInput?.Id ?? default,
                        Name = categoryInput.Name,
                        Image = categoryInput.Image
                    };
                    return await categories.Update(category, cancellationToken);
                });
            FieldAsync<ItemType>("addItem", arguments: new QueryArguments(new QueryArgument<NonNullGraphType<ItemInputType>> { Name = "item" }),
                resolve: async context =>
                {
                    // Define the cancellation token.
                    CancellationTokenSource source = new CancellationTokenSource();
                    CancellationToken cancellationToken = source.Token;
                    var itemInput = context.GetArgument<ItemInput>("item");
                    var item = new Item()
                    {
                        Name = itemInput.Name,
                        Description = itemInput.Description,
                        Image = itemInput.Image,
                        Price = itemInput.Price,
                        Amount = itemInput.Amount
                    };
                    return await items.Create(item, itemInput.CategoryId, cancellationToken);
                });

            FieldAsync<ItemType>("updateItem", arguments: new QueryArguments(new QueryArgument<NonNullGraphType<ItemInputType>> { Name = "item" }),
                resolve: async context =>
                {
                    // Define the cancellation token.
                    CancellationTokenSource source = new CancellationTokenSource();
                    CancellationToken cancellationToken = source.Token;
                    var itemInput = context.GetArgument<ItemInput>("item");
                    var item = new Item()
                    {
                        Id = itemInput?.Id ?? default,
                        Name = itemInput.Name,
                        Description = itemInput.Description,
                        Image = itemInput.Image,
                        Price = itemInput.Price,
                        Amount = itemInput.Amount
                    };
                    return await items.Update(item, itemInput.CategoryId, cancellationToken);
                });
            FieldAsync<ItemType>("deleteItem", arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "itemId" }),
                resolve: async context =>
                {
                    // Define the cancellation token.
                    CancellationTokenSource source = new CancellationTokenSource();
                    CancellationToken cancellationToken = source.Token;
                    var itemId = context.GetArgument<int>("itemId");
                    return await items.Delete(itemId, cancellationToken);
                });
            FieldAsync<CategoryType>("deleteCategory", arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "categoryId" }),
                resolve: async context =>
                {
                    // Define the cancellation token.
                    CancellationTokenSource source = new CancellationTokenSource();
                    CancellationToken cancellationToken = source.Token;
                    var categoryId = context.GetArgument<int>("categoryId");
                    return await categories.Delete(categoryId, cancellationToken);
                });

        }
    }
}
