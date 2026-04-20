using Microsoft.EntityFrameworkCore;
using SmartStock.Domain.Entities;

namespace SmartStock.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Sale> Sales { get; set; } = null!;
    public DbSet<SaleItem> SaleItems { get; set; } = null!;
    public DbSet<Creditor> Creditors { get; set; } = null!;
    public DbSet<CreditorPayment> CreditorPayments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("dbo");
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.Property(u => u.Id).HasColumnName("id");
            entity.Property(u => u.CreatedAt).HasColumnName("createdat");
            entity.Property(u => u.UpdatedAt).HasColumnName("updatedat");
            entity.Property(u => u.Email).HasColumnName("email");
            entity.Property(u => u.PasswordHash).HasColumnName("passwordhash");
            entity.Property(u => u.FullName).HasColumnName("fullname");
            entity.Property(u => u.BusinessName).HasColumnName("businessname");
            entity.Property(u => u.BusinessType).HasColumnName("businesstype");
            entity.Property(u => u.PhoneNumber).HasColumnName("phonenumber");
            entity.Property(u => u.Role).HasColumnName("role");
            entity.HasIndex(u => u.Email).IsUnique();
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("products");
            entity.HasIndex(p => new { p.UserId, p.Sku }).IsUnique();
            entity.HasIndex(p => new { p.UserId, p.Barcode }).IsUnique();
            entity.Property(p => p.IsActive).HasDefaultValue(true);
            entity.Property(p => p.LowStockThreshold).HasDefaultValue(5);
            entity.Property(p => p.CreatedAt).HasDefaultValueSql("NOW()");
            entity.Property(p => p.UpdatedAt).HasDefaultValueSql("NOW()");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.ToTable("sales");
            entity.HasIndex(s => new { s.UserId, s.SaleNumber }).IsUnique();
            entity.Property(s => s.PaymentMethod).HasDefaultValue("cash");
            entity.Property(s => s.Status).HasDefaultValue("completed");
            entity.Property(s => s.CreatedAt).HasDefaultValueSql("NOW()");
            entity.Property(s => s.SoldAt).HasDefaultValueSql("NOW()");
        });

        modelBuilder.Entity<SaleItem>(entity =>
        {
            entity.ToTable("sale_items");
            entity.Property(i => i.ProductName).IsRequired();
        });

        modelBuilder.Entity<Creditor>(entity =>
        {
            entity.ToTable("creditors");
            entity.Property(c => c.TotalOwed).HasDefaultValue(0);
            entity.Property(c => c.CreatedAt).HasDefaultValueSql("NOW()");
            entity.Property(c => c.UpdatedAt).HasDefaultValueSql("NOW()");
        });

        modelBuilder.Entity<CreditorPayment>(entity =>
        {
            entity.ToTable("creditor_payments");
            entity.Property(c => c.PaymentMethod).HasDefaultValue("cash");
            entity.Property(c => c.PaidAt).HasDefaultValueSql("NOW()");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("refresh_tokens");
            entity.Property(rt => rt.Id).HasColumnName("id");
            entity.Property(rt => rt.CreatedAt).HasColumnName("createdat");
            entity.Property(rt => rt.UpdatedAt).HasColumnName("updatedat");
            entity.Property(rt => rt.Token).HasColumnName("token");
            entity.Property(rt => rt.ExpiresAt).HasColumnName("expiresat");
            entity.Property(rt => rt.RevokedAt).HasColumnName("revokedat");
            entity.Property(rt => rt.UserId).HasColumnName("userid");
        });
    }
}
