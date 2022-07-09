using CartService.BLL;
using CartService.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace CartServ.Controllers.V2
{
    /// <summary>
    /// Cart controller V2
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    public class CartController : ControllerBase
    {
        private readonly ManagerCart _managerCart;

        public CartController(ManagerCart managerCart)
        {
            _managerCart = managerCart;
        }
        /// <summary>
        /// Get cart by Id
        /// </summary>
        /// <param name="id">identifier</param>
        /// <returns>cart item</returns>
        /// <response code="200">Cart items have been recieved successfully</response>
        [MapToApiVersion("2.0")]
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCartInfo(Guid id)
        {
            var result = _managerCart.GetItems(id);
            return Ok(result);
        }
        /// <summary>
        /// Add a new item to cart
        /// </summary>
        /// <param name="id">cart id</param>
        /// <param name="item">new item</param>
        /// <returns></returns>
        /// <response code="200">Cart item has been added successfully</response>
        [MapToApiVersion("2.0")]
        [HttpPost]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult AddItemToCart(Guid id, [FromBody] Item item)
        {
            _managerCart.AddItem(id, item);
            return Ok();
        }
        /// <summary>
        /// Delete item from cart
        /// </summary>
        /// <param name="cartId">cart id</param>
        /// <param name="itemId">item id</param>
        /// <returns></returns>
        /// <response code="200">Cart item has been removed successfully</response>
        [MapToApiVersion("2.0")]
        [HttpDelete]
        [Route("{cartId}/{itemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult DeleteItemFromCart(Guid cartId, Guid itemId)
        {
            _managerCart.RemoveItem(cartId, itemId);
            return Ok();
        }
    }
}
