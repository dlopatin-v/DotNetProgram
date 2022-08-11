using AutoMapper;
using CatalogService.Application.Common.Interfaces;
using CatalogService.Application.Common.Mappings;
using MediatR;

namespace CatalogService.Application.Items.Queries.GetItems;

public record GetItemsQuery : IRequest<List<ItemDto>>
{
}

public class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, List<ItemDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetItemsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ItemDto>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Items
            .ProjectToListAsync<ItemDto>(_mapper.ConfigurationProvider); //use automapper directly
    }
}