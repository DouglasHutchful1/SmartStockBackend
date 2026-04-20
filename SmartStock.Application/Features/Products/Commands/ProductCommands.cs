using MediatR;
using SmartStock.Application.Dtos;

namespace SmartStock.Application.Features.Products.Commands;

public class CreateProductCommand : IRequest<ProductDto>
{
    public Guid UserId { get; set; }
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

public class UpdateProductCommand : IRequest<ProductDto>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
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

public class AdjustStockCommand : IRequest<ProductDto>
{
    public Guid ProductId { get; set; }
    public Guid UserId { get; set; }
    public int Adjustment { get; set; }
}

public class DeleteProductCommand : IRequest<Unit>
{
    public Guid ProductId { get; set; }
    public Guid UserId { get; set; }
}
