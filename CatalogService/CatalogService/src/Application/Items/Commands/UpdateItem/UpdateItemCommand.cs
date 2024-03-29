﻿using CatalogService.Application.Common.Exceptions;
using CatalogService.Application.Common.Interfaces;
using CatalogService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Application.Items.Commands.UpdateItem;

public record UpdateItemCommand : IRequest
{
    public int Id { get; init; }
    public int? CategoryId { get; init; }

    public string? Name { get; init; }

    public decimal? Price { get; init; }
    public string? Description { get; set; }
    public uint? Amount { get; init; }
}

public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IMessageSender _sender;

    public UpdateItemCommandHandler(IApplicationDbContext context, IMessageSender sender)
    {
        _context = context;
        _sender = sender;
    }

    public async Task<Unit> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Items
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Item), request.Id);
        }

        entity.Name = request.Name ?? entity.Name;
        entity.Description = request.Description;
        entity.Amount = request.Amount ?? entity.Amount;
        entity.Price = request.Price ?? entity.Price;
        if (request.CategoryId is not null)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == request.CategoryId);

            if (category == null)
            {
                throw new NotFoundException(nameof(Category), request.CategoryId);
            }

            entity.Category = category;
        }

        await _context.SaveChangesAsync(cancellationToken);
        await _sender.SendAsync(new { entity.Name, entity.Price, entity.Description, entity.Amount});
        return Unit.Value;
    }
}