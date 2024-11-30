using MegaProject.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MegaProject.Repository.Configurations;

// Конфигурация модели Worker
public class WorkerConfiguration : IEntityTypeConfiguration<Worker>
{
    public void Configure(EntityTypeBuilder<Worker> builder)
    {
        builder.ToTable("Workers").HasKey(x => x.Id);
        builder.HasOne(w => w.Brigade)
            .WithMany(b => b.Workers)
            .HasForeignKey(w => w.BrigadeId)
            .IsRequired(false);
    }
}