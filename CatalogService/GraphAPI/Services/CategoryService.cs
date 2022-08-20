using CatalogService.Application.Common.Interfaces;
using CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GraphAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IApplicationDbContext context;

        public CategoryService(IApplicationDbContext context)
        {
            this.context=context;
        }
        public async Task<IEnumerable<Category>> GetCategories() => await context.Categories.ToListAsync();
        public async Task<Category?> GetCategoryById(int id) => await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        public async Task<Category> Create(Category category, CancellationToken cancellationToken) {
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync(cancellationToken);
            return category;
        }
        public async Task<Category?> Update(Category category, CancellationToken cancellationToken)
        {
            Category? categoryToUpdate = await context.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);
            if (categoryToUpdate == null) return null;
            categoryToUpdate.Name = category.Name;
            categoryToUpdate.Image = category.Image;
            await context.SaveChangesAsync(cancellationToken);
            return categoryToUpdate;
        }

        public async Task<Category> Delete(int categoryId, CancellationToken cancellationToken)
        {
            Category category = await context.Categories.Include(c => c.Items).FirstAsync(c => c.Id == categoryId);
            context.Items.RemoveRange(category.Items);
            context.Categories.Remove(category);
            await context.SaveChangesAsync(cancellationToken);
            return category;
        }
    }
    public interface ICategoryService
    {
        Task<Category?> GetCategoryById(int id);
        Task<IEnumerable<Category>> GetCategories();
        Task<Category> Create(Category category, CancellationToken cancellationToken);
        Task<Category?> Update(Category category, CancellationToken cancellationToken);
        Task<Category> Delete(int categoryId, CancellationToken cancellationToken);
    }
}
