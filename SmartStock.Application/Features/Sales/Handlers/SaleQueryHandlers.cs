using MediatR;
using SmartStock.Application.Dtos;
using SmartStock.Application.Features.Sales.Queries;
using SmartStock.Domain.Repositories;

namespace SmartStock.Application.Features.Sales.Handlers;

public class GetSalesQueryHandler : IRequestHandler<GetSalesQuery, IEnumerable<SaleDto>>
{
    private readonly ISaleRepository _saleRepository;

    public GetSalesQueryHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<IEnumerable<SaleDto>> Handle(GetSalesQuery request, CancellationToken cancellationToken)
    {
        var sales = await _saleRepository.GetUserSalesAsync(request.UserId, request.From ?? DateTime.MinValue, request.To ?? DateTime.UtcNow, cancellationToken);

        var results = sales.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(request.Status))
            results = results.Where(s => s.Status == request.Status.Trim());

        var pageSize = 20;
        results = results.Skip((request.Page - 1) * pageSize).Take(pageSize);

        return results.Select(ToDto);
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

public class GetSaleByIdQueryHandler : IRequestHandler<GetSaleByIdQuery, SaleDto>
{
    private readonly ISaleRepository _saleRepository;

    public GetSaleByIdQueryHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<SaleDto> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(request.SaleId, cancellationToken);
        if (sale == null || sale.UserId != request.UserId)
            throw new InvalidOperationException("Sale not found.");

        return ToDto(sale);
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
