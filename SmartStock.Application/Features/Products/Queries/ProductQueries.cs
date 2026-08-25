using MediatR;
using SmartStock.Application.Dtos;

namespace SmartStock.Application.Features.Products.Queries;

public class GetProductByIdQuery : IRequest<ProductDto>
{
    public Guid ProductId { get; set; }
    public Guid UserId { get; set; }
}

public class GetAllProductsQuery : IRequest<IEnumerable<ProductDto>>
{
    public Guid UserId { get; set; }
    public string? Search { get; set; }
    public string? Category { get; set; }
    public bool LowStockOnly { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class GetProductByBarcodeQuery : IRequest<ProductDto>
{
    public string Barcode { get; set; } = null!;
    public Guid UserId { get; set; }
}
