using CatalogService.Application.Common.Exceptions;
using CatalogService.Application.Common.Interfaces;
using CatalogService.Domain.Entities;
using CatalogService.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Application.Items.Commands.CreateItem;

public record CreateItemCommand : IRequest<int>
{
    public int CategoryId { get; init; }

    public string Name { get; init; } = null!;

    public decimal Price { get; init; }
    public string? Description { get; set; }
    public uint Amount { get; init; }
}

public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == request.CategoryId);

        if (category == null)
        {
            throw new NotFoundException(nameof(Category), request.CategoryId);
        }

        var entity = new Item
        {
            Category = category,
            Name = request.Name,
            Price = request.Price,
            Description = request.Description,
            Amount = request.Amount
        };

        entity.AddDomainEvent(new ItemCreatedEvent(entity));

        _context.Items.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}