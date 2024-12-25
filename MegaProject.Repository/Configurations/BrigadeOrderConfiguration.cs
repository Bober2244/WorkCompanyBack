using MegaProject.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MegaProject.Repository.Configurations;

// Конфигурация модели BrigadeOrder
public class BrigadeOrderConfiguration : IEntityTypeConfiguration<BrigadeOrder>
{
    public void Configure(EntityTypeBuilder<BrigadeOrder> builder)
    {
        // Установка таблицы и первичного ключа
        builder.ToTable("BrigadeOrders").HasKey(x => x.Id);

        // Связь с Order
        builder.HasOne(bo => bo.Order)
            .WithMany(o => o.BrigadeOrders)
            .HasForeignKey(bo => bo.OrderId)
            .OnDelete(DeleteBehavior.Cascade); 

        // Связь с Brigade
        builder.HasOne(bo => bo.Brigade)
            .WithMany(b => b.BrigadeOrders)
            .HasForeignKey(bo => bo.BrigadeId)
            .OnDelete(DeleteBehavior.Cascade); 
    }
}