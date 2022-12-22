using System.Collections.Generic;
using OrderService.Model.DTOs.OrderLine;

namespace OrderService.Model.DTOs.Order
{
    public class AddOrderDto
    {
        public string UserId { get; set; }
        public List<AddOrderLineDto> OrderLines { get; set; }

    }
}
