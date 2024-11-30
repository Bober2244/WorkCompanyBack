using MegaProject.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MegaProject.Repository.Configurations;

// Конфигурация модели Order
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders").HasKey(x => x.Id);
        builder.HasOne(o => o.Bid)
            .WithMany(b => b.Orders)
            .HasForeignKey(o => o.BidId);

        builder.HasMany(o => o.MaterialOrders)
            .WithOne(mo => mo.Order)
            .HasForeignKey(mo => mo.OrderId);

        builder.HasMany(o => o.BrigadeOrders)
            .WithOne(bo => bo.Order)
            .HasForeignKey(bo => bo.OrderId);
    }
}