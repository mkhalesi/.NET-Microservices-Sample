namespace Microservices.Web.Frontend.Models.DTO.Product
{
    public class ProductDto
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public object image { get; set; }
        public int price { get; set; }
        public ProductCategory productCategory { get; set; }
    }
}