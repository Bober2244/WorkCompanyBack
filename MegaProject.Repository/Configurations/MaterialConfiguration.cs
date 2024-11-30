using MegaProject.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MegaProject.Repository.Configurations;

// Конфигурация модели Material
public class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.ToTable("Materials").HasKey(x => x.Id);
        builder.HasMany(m => m.MaterialOrders)
            .WithOne(mo => mo.Material)
            .HasForeignKey(mo => mo.MaterialId);

        builder.HasMany(m => m.Purchases)
            .WithOne(p => p.Material)
            .HasForeignKey(p => p.MaterialId);
    }
}