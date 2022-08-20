using AutoMapper;
using AutoMapper.QueryableExtensions;
using CatalogService.Application.Common.Interfaces;
using CatalogService.Application.Items.Queries.GetItems;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Application.Items.Queries.GetItemProperties;
public class GetItemPropertiesQuery : IRequest<ItemDto?>
{
    public int ItemId { get; init; }
}

public class GetItemPropertiesQueryHandler : IRequestHandler<GetItemPropertiesQuery, ItemDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetItemPropertiesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ItemDto?> Handle(GetItemPropertiesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Items
            .AsNoTracking()
            .Where(i => i.Id == request.ItemId)
            .ProjectTo<ItemDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}