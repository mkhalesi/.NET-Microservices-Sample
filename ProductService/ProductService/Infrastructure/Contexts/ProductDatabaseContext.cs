using Microsoft.EntityFrameworkCore;
using ProductService.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
