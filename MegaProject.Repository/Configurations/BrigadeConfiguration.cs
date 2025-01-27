using MegaProject.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MegaProject.Repository.Configurations;

// Конфигурация модели Brigade
public class BrigadeConfiguration : IEntityTypeConfiguration<Brigade>
{
    public void Configure(EntityTypeBuilder<Brigade> builder)
    {
        builder.ToTable("Brigades").HasKey(x => x.Id);
        builder.HasMany(b => b.Workers)
            .WithOne(w => w.Brigade)
            .HasForeignKey(w => w.BrigadeId);

        builder.HasMany(b => b.BrigadeOrders)
            .WithOne(bo => bo.Brigade)
            .HasForeignKey(bo => bo.BrigadeId);
        
        builder
            .HasOne(b => b.User)
            .WithOne(u => u.Brigade)
            .HasForeignKey<Brigade>(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}