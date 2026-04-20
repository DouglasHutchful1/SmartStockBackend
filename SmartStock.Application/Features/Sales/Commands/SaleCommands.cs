using MediatR;
using SmartStock.Application.Dtos;

namespace SmartStock.Application.Features.Sales.Commands;

public class CreateSaleCommand : IRequest<SaleDto>
{
    public Guid UserId { get; set; }
    public List<SaleItemRequest> Items { get; set; } = new();
    public string PaymentMethod { get; set; } = "cash";
    public decimal AmountPaid { get; set; }
    public Guid? CreditorId { get; set; }
    public string? Notes { get; set; }
}

public class RefundSaleCommand : IRequest<SaleDto>
{
    public Guid SaleId { get; set; }
    public Guid UserId { get; set; }
}
