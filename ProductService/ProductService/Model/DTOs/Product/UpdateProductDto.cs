using System;

namespace ProductService.Model.DTOs.Product
{
    public record UpdateProductDto(Guid ProductId, string Name);
}