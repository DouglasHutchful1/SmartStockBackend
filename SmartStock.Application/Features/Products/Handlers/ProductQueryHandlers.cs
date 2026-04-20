using MediatR;
using SmartStock.Application.Dtos;
using SmartStock.Application.Features.Products.Queries;
using SmartStock.Domain.Repositories;

namespace SmartStock.Application.Features.Products.Handlers;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product == null || product.UserId != request.UserId)
            throw new InvalidOperationException("Product not found.");

        return ToDto(product);
    }

    private static ProductDto ToDto(Domain.Entities.Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Sku = product.Sku,
        Barcode = product.Barcode,
        Category = product.Category,
        Description = product.Description,
        CostPrice = product.CostPrice,
        SellingPrice = product.SellingPrice,
        StockQuantity = product.StockQuantity,
        LowStockThreshold = product.LowStockThreshold,
        Unit = product.Unit,
        ImageUrl = product.ImageUrl,
        IsActive = product.IsActive,
        CreatedAt = product.CreatedAt,
        UpdatedAt = product.UpdatedAt
    };
}

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
{
    private readonly IProductRepository _productRepository;

    public GetAllProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetUserProductsAsync(request.UserId, cancellationToken);

        var results = products.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            results = results.Where(p =>
                p.Name.Contains(term) ||
                (p.Sku != null && p.Sku.Contains(term)) ||
                (p.Barcode != null && p.Barcode.Contains(term)));
        }

        if (!string.IsNullOrWhiteSpace(request.Category))
            results = results.Where(p => p.Category == request.Category.Trim());

        if (request.LowStockOnly)
            results = results.Where(p => p.StockQuantity <= p.LowStockThreshold);

        var pageSize = 20;
        results = results.Skip((request.Page - 1) * pageSize).Take(pageSize);

        return results.Select(ToDto);
    }

    private static ProductDto ToDto(Domain.Entities.Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Sku = product.Sku,
        Barcode = product.Barcode,
        Category = product.Category,
        Description = product.Description,
        CostPrice = product.CostPrice,
        SellingPrice = product.SellingPrice,
        StockQuantity = product.StockQuantity,
        LowStockThreshold = product.LowStockThreshold,
        Unit = product.Unit,
        ImageUrl = product.ImageUrl,
        IsActive = product.IsActive,
        CreatedAt = product.CreatedAt,
        UpdatedAt = product.UpdatedAt
    };
}

public class GetProductByBarcodeQueryHandler : IRequestHandler<GetProductByBarcodeQuery, ProductDto>
{
    private readonly IProductRepository _productRepository;

    public GetProductByBarcodeQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto> Handle(GetProductByBarcodeQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByBarcodeAsync(request.Barcode, request.UserId, cancellationToken);
        if (product == null)
            throw new InvalidOperationException("Product not found.");

        return ToDto(product);
    }

    private static ProductDto ToDto(Domain.Entities.Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Sku = product.Sku,
        Barcode = product.Barcode,
        Category = product.Category,
        Description = product.Description,
        CostPrice = product.CostPrice,
        SellingPrice = product.SellingPrice,
        StockQuantity = product.StockQuantity,
        LowStockThreshold = product.LowStockThreshold,
        Unit = product.Unit,
        ImageUrl = product.ImageUrl,
        IsActive = product.IsActive,
        CreatedAt = product.CreatedAt,
        UpdatedAt = product.UpdatedAt
    };
}
