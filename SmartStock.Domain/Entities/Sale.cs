using SmartStock.Domain.Common;

namespace SmartStock.Domain.Entities;

public class Sale : BaseEntity
{
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid? CreditorId { get; set; }
    public Creditor? Creditor { get; set; }
    public string SaleNumber { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal AmountPaid { get; set; }
    public string PaymentMethod { get; set; } = "cash";
    public string Status { get; set; } = "completed";
    public string? Notes { get; set; }
    public DateTime SoldAt { get; set; } = DateTime.UtcNow;

    public ICollection<SaleItem> Items { get; set; } = new List<SaleItem>();
}

public class SaleItem : BaseEntity
{
    public Guid SaleId { get; set; }
    public Sale? Sale { get; set; }
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
}
