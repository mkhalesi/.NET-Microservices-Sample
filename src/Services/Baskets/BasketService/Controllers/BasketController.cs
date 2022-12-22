using Microsoft.AspNetCore.Mvc;
using System;
using BasketService.Model.DTOs.Basket;
using BasketService.Model.Services.BasketService;
using BasketService.Model.Services.DiscountService;

namespace BasketService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService basketService;
        public BasketController(IBasketService basketService )
        {
            this.basketService = basketService;
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get(string UserId)
        {
            var basket = basketService.GetOrCreateBasketForUser(UserId);
            return Ok(basket);
        }

        /// <summary>
        /// AddItemToBasket
        /// </summary>
        /// <param name="request"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddItemToBasket(AddItemToBasketDto request, string UserId)
        {
            var basket = basketService.GetOrCreateBasketForUser(UserId);
            request.basketId = basket.Id;
            basketService.AddItemToBasket(request);
            var basketData = basketService.GetBasket(UserId);
            return Ok();
        }

        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="ItemId"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult Remove(Guid ItemId)
        {
            basketService.RemoveItemFromBasket(ItemId);
            return Ok();
        }

        /// <summary>
        /// SetQuantity
        /// </summary>
        /// <param name="basketItemId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult SetQuantity(Guid basketItemId, int quantity)
        {
            basketService.SetQuantities(basketItemId, quantity);
            return Ok();
        }

        [HttpPost("CheckoutBasket")]
        public IActionResult CheckoutBasket(CheckoutBasketDTO checkoutBasket,
            [FromServices] IDiscountService discountService)
        {
            var result = basketService.CheckoutBasket(checkoutBasket, discountService);
            if (result.IsSuccess)
                return Ok(result);

            return StatusCode(500, result);
        }

    }
}
