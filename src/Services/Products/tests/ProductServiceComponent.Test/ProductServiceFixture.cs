using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProductService.Infrastructure.Contexts;
using ProductService.MessagingBus.Config;
using ProductService.MessagingBus.SendMessages;
using ProductService.Model.Services.ProductCategoryService;
using ProductService.Model.Services.ProductService;

namespace ProductService.ComponentTests
{
    public class ProductServiceFixture
    {
        public IProductService productService { get; }
        public IMessageBus messageBus { get; }
        public IOptions<RabbitMqConfiguration> options;
        public ICategoryService categoryService { get; }
        public ProductDatabaseContext db { get; set; }
        public ProductServiceFixture()
        {
            this.options = new OptionsWrapper<RabbitMqConfiguration>(new RabbitMqConfiguration());
            DbContextOptionsBuilder<ProductDatabaseContext> builder = new DbContextOptionsBuilder<ProductDatabaseContext>();

            builder.UseInMemoryDatabase("productDatabase");

            db = new ProductDatabaseContext(builder.Options);
            productService = new Model.Services.ProductService.ProductService(db);
            categoryService = new CategoryService(db);

            messageBus = new RabbitMqMessageBus(this.options);
        }
    }
}
