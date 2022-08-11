using AutoMapper;
using CatalogService.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Application.Items.Queries.GetItem;
public class GetItemsNamesQuery : IRequest<List<string>>
{
}

public class GetItemsNamesQueryHandler : IRequestHandler<GetItemsNamesQuery, List<string>>
{
    private readonly IApplicationDbContext _context;

    public GetItemsNamesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<string>> Handle(GetItemsNamesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Items
            .AsNoTracking()
            .Select(i => i.Name)
            .ToListAsync(cancellationToken);
    }
}