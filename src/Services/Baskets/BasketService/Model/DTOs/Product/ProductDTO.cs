﻿using System;

namespace BasketService.Model.DTOs.Product
{
    public class ProductDTO
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int UnitPrice { get; set; }
        public string ImageUrl { get; set; }
    }
}
