using AutoMapper;
using CatalogService.Application.Common.Interfaces;
using CatalogService.Application.Common.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Application.Categories.Queries.GetCategories;
public record GetCategoriesQuery : IRequest<List<CategoryDto>>
{
}

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Categories
            .Include(c => c.Items)
            .ProjectToListAsync<CategoryDto>(_mapper.ConfigurationProvider); //use automapper directly
    }
}