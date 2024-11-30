using MegaProject.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MegaProject.Repository.Configurations;

// Конфигурация модели BrigadeOrder
public class BrigadeOrderConfiguration : IEntityTypeConfiguration<BrigadeOrder>
{
    public void Configure(EntityTypeBuilder<BrigadeOrder> builder)
    {
        builder.ToTable("BrigadeOrders").HasKey(x => x.Id);
        builder.HasOne(bo => bo.Order)
            .WithMany(o => o.BrigadeOrders)
            .HasForeignKey(bo => bo.OrderId);
    }
}