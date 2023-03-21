using Microsoft.EntityFrameworkCore;
using ProductService.Model.Entities;

namespace ProductService.Infrastructure.Contexts
{
    public class ProductDatabaseContext : DbContext
    {
        public ProductDatabaseContext(DbContextOptions<ProductDatabaseContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
