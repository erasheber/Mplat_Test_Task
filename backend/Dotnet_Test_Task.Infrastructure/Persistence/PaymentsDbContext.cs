using Dotnet_Test_Task.Domain.Payments;
using Microsoft.EntityFrameworkCore;

namespace Dotnet_Test_Task.Infrastructure.Persistence;

public class PaymentsDbContext(DbContextOptions<PaymentsDbContext> options) : DbContext(options)
{
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentsDbContext).Assembly);
    }
}