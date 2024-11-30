using MegaProject.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MegaProject.Repository.Configurations;

// Конфигурация модели Customer
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers").HasKey(x => x.Id);
        builder.HasMany(c => c.Bids)
            .WithOne(b => b.Customer)
            .HasForeignKey(b => b.CustomerId);
    }
}