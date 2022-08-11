using CatalogService.Application.Common.Exceptions;
using CatalogService.Application.Common.Models;
using CatalogService.Application.Items.Commands.CreateItem;
using CatalogService.Application.Items.Commands.DeleteItem;
using CatalogService.Application.Items.Commands.UpdateItem;
using CatalogService.Application.Items.Queries.GetItem;
using CatalogService.Application.Items.Queries.GetItemProperties;
using CatalogService.Application.Items.Queries.GetItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
/// <summary>
/// Item controller
/// </summary>
[Route("api/[controller]")]
[Authorize]
[ApiController]
public class ItemController : ApiControllerBase
{
    private readonly ILogger<ItemController> logger;

    public ItemController(ILogger<ItemController> logger)
    {
        this.logger=logger;
    }
    /// <summary>
    /// Get item properties
    /// </summary>
    /// <param name="query">item name</param>
    /// <returns>Item properties</returns>
    /// <response code="200">Returns item properties successfully</response>
    [Authorize(Roles = "Buyer")]
    [HttpGet("properties")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ItemDto?>> GetItemProperties([FromQuery] GetItemPropertiesQuery query)
    {
        return await Mediator.Send(query);
    }
    /// <summary>
    /// Get item names
    /// </summary>
    /// <returns>Item</returns>
    /// <response code="200">Returns item successfully</response>
    [Authorize(Roles = "Buyer")]
    [HttpGet("names")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<string>>> GetItemsNames()
    {
        return await Mediator.Send(new GetItemsNamesQuery());
    }
    /// <summary>
    /// Get list of items
    /// </summary>
    /// <returns>List of items</returns>
    /// <response code="200">Returns items successfully</response>
    [Authorize(Roles = "Buyer")]
    [HttpGet("list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ItemDto>>> GetItems()
    {
        return await Mediator.Send(new GetItemsQuery());
    }
    /// <summary>
    /// Get paginated list of items
    /// </summary>
    /// <param name="query">category id with paging info</param>
    /// <returns>paginated list of items</returns>
    /// <response code="200">Returns items successfully</response>
    [Authorize(Roles = "Manager")]
    [HttpGet("paginated")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedList<ItemDto>>> GetTodoItemsWithPagination([FromQuery] GetItemsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }
    /// <summary>
    /// Add a new item
    /// </summary>
    /// <param name="command">new item</param>
    /// <returns>item count</returns>
    /// <response code="200">Item has been created successfully</response>
    [Authorize(Roles = "Manager")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> Create(CreateItemCommand command)
    {
        int result = 0;
        try
        {
            result = await Mediator.Send(command);
        }
        catch (ValidationException ex)
        {
            ex.Errors.Values.ToList().ForEach(v => v.ToList().ForEach(ie => logger.LogError(ie)));
        }
        return result;
    }
    /// <summary>
    /// Update item
    /// </summary>
    /// <param name="id">item id</param>
    /// <param name="command">updated fields</param>
    /// <returns></returns>
    /// <response code="204">Item has been updated successfully</response>
    /// <response code="400">Item has not been updated successfully</response>
    [Authorize(Roles = "Manager")]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Update(int id, UpdateItemCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }
    /// <summary>
    /// Delete item
    /// </summary>
    /// <param name="id">item id</param>
    /// <returns></returns>
    /// <response code="204">Item has been deleted successfully</response>
    [Authorize(Roles = "Manager")]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteItemCommand(id));

        return NoContent();
    }
}
