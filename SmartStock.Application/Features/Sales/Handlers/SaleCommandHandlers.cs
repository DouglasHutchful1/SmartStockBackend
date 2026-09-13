using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartStock.Application.Common;
using SmartStock.Application.Dtos;
using SmartStock.Application.Features.Sales.Commands;
using SmartStock.Domain.Repositories;
using SmartStock.Infrastructure.Data;

namespace SmartStock.Application.Features.Sales.Handlers;

public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, SaleDto>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICreditorRepository _creditorRepository;
    private readonly ApplicationDbContext _dbContext;

    public CreateSaleCommandHandler(
        ISaleRepository saleRepository,
        IProductRepository productRepository,
        ICreditorRepository creditorRepository,
        ApplicationDbContext dbContext)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _creditorRepository = creditorRepository;
        _dbContext = dbContext;
    }

    public async Task<SaleDto> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        if (!request.Items.Any())
            throw new ValidationException("At least one sale item is required.");
        if (request.AmountPaid < 0)
            throw new ValidationException("Amount paid cannot be negative.");
        if (string.IsNullOrWhiteSpace(request.PaymentMethod))
            throw new ValidationException("Payment method is required.");

        using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var productIds = request.Items.Select(i => i.ProductId).Distinct().ToList();
            var products = (await _productRepository.GetUserProductsAsync(request.UserId, cancellationToken))
                .Where(p => productIds.Contains(p.Id))
                .ToList();

            if (products.Count != productIds.Count)
                throw new ValidationException("One or more products are invalid or inactive.");

            var lineItems = new List<Domain.Entities.SaleItem>();
            var totalAmount = 0m;

            foreach (var item in request.Items)
            {
                if (item.Quantity <= 0)
                    throw new ValidationException("Each sale item quantity must be greater than zero.");
                if (item.UnitPrice < 0)
                    throw new ValidationException("Unit price cannot be negative.");

                var product = products.First(p => p.Id == item.ProductId);
                if (product.StockQuantity < item.Quantity)
                    throw new ValidationException($"Insufficient stock for {product.Name}.");

                var subtotal = item.Quantity * item.UnitPrice;
                totalAmount += subtotal;
                product.StockQuantity -= item.Quantity;
                product.UpdatedAt = DateTime.UtcNow;

                lineItems.Add(new Domain.Entities.SaleItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Subtotal = subtotal
                });
            }

            if (request.AmountPaid > totalAmount && request.PaymentMethod.Trim().ToLowerInvariant() != "credit")
                throw new ValidationException("Amount paid cannot exceed the total sale amount for non-credit payments.");

            var status = request.AmountPaid >= totalAmount ? "completed" : "pending";
            var sale = new Domain.Entities.Sale
            {
                UserId = request.UserId,
                CreditorId = request.CreditorId,
                SaleNumber = $"INV-{DateTime.UtcNow:yyyyMMddHHmmssfff}",
                TotalAmount = totalAmount,
                AmountPaid = request.AmountPaid,
                PaymentMethod = request.PaymentMethod,
                Status = status,
                Notes = request.Notes,
                SoldAt = DateTime.UtcNow,
                Items = lineItems
            };

            if (request.CreditorId.HasValue && request.PaymentMethod.Trim().ToLowerInvariant() == "credit")
            {
                var creditor = await _creditorRepository.GetByIdAsync(request.CreditorId.Value, cancellationToken);
                if (creditor == null || creditor.UserId != request.UserId)
                    throw new NotFoundException("Creditor not found.");

                creditor.TotalOwed += totalAmount - request.AmountPaid;
                creditor.UpdatedAt = DateTime.UtcNow;
                await _creditorRepository.UpdateAsync(creditor, cancellationToken);
            }

            await _saleRepository.AddAsync(sale, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return ToDto(sale);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private static SaleDto ToDto(Domain.Entities.Sale sale) => new()
    {
        Id = sale.Id,
        SaleNumber = sale.SaleNumber,
        CreditorId = sale.CreditorId,
        TotalAmount = sale.TotalAmount,
        AmountPaid = sale.AmountPaid,
        PaymentMethod = sale.PaymentMethod,
        Status = sale.Status,
        Notes = sale.Notes,
        SoldAt = sale.SoldAt,
        CreatedAt = sale.CreatedAt,
        Items = sale.Items.Select(i => new SaleItemDto
        {
            Id = i.Id,
            ProductId = i.ProductId,
            ProductName = i.ProductName,
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice,
            Subtotal = i.Subtotal
        }).ToList()
    };
}

public class RefundSaleCommandHandler : IRequestHandler<RefundSaleCommand, SaleDto>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICreditorRepository _creditorRepository;
    private readonly ApplicationDbContext _dbContext;

    public RefundSaleCommandHandler(
        ISaleRepository saleRepository,
        IProductRepository productRepository,
        ICreditorRepository creditorRepository,
        ApplicationDbContext dbContext)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _creditorRepository = creditorRepository;
        _dbContext = dbContext;
    }

    public async Task<SaleDto> Handle(RefundSaleCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var sale = await _saleRepository.GetByIdAsync(request.SaleId, cancellationToken);
            if (sale == null || sale.UserId != request.UserId)
                throw new NotFoundException("Sale not found.");

            if (sale.Status == "refunded")
                throw new ValidationException("Sale has already been refunded.");

            foreach (var item in sale.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);
                if (product != null)
                {
                    product.StockQuantity += item.Quantity;
                    product.UpdatedAt = DateTime.UtcNow;
                    await _productRepository.UpdateAsync(product, cancellationToken);
                }
            }

            if (sale.CreditorId.HasValue)
            {
                var creditor = await _creditorRepository.GetByIdAsync(sale.CreditorId.Value, cancellationToken);
                if (creditor != null)
                {
                    creditor.TotalOwed = Math.Max(0, creditor.TotalOwed - (sale.TotalAmount - sale.AmountPaid));
                    creditor.UpdatedAt = DateTime.UtcNow;
                    await _creditorRepository.UpdateAsync(creditor, cancellationToken);
                }
            }

            sale.Status = "refunded";
            await _saleRepository.UpdateAsync(sale, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return ToDto(sale);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private static SaleDto ToDto(Domain.Entities.Sale sale) => new()
    {
        Id = sale.Id,
        SaleNumber = sale.SaleNumber,
        CreditorId = sale.CreditorId,
        TotalAmount = sale.TotalAmount,
        AmountPaid = sale.AmountPaid,
        PaymentMethod = sale.PaymentMethod,
        Status = sale.Status,
        Notes = sale.Notes,
        SoldAt = sale.SoldAt,
        CreatedAt = sale.CreatedAt,
        Items = sale.Items.Select(i => new SaleItemDto
        {
            Id = i.Id,
            ProductId = i.ProductId,
            ProductName = i.ProductName,
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice,
            Subtotal = i.Subtotal
        }).ToList()
    };
}
