using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Entities;

namespace PaymentService.Application.Context
{
    public interface IPaymentDatabaseContext
    {
         DbSet<Order> Order { get; set; }
         DbSet<Payment> Payments { get; set; }
         int SaveChanges();
    }
}
