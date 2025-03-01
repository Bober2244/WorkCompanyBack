using MegaProject.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MegaProject.Repository;

public class AppDbContext(IConfiguration configuration) : DbContext
{
    public DbSet<Bid> Bids { get; set; }
    public DbSet<Brigade> Brigades { get; set; }
    public DbSet<BrigadeOrder> BrigadeOrders { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<MaterialOrder> MaterialOrders { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<Worker> Workers { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql(configuration.GetConnectionString("Database"))
            .UseLoggerFactory(CreateLoggerFactory())
            .LogTo(Console.WriteLine)
            .EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => builder.AddConsole());
}