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
            entity.Ignore(u => u.UpdatedAt);
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
            entity.Property(p => p.Id).HasColumnName("id");
            entity.Property(p => p.UserId).HasColumnName("userid");
            entity.Property(p => p.Name).HasColumnName("name");
            entity.Property(p => p.Sku).HasColumnName("sku");
            entity.Property(p => p.Barcode).HasColumnName("barcode");
            entity.Property(p => p.Category).HasColumnName("category");
            entity.Property(p => p.Description).HasColumnName("description");
            entity.Property(p => p.CostPrice).HasColumnName("costprice");
            entity.Property(p => p.SellingPrice).HasColumnName("sellingprice");
            entity.Property(p => p.StockQuantity).HasColumnName("stockquantity");
            entity.Property(p => p.LowStockThreshold).HasColumnName("lowstockthreshold");
            entity.Property(p => p.Unit).HasColumnName("unit");
            entity.Property(p => p.ImageUrl).HasColumnName("imageurl");
            entity.Property(p => p.IsActive).HasColumnName("isactive");
            entity.Property(p => p.CreatedAt).HasColumnName("createdat");
            entity.Ignore(p => p.UpdatedAt);
            entity.HasIndex(p => new { p.UserId, p.Sku }).IsUnique();
            entity.HasIndex(p => new { p.UserId, p.Barcode }).IsUnique();
            entity.Property(p => p.IsActive).HasDefaultValue(true);
            entity.Property(p => p.LowStockThreshold).HasDefaultValue(5);
            entity.Property(p => p.CreatedAt).HasDefaultValueSql("NOW()");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.ToTable("sales");
            entity.Property(s => s.Id).HasColumnName("id");
            entity.Property(s => s.UserId).HasColumnName("userid");
            entity.Property(s => s.CreditorId).HasColumnName("creditorid");
            entity.Property(s => s.SaleNumber).HasColumnName("salenumber");
            entity.Property(s => s.TotalAmount).HasColumnName("totalamount");
            entity.Property(s => s.AmountPaid).HasColumnName("amountpaid");
            entity.Property(s => s.PaymentMethod).HasColumnName("paymentmethod");
            entity.Property(s => s.Status).HasColumnName("status");
            entity.Property(s => s.Notes).HasColumnName("notes");
            entity.Property(s => s.SoldAt).HasColumnName("soldat");
            entity.Property(s => s.CreatedAt).HasColumnName("createdat");
            entity.Ignore(s => s.UpdatedAt);
            entity.HasIndex(s => new { s.UserId, s.SaleNumber }).IsUnique();
            entity.Property(s => s.PaymentMethod).HasDefaultValue("cash");
            entity.Property(s => s.Status).HasDefaultValue("completed");
            entity.Property(s => s.CreatedAt).HasDefaultValueSql("NOW()");
            entity.Property(s => s.SoldAt).HasDefaultValueSql("NOW()");
        });

        modelBuilder.Entity<SaleItem>(entity =>
        {
            entity.ToTable("saleitems");
            entity.Property(i => i.Id).HasColumnName("id");
            entity.Property(i => i.SaleId).HasColumnName("saleid");
            entity.Property(i => i.ProductId).HasColumnName("productid");
            entity.Property(i => i.ProductName).HasColumnName("productname");
            entity.Property(i => i.Quantity).HasColumnName("quantity");
            entity.Property(i => i.UnitPrice).HasColumnName("unitprice");
            entity.Property(i => i.Subtotal).HasColumnName("subtotal");
            entity.Ignore(i => i.CreatedAt);
            entity.Ignore(i => i.UpdatedAt);
            entity.Property(i => i.ProductName).IsRequired();
        });

        modelBuilder.Entity<Creditor>(entity =>
        {
            entity.ToTable("creditors");
            entity.Property(c => c.Id).HasColumnName("id");
            entity.Property(c => c.UserId).HasColumnName("userid");
            entity.Property(c => c.Name).HasColumnName("name");
            entity.Property(c => c.PhoneNumber).HasColumnName("phonenumber");
            entity.Property(c => c.Email).HasColumnName("email");
            entity.Property(c => c.Address).HasColumnName("address");
            entity.Property(c => c.TotalOwed).HasColumnName("totalowed");
            entity.Property(c => c.CreditLimit).HasColumnName("creditlimit");
            entity.Property(c => c.Notes).HasColumnName("notes");
            entity.Property(c => c.CreatedAt).HasColumnName("createdat");
            entity.Ignore(c => c.UpdatedAt);
            entity.Property(c => c.TotalOwed).HasDefaultValue(0);
            entity.Property(c => c.CreatedAt).HasDefaultValueSql("NOW()");
        });

        modelBuilder.Entity<CreditorPayment>(entity =>
        {
            entity.ToTable("creditorpayments");
            entity.Property(c => c.Id).HasColumnName("id");
            entity.Property(c => c.CreditorId).HasColumnName("creditorid");
            entity.Property(c => c.SaleId).HasColumnName("saleid");
            entity.Property(c => c.Amount).HasColumnName("amount");
            entity.Property(c => c.PaymentMethod).HasColumnName("paymentmethod");
            entity.Property(c => c.Notes).HasColumnName("notes");
            entity.Property(c => c.PaidAt).HasColumnName("paidat");
            entity.Ignore(c => c.CreatedAt);
            entity.Ignore(c => c.UpdatedAt);
            entity.Property(c => c.PaymentMethod).HasDefaultValue("cash");
            entity.Property(c => c.PaidAt).HasDefaultValueSql("NOW()");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("refresh_tokens");
            entity.Property(rt => rt.Id).HasColumnName("id");
            entity.Property(rt => rt.CreatedAt).HasColumnName("createdat");
            entity.Ignore(rt => rt.UpdatedAt);
            entity.Property(rt => rt.Token).HasColumnName("token");
            entity.Property(rt => rt.ExpiresAt).HasColumnName("expiresat");
            entity.Property(rt => rt.RevokedAt).HasColumnName("revokedat");
            entity.Property(rt => rt.UserId).HasColumnName("userid");
        });
    }
}
