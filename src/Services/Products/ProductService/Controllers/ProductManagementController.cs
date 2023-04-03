using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using ProductService.MessagingBus.Config;
using ProductService.MessagingBus.Messages;
using ProductService.MessagingBus.SendMessages;
using ProductService.Model.DTOs.Product;
using ProductService.Model.Services.ProductService;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ProductsManagement")]
    public class ProductManagementController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IMessageBus messageBus;
        private readonly string ExchangeName_UpdateProductName;
        public ProductManagementController(IProductService productService, IMessageBus messageBus,
            IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            this.productService = productService;
            this.messageBus = messageBus;
            ExchangeName_UpdateProductName = rabbitMqOptions.Value.ExchangeName_UpdateProductName;
        }

        [HttpPost]
        public IActionResult Post([FromBody] AddNewProductDto addNewProductDto)
        {
            var productId = productService.AddNewProduct(addNewProductDto);
            return Created($"api/ProductManagement/get/{productId}", productId);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var products = productService.GetProductList();
            return Ok(products);
        }


        [HttpGet("Id")]
        public IActionResult Get(Guid Id)
        {
            var product = productService.GetProduct(Id);
            return Ok(product);
        }

        [HttpPut]
        public IActionResult Put(UpdateProductDto updateProduct)
        {
            var result = productService.UpdateProductName(updateProduct);
            if (result)
            {
                UpdateProductNameMessage message = new UpdateProductNameMessage()
                {
                    Id = updateProduct.ProductId,
                    NewName = updateProduct.Name
                };
                messageBus.SendMessage(message, ExchangeName_UpdateProductName);
            }

            return Ok(result);
        }

    }
}
