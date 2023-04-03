namespace Microservices.Admin.Frontend.Models.ViewServices.ProductServices;

public record ProductDto(Guid id, string? name, string? description,
    string? image, int price,
    ProductCategoryDto? productCategory);