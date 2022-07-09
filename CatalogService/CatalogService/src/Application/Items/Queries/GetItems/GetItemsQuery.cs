using AutoMapper;
using AutoMapper.QueryableExtensions;
using CatalogService.Application.Common.Interfaces;
using CatalogService.Application.Common.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Application.Items.Queries.GetItems;

public record GetItemsQuery : IRequest<List<ItemDto>>
{
    public int CategoryId { get; init; }
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
        return await _context.Categories
            .Where(c => c.Id == request.CategoryId)
            .SelectMany(c => c.Items)
            .ProjectToListAsync<ItemDto>(_mapper.ConfigurationProvider); //use automapper directly
    }
}