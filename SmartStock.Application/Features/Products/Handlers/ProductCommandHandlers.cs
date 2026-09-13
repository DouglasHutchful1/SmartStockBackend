using MediatR;
using SmartStock.Application.Common;
using SmartStock.Application.Dtos;
using SmartStock.Application.Features.Products.Commands;
using SmartStock.Domain.Repositories;

namespace SmartStock.Application.Features.Products.Handlers;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var name = ValidationRules.RequireText(request.Name, "Product name");
        var sku = string.IsNullOrWhiteSpace(request.Sku) ? null : request.Sku.Trim();
        var barcode = string.IsNullOrWhiteSpace(request.Barcode) ? null : request.Barcode.Trim();
        var unit = string.IsNullOrWhiteSpace(request.Unit) ? "pcs" : request.Unit.Trim();

        if (request.CostPrice < 0)
            throw new ValidationException("Cost price cannot be negative.");
        if (request.SellingPrice < 0)
            throw new ValidationException("Selling price cannot be negative.");
        if (request.StockQuantity < 0)
            throw new ValidationException("Stock quantity cannot be negative.");
        if (request.LowStockThreshold < 0)
            throw new ValidationException("Low stock threshold cannot be negative.");
        if (request.SellingPrice < request.CostPrice)
            throw new ValidationException("Selling price cannot be lower than cost price.");

        var product = new Domain.Entities.Product
        {
            UserId = request.UserId,
            Name = name,
            Sku = sku,
            Barcode = barcode,
            Category = request.Category,
            Description = request.Description,
            CostPrice = request.CostPrice,
            SellingPrice = request.SellingPrice,
            StockQuantity = request.StockQuantity,
            LowStockThreshold = request.LowStockThreshold,
            Unit = unit,
            ImageUrl = request.ImageUrl,
            IsActive = true
        };

        await _productRepository.AddAsync(product, cancellationToken);

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

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product == null || product.UserId != request.UserId)
            throw new NotFoundException("Product not found.");

        var name = ValidationRules.RequireText(request.Name, "Product name");
        var sku = string.IsNullOrWhiteSpace(request.Sku) ? null : request.Sku.Trim();
        var barcode = string.IsNullOrWhiteSpace(request.Barcode) ? null : request.Barcode.Trim();
        var unit = string.IsNullOrWhiteSpace(request.Unit) ? "pcs" : request.Unit.Trim();

        if (request.CostPrice < 0)
            throw new ValidationException("Cost price cannot be negative.");
        if (request.SellingPrice < 0)
            throw new ValidationException("Selling price cannot be negative.");
        if (request.StockQuantity < 0)
            throw new ValidationException("Stock quantity cannot be negative.");
        if (request.LowStockThreshold < 0)
            throw new ValidationException("Low stock threshold cannot be negative.");
        if (request.SellingPrice < request.CostPrice)
            throw new ValidationException("Selling price cannot be lower than cost price.");

        product.Name = name;
        product.Sku = sku;
        product.Barcode = barcode;
        product.Category = request.Category;
        product.Description = request.Description;
        product.CostPrice = request.CostPrice;
        product.SellingPrice = request.SellingPrice;
        product.StockQuantity = request.StockQuantity;
        product.LowStockThreshold = request.LowStockThreshold;
        product.Unit = unit;
        product.ImageUrl = request.ImageUrl;
        product.UpdatedAt = DateTime.UtcNow;

        await _productRepository.UpdateAsync(product, cancellationToken);

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

public class AdjustStockCommandHandler : IRequestHandler<AdjustStockCommand, ProductDto>
{
    private readonly IProductRepository _productRepository;

    public AdjustStockCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto> Handle(AdjustStockCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product == null || product.UserId != request.UserId)
            throw new NotFoundException("Product not found.");

        if (request.Adjustment == 0)
            throw new ValidationException("Adjustment cannot be zero.");

        var newQuantity = product.StockQuantity + request.Adjustment;
        if (newQuantity < 0)
            throw new ValidationException("Stock cannot be negative.");

        product.StockQuantity = newQuantity;
        product.UpdatedAt = DateTime.UtcNow;

        await _productRepository.UpdateAsync(product, cancellationToken);

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

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product == null || product.UserId != request.UserId)
            throw new NotFoundException("Product not found.");

        product.IsActive = false;
        product.UpdatedAt = DateTime.UtcNow;

        await _productRepository.UpdateAsync(product, cancellationToken);

        return Unit.Value;
    }
}
