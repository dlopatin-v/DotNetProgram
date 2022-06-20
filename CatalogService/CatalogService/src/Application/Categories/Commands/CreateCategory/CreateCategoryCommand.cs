using CatalogService.Application.Common.Exceptions;
using CatalogService.Application.Common.Interfaces;
using CatalogService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Application.Categories.Commands.CreateCategory;
public class CreateCategoryCommand : IRequest<int>
{

    public string Name { get; set; } = null!;

    public string? Image { get; set; }

    public int? ParentId { get; set; }
}

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {


        var entity = new Category
        {
            Name = request.Name,
            Image = request.Image
        };
        if (request.ParentId is not null)
        {
            var categoryParent = await _context.Categories.FirstOrDefaultAsync(c => c.Id == request.ParentId);
            if (categoryParent is null)
            {
                throw new NotFoundException(nameof(Category), request.ParentId);
            }
            entity.Parent = categoryParent;
        }

        _context.Categories.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
