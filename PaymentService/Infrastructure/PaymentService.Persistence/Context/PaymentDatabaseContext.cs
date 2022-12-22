using Microsoft.EntityFrameworkCore;
using PaymentService.Application.Context;
using PaymentService.Domain.Entities;

namespace PaymentService.Persistence.Context
{
    public class PaymentDatabaseContext : DbContext, IPaymentDatabaseContext
    {
        public PaymentDatabaseContext(DbContextOptions<PaymentDatabaseContext> options) : base(options)
        {

        }

        public DbSet<Order> Order { get; set; }
        public DbSet<Payment> Payments { get; set; }
    }
}
