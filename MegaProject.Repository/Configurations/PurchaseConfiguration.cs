using MegaProject.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MegaProject.Repository.Configurations;

// Конфигурация модели Purchase
public class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
{
    public void Configure(EntityTypeBuilder<Purchase> builder)
    {
        builder.ToTable("Purchases").HasKey(x => x.Id);
        builder.HasOne(p => p.Material)
            .WithMany(m => m.Purchases)
            .HasForeignKey(p => p.MaterialId);
    }
}