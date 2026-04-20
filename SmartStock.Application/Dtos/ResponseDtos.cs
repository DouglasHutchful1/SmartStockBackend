namespace SmartStock.Application.Dtos;

public class UserResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? BusinessName { get; set; }
    public string? BusinessType { get; set; }
    public string? PhoneNumber { get; set; }
    public string Role { get; set; } = "owner";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class AuthResponse
{
    public UserResponse? User { get; set; }
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Sku { get; set; }
    public string? Barcode { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
    public decimal CostPrice { get; set; }
    public decimal SellingPrice { get; set; }
    public int StockQuantity { get; set; }
    public int LowStockThreshold { get; set; }
    public string Unit { get; set; } = "pcs";
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class SaleDto
{
    public Guid Id { get; set; }
    public string SaleNumber { get; set; } = string.Empty;
    public Guid? CreditorId { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal AmountPaid { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime SoldAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<SaleItemDto> Items { get; set; } = new();
}

public class SaleItemDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
}

public class CreditorDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public decimal TotalOwed { get; set; }
    public decimal? CreditLimit { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class DashboardDto
{
    public decimal TodaySales { get; set; }
    public decimal WeekSales { get; set; }
    public int LowStockCount { get; set; }
    public int TotalProducts { get; set; }
    public decimal TotalOwed { get; set; }
}

// Aliases for consistency with handlers
public class CreditorResponse : CreditorDto;
public class CreditorPaymentResponse
{
    public Guid Id { get; set; }
    public Guid CreditorId { get; set; }
    public Guid? SaleId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime PaidAt { get; set; }
}

public class TimeSeriesPoint
{
    public string Label { get; set; } = string.Empty;
    public decimal Value { get; set; }
}

public class TopProductResponse
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int QuantitySold { get; set; }
    public decimal Revenue { get; set; }
}

public class InventoryValueResponse
{
    public decimal TotalCostValue { get; set; }
    public decimal TotalSellingValue { get; set; }
}

// Response DTO alias for SaleItemDto
public class SaleItemResponse : SaleItemDto;

// Response DTO aliases for consistency
public class DashboardResponse : DashboardDto;

