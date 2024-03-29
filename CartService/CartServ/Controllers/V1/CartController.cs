﻿using CartServ.BLL;
using CartService.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace CartServ.Controllers.V1
{
    /// <summary>
    /// Cart controller V1
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
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
        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCartInfo(int id)
        {
            var result = _managerCart.GetItems(id);
            return Ok(new { id, result });
        }
        /// <summary>
        /// Add a new item to cart
        /// </summary>
        /// <param name="id">cart id</param>
        /// <param name="item">new item</param>
        /// <returns></returns>
        /// <response code="200">Cart item has been added successfully</response>
        [MapToApiVersion("1.0")]
        [HttpPost]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult AddItemToCart(int id, [FromBody] Item item)
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
        [MapToApiVersion("1.0")]
        [HttpDelete]
        [Route("{cartId}/{itemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult DeleteItemFromCart(int cartId, int itemId)
        {
            _managerCart.RemoveItem(cartId, itemId);
            return Ok();
        }
    }
}
