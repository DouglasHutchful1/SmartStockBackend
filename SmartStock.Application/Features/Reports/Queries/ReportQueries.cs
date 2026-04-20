using MediatR;
using SmartStock.Application.Dtos;

namespace SmartStock.Application.Features.Reports.Queries;

public class GetDashboardQuery : IRequest<DashboardResponse>
{
    public required Guid UserId { get; set; }
}

public class GetSalesSummaryQuery : IRequest<IEnumerable<TimeSeriesPoint>>
{
    public required Guid UserId { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public string GroupBy { get; set; } = "day"; // day, week, month
}

public class GetTopProductsQuery : IRequest<IEnumerable<TopProductResponse>>
{
    public required Guid UserId { get; set; }
    public int Limit { get; set; } = 10;
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}

public class GetInventoryValueQuery : IRequest<InventoryValueResponse>
{
    public required Guid UserId { get; set; }
}
