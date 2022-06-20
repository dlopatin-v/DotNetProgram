using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatalogService.Application.Common.Exceptions;
using CatalogService.Application.Common.Interfaces;
using CatalogService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Application.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand : IRequest
{
    public int Id { get; init; }

    public string? Name { get; init; }
    public string? Image { get; init; }
    public int? ParentId { get; init; }
}

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Categories
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Category), request.Id);
        }

        entity.Name = request.Name ?? entity.Name;
        entity.Image = request.Image ?? entity.Image;
        if (request.ParentId is not null)
        {
            var categoryParent = await _context.Categories.FirstOrDefaultAsync(c => c.Id == request.ParentId);

            if (categoryParent is null)
            {
                throw new NotFoundException(nameof(Category), request.ParentId);
            }

            entity.Parent = categoryParent;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}