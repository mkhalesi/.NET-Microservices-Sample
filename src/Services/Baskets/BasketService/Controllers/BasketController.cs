using Microsoft.AspNetCore.Mvc;
using System;
using BasketService.Model.DTOs.Basket;
using BasketService.Model.Services.BasketService;
using BasketService.Model.Services.DiscountService;
using Microsoft.AspNetCore.Authorization;

namespace BasketService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService basketService;
        public BasketController(IBasketService basketService)
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
        public IActionResult AddItemToBasket(AddItemToBasketDto request, string userId)
        {
            var basket = basketService.GetOrCreateBasketForUser(userId);
            request.basketId = basket.Id;
            basketService.AddItemToBasket(request);
            //var basketData = basketService.GetBasket(userId);
            return Ok();
        }

        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="ItemId"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult Delete(Guid ItemId, string userId)
        {
            basketService.RemoveItemFromBasket(ItemId.ToString(), userId);
            return Ok();
        }

        /// <summary>
        /// SetQuantity
        /// </summary>
        /// <param name="basketItemId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult SetQuantity(Guid basketItemId, int quantity, string userId)
        {
            basketService.SetQuantities(basketItemId.ToString(), quantity,userId);
            return Ok();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="basketId"></param>
        /// <param name="discountId"></param>
        /// <returns></returns>
        [HttpPut("{basketId}/{discountId}")]
        public IActionResult ApplyDiscountToBasket(Guid basketId, Guid discountId, string userId)
        {
            basketService.ApplyDiscountToBasket(basketId.ToString(), discountId.ToString(), userId);
            return Accepted();
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
