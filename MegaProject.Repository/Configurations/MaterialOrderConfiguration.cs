using MegaProject.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MegaProject.Repository.Configurations;

// Конфигурация модели MaterialOrder
public class MaterialOrderConfiguration : IEntityTypeConfiguration<MaterialOrder>
{
    public void Configure(EntityTypeBuilder<MaterialOrder> builder)
    {
        builder.ToTable("MaterialOrders").HasKey(x => x.Id);
        builder.HasOne(mo => mo.Order)
            .WithMany(o => o.MaterialOrders)
            .HasForeignKey(mo => mo.OrderId);

        builder.HasOne(mo => mo.Material)
            .WithMany(m => m.MaterialOrders)
            .HasForeignKey(mo => mo.MaterialId);
        builder
            .Property(x => x.OrderId)
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Property(x => x.Id)
            .UseIdentityAlwaysColumn();
    }
}