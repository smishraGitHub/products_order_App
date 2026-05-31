using Microsoft.EntityFrameworkCore;
using MyConsoleApp.Models;

namespace MyConsoleApp.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    
    public DbSet<Orders> Orders => Set<Orders>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("products");

            entity.HasKey(product => product.Id);

            entity.Property(product => product.Name)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(product => product.Description)
                .HasMaxLength(500);

            entity.Property(product => product.Price)
                .HasPrecision(18, 2);

            entity.Property(product => product.CreatedAtUtc)
                .HasDefaultValueSql("timezone('utc', now())");
        });


        modelBuilder.Entity<Orders>(entity =>
        {
            entity.ToTable("orders");

            entity.HasKey(order => order.Id);

            entity.Property(order => order.Quantity)
                .IsRequired();

            entity.Property(order => order.OrderDateUtc)
                .HasDefaultValueSql("timezone('utc', now())");

            entity.HasOne(order => order.Product)
                .WithMany(product => product.Orders)
                .HasForeignKey(order => order.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
