using DiscountService.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace DiscountService.Infrastructure.Contexts
{
    public class DiscountDataBaseContext : DbContext
    {
        public DiscountDataBaseContext(DbContextOptions<DiscountDataBaseContext> options)
        : base(options)
        {
        }
        public DbSet<DiscountCode> DiscountCodes { get; set; }
    }
}
