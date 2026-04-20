using SmartStock.Domain.Common;

namespace SmartStock.Domain.Entities;

public class Product : BaseEntity
{
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public string Name { get; set; } = null!;
    public string? Sku { get; set; }
    public string? Barcode { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
    public decimal CostPrice { get; set; } = 0;
    public decimal SellingPrice { get; set; } = 0;
    public int StockQuantity { get; set; } = 0;
    public int LowStockThreshold { get; set; } = 5;
    public string Unit { get; set; } = "pcs";
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}
