namespace SmartStock.Application.Dtos;

public class RegisterRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? BusinessName { get; set; }
    public string? BusinessType { get; set; }
    public string? PhoneNumber { get; set; }
}

public class LoginRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class RefreshTokenRequest
{
    public string RefreshToken { get; set; } = null!;
}

public class CreateProductRequest
{
    public string Name { get; set; } = null!;
    public string? Sku { get; set; }
    public string? Barcode { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
    public decimal CostPrice { get; set; }
    public decimal SellingPrice { get; set; }
    public int StockQuantity { get; set; }
    public int LowStockThreshold { get; set; } = 5;
    public string Unit { get; set; } = "pcs";
    public string? ImageUrl { get; set; }
}

public class UpdateProductRequest
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
}

public class StockAdjustmentRequest
{
    public int Adjustment { get; set; }
}

public class CreateSaleRequest
{
    public List<SaleItemRequest> Items { get; set; } = new();
    public string PaymentMethod { get; set; } = "cash";
    public decimal AmountPaid { get; set; }
    public Guid? CreditorId { get; set; }
    public string? Notes { get; set; }
}

public class SaleItemRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public class CreateCreditorRequest
{
    public string Name { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public decimal? CreditLimit { get; set; }
    public string? Notes { get; set; }
}

public class UpdateCreditorRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public decimal? CreditLimit { get; set; }
    public string? Notes { get; set; }
}

public class CreateCreditorPaymentRequest
{
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = "cash";
    public Guid? SaleId { get; set; }
    public string? Notes { get; set; }
}
