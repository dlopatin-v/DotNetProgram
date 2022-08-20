using CatalogService.Application.Common.Interfaces;
using CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAPI.Services
{
    public class ItemService : IItemService
    {
        private readonly IApplicationDbContext context;

        public ItemService(IApplicationDbContext context)
        {
            this.context=context;
        }
        public async Task<Item?> GetItemById(int id) => await context.Items.FirstOrDefaultAsync(i => i.Id == id);
        public async Task<IEnumerable<Item>> GetItemsByCategoryId(int id) => await context.Items.Include(i => i.Category).Where(i => i.Category != null && i.Category.Id == id).ToListAsync();

        public async Task<IEnumerable<Item>> GetItems() => await context.Items.ToListAsync();
        public async Task<Item> Create(Item item, int? categoryId, CancellationToken cancellationToken)
        {
            Category? category = categoryId == null? null : await context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
            item.Category = category;
            await context.Items.AddAsync(item);
            await context.SaveChangesAsync(cancellationToken);
            return item;
        }

        public async Task<Item?> Update(Item item, int? categoryId, CancellationToken cancellationToken)
        {
            Category? category = categoryId == null ? null : await context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
            item.Category = category;
            context.Items.Update(item);
            await context.SaveChangesAsync(cancellationToken);
            return item;
        }


        public async Task<Item> Delete(int itemId, CancellationToken cancellationToken)
        {
            Item item = await context.Items.FirstAsync(c => c.Id == itemId);
            context.Items.Remove(item);
            await context.SaveChangesAsync(cancellationToken);
            return item;
        }
    }
    public interface IItemService
    {
        Task<Item?> GetItemById(int id);
        Task<IEnumerable<Item>> GetItemsByCategoryId(int id);
        Task<IEnumerable<Item>> GetItems();
        Task<Item> Create(Item item, int? categoryId, CancellationToken cancellationToken);
        Task<Item?> Update(Item item, int? categoryId, CancellationToken cancellationToken);
        Task<Item> Delete(int itemId, CancellationToken cancellationToken);
    }
}
