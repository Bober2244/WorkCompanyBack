using MegaProject.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MegaProject.Repository.Configurations;

// Конфигурация модели Пользователя
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users").HasKey(x => x.Id);
        
        builder
            .HasOne(u => u.Brigade)
            .WithOne(b => b.User)
            .HasForeignKey<User>(u => u.BrigadeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

