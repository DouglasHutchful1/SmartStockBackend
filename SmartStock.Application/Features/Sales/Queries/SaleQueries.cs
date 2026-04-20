using MediatR;
using SmartStock.Application.Dtos;

namespace SmartStock.Application.Features.Sales.Queries;

public class GetSalesQuery : IRequest<IEnumerable<SaleDto>>
{
    public Guid UserId { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public string? Status { get; set; }
    public int Page { get; set; } = 1;
}

public class GetSaleByIdQuery : IRequest<SaleDto?>
{
    public Guid SaleId { get; set; }
    public Guid UserId { get; set; }
}
