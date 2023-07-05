using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ProductService.Infrastructure.Contexts;
using ProductService.MessagingBus.Config;
using ProductService.MessagingBus.SendMessages;
using ProductService.Model.Services.ProductCategoryService;
using ProductService.Model.Services.ProductService;

namespace ProductService.IntegrationTests
{
    public class ProductServiceFixture : IDisposable
    {
        public IProductService productService { get; }
        public ICategoryService categoryService { get; }
        public ProductDatabaseContext dbContext { get; set; }
        public IMessageBus messageBus { get; }
        public IOptions<RabbitMqConfiguration> options { get; }
        //private IConfiguration _configuration;

        public ProductServiceFixture()
        {
            DbContextOptionsBuilder<ProductDatabaseContext> builder = new DbContextOptionsBuilder<ProductDatabaseContext>();
            //builder.UseSqlServer(_configuration["ProductDBTestConnection"]);
            builder.UseSqlServer("Data Source=DESKTOP-6JFHOLC\\SQLDEV2019;Initial Catalog=ProductDBTest;Integrated Security=True;MultipleActiveResultSets=True");
            dbContext = new ProductDatabaseContext(builder.Options);
            dbContext.Database.EnsureCreated();

            this.options = new OptionsWrapper<RabbitMqConfiguration>(new RabbitMqConfiguration());
            messageBus = new RabbitMqMessageBus(this.options);

            productService = new ProductService.Model.Services.ProductService.ProductService(dbContext);
            categoryService = new CategoryService(dbContext);
        }

        public void Dispose()
        {
            dbContext.Database.EnsureDeleted();
        }
    }
}