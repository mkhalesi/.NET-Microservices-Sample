using System;
using System.Threading.Tasks;
using Microservices.Web.Frontend.Models.DTO;
using Microservices.Web.Frontend.Models.DTO.Basket;
using Microservices.Web.Frontend.Services.BasketServices;
using Microservices.Web.Frontend.Services.DiscountService;
using Microservices.Web.Frontend.Services.ProductServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Web.Frontend.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {
        #region constructor

        private readonly IBasketService basketService;
        private readonly IProductService productService;
        private readonly IDiscountService discountService;
        private const string UserId = "1";
        public BasketController(IBasketService basketService, IProductService productService, IDiscountService discountService)
        {
            this.basketService = basketService;
            this.productService = productService;
            this.discountService = discountService;
        }

        #endregion

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var basket = await basketService.GetBasket(UserId);

            if (basket != null && basket.DiscountId.HasValue)
            {
                var discount = discountService.GetDiscountById(basket.DiscountId.Value);
                basket.DiscountDetail = new DiscountInBasketDTO()
                {
                    Amount = discount.Data.Amount,
                    DiscountCode = discount.Data.Code,
                };
            }

            return View(basket);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid Id)
        {
            basketService.DeleteFromBasket(Id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddToBasket(Guid ProductId)
        {
            var product = productService.GetProduct(ProductId);
            var basket = await basketService.GetBasket(UserId);

            AddToBasketDTO item = new AddToBasketDTO()
            {
                BasketId = basket.Id,
                ImageUrl = product.image.ToString(),
                ProductId = product.id,
                ProductName = product.name,
                Quantity = 1,
                UnitPrice = product.price,
            };
            await basketService.AddToBasket(item, UserId);
            return RedirectToAction("Index");
        }

        [HttpPut]
        public async Task<IActionResult> Edit(Guid BasketItemId, int quantity)
        {
            await basketService.UpdateQuantity(BasketItemId, quantity);
            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<IActionResult> ApplyDiscount(string DiscountCode)
        {
            if (string.IsNullOrWhiteSpace(DiscountCode))
            {
                return Json(new ResultDTO()
                {
                    IsSuccess = false,
                    Message = "Please enter the discount code"
                });
            }
            var discount = discountService.GetDiscountByCode(DiscountCode);
            if (discount.IsSuccess == true)
            {
                if (discount.Data.Used)
                {
                    return Json(new ResultDTO
                    {
                        IsSuccess = false,
                        Message = "This discount code has already been used"
                    });
                }

                var basket = await basketService.GetBasket(UserId);
                //basketService.ApplyDiscountToBasket(Guid.Parse(basket.Id), discount.Data.Id);
                await basketService.ApplyDiscountToBasket(basket.Id, discount.Data.Id);
                discountService.UseDiscount(discount.Data.Id);
                return Json(new ResultDTO
                {
                    IsSuccess = true,
                    Message = "The discount code has been successfully applied to your shopping cart",
                });
            }
            else
            {
                return Json(new ResultDTO
                {
                    IsSuccess = false,
                    Message = discount.Message,
                });
            }
        }

        #region checkout

        public async Task<IActionResult> Checkout()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutDTO checkout)
        {
            checkout.UserId = UserId;
            checkout.BasketId = basketService.GetBasket(UserId).Result.Id;
            //checkout.BasketId = Guid.Parse(basketService.GetBasket(UserId).Id);
            var result = await basketService.Checkout(checkout);
            if (result.IsSuccess)
                return RedirectToAction("OrderCreated");

            // creating message
            return View(Checkout());
        }

        #endregion

        [HttpGet]
        public async Task<IActionResult> OrderCreated()
        {
            return View();
        }
    }

}
