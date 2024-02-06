using System;
using System.Linq;
using System.Security.Claims;
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
        public BasketController(IBasketService basketService,
            IProductService productService,
            IDiscountService discountService)
        {
            this.basketService = basketService;
            this.productService = productService;
            this.discountService = discountService;
        }

        #endregion

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?.Value;

            var basket = await basketService.GetBasket(userId);

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

        public async Task<IActionResult> Delete(Guid Id)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?.Value;

            await basketService.DeleteFromBasket(Id, userId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddToBasket(Guid ProductId)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?.Value;

            var product = productService.GetProduct(ProductId);
            var basket = await basketService.GetBasket(userId);

            AddToBasketDTO item = new AddToBasketDTO()
            {
                BasketId = basket.Id,
                ImageUrl = product.image.ToString(),
                ProductId = product.id,
                ProductName = product.name,
                Quantity = 1,
                UnitPrice = product.price,
            };
            await basketService.AddToBasket(item, userId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(Guid BasketItemId, int quantity)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?.Value;

            await basketService.UpdateQuantity(BasketItemId, quantity, userId);
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

                var userId = HttpContext.User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?.Value;

                var basket = await basketService.GetBasket(userId);
                //basketService.ApplyDiscountToBasket(Guid.Parse(basket.Id), discount.Data.Id);
                await basketService.ApplyDiscountToBasket(basket.Id, discount.Data.Id, userId);
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

        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutDTO checkout)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?.Value;

            checkout.UserId = userId;
            checkout.BasketId = basketService.GetBasket(userId).Result.Id;
            //checkout.BasketId = Guid.Parse(basketService.GetBasket(UserId).Id);
            var result = await basketService.Checkout(checkout);
            if (result.IsSuccess)
                return RedirectToAction("OrderCreated");

            // creating message
            return View(checkout);
        }

        #endregion

        public IActionResult OrderCreated()
        {
            return View();
        }
    }

}
