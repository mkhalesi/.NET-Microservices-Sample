﻿using System;

namespace Microservices.Web.Frontend.Models.DTO.Basket
{
    public class AddToBasketDTO
    {
        public string UserId { get; set; }
        public Guid BasketId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
    }
}
