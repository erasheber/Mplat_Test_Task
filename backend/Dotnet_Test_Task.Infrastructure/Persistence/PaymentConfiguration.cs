using Dotnet_Test_Task.Domain.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dotnet_Test_Task.Infrastructure.Persistence;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> b)
    {
        b.ToTable("payments");

        b.HasKey(x => x.Id);

        b.Property(x => x.WalletNumber).IsRequired().HasMaxLength(64);
        b.Property(x => x.Account).IsRequired().HasMaxLength(64);
        b.Property(x => x.Email).IsRequired().HasMaxLength(256);
        b.Property(x => x.Phone).HasMaxLength(32);

        b.Property(x => x.Currency).IsRequired().HasMaxLength(3);
        b.Property(x => x.Amount).HasPrecision(18, 2);

        b.Property(x => x.Comment).HasMaxLength(1024);

        b.Property(x => x.Status).IsRequired();
        b.Property(x => x.CreatedAt).IsRequired();

        b.HasIndex(x => x.CreatedAt);
        b.HasIndex(x => x.Account);
        b.HasIndex(x => x.Email);
    }
}