﻿using Microsoft.AspNetCore.Mvc;
using System;
using ProductService.Model.DTOs.Product;
using ProductService.Model.Services.ProductService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;
        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        // GET: api/<ProductController>
        [HttpGet]
        public IActionResult Get()
        {
            var data = productService.GetProductList();
            return Ok(data);
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var data = productService.GetProduct(id);
            return Ok(data);
        }

        [HttpGet("/api/product/verify/{Id}")]
        public IActionResult Verify(Guid Id)
        {
            var data = productService.GetProduct(Id);
            return Ok(new ProductVerifyDTO(data));
        }

    }
}
