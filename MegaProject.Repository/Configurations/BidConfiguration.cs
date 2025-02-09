using MegaProject.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MegaProject.Repository.Configurations;

// Конфигурация модели Bid
public class BidConfiguration : IEntityTypeConfiguration<Bid>
{
    public void Configure(EntityTypeBuilder<Bid> builder)
    {
        builder.ToTable("Bids").HasKey(x => x.Id);
        builder.HasOne(b => b.Customer)
            .WithMany(c => c.Bids)
            .HasForeignKey(b => b.CustomerId);

        builder.HasMany(b => b.Orders)
            .WithOne(o => o.Bid)
            .HasForeignKey(o => o.BidId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}