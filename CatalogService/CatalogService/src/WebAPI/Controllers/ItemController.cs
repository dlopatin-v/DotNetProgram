using CatalogService.Application.Common.Exceptions;
using CatalogService.Application.Items.Commands.CreateItem;
using CatalogService.Application.Items.Commands.DeleteItem;
using CatalogService.Application.Items.Commands.UpdateItem;
using CatalogService.Application.Items.Queries.GetItems;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ApiControllerBase
    {
        private readonly ILogger<ItemController> logger;

        public ItemController(ILogger<ItemController> logger)
        {
            this.logger=logger;
        }
        [HttpGet]
        public async Task<ActionResult<List<ItemDto>>> GetItems([FromQuery] GetItemsQuery query)
        {
            return await Mediator.Send(query);
        }
        [HttpPost]
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

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateItemCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteItemCommand(id));

            return NoContent();
        }
    }
}
