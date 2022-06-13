using CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CatalogService.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default data
        // Seed, if necessary
        if (!_context.Categories.Any())
        {
            _context.Categories.Add(new Category
            {
                Name = "Toys",
                Items =
                {
                    new Item { Name = "Rubber duck", Amount = 1, Description = "Made from rubber", Price = 1M },
                    new Item { Name = "LOL", Amount = 1, Description = "Ugly doll", Price = 4M },
                    new Item { Name = "Marvel team", Amount = 5, Description = "Brave guys", Price = 3M },
                    new Item { Name = "Lego", Amount = 1, Description = "Think before", Price = 6M },
                }
            });

            await _context.SaveChangesAsync();
        }
    }
}