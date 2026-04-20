using SmartStock.Domain.Common;

namespace SmartStock.Domain.Entities;

public class Creditor : BaseEntity
{
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public string Name { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public decimal TotalOwed { get; set; } = 0;
    public decimal? CreditLimit { get; set; }
    public string? Notes { get; set; }

    public ICollection<CreditorPayment> Payments { get; set; } = new List<CreditorPayment>();
}

public class CreditorPayment : BaseEntity
{
    public Guid CreditorId { get; set; }
    public Creditor? Creditor { get; set; }
    public Guid? SaleId { get; set; }
    public Sale? Sale { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = "cash";
    public string? Notes { get; set; }
    public DateTime PaidAt { get; set; } = DateTime.UtcNow;
}
