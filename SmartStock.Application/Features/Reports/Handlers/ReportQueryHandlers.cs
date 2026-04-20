using System.Globalization;
using MediatR;
using SmartStock.Application.Dtos;
using SmartStock.Application.Features.Reports.Queries;
using SmartStock.Domain.Repositories;

namespace SmartStock.Application.Features.Reports.Handlers;

public class GetDashboardQueryHandler : IRequestHandler<GetDashboardQuery, DashboardResponse>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICreditorRepository _creditorRepository;

    public GetDashboardQueryHandler(ISaleRepository saleRepository, IProductRepository productRepository, ICreditorRepository creditorRepository)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _creditorRepository = creditorRepository;
    }

    public async Task<DashboardResponse> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.Date;
        var weekStart = today.AddDays(-7);

        var sales = await _saleRepository.GetUserSalesAsync(request.UserId, weekStart, DateTime.UtcNow, cancellationToken);
        var todaySales = sales.Where(s => s.SoldAt.Date == today).Sum(s => s.TotalAmount);
        var weekSales = sales.Sum(s => s.TotalAmount);

        var products = await _productRepository.GetUserProductsAsync(request.UserId, cancellationToken);
        var lowStockCount = products.Count(p => p.StockQuantity <= p.LowStockThreshold);
        var totalProducts = products.Count(p => p.IsActive);

        var creditors = await _creditorRepository.GetUserCreditorsAsync(request.UserId, cancellationToken);
        var totalOwed = creditors.Sum(c => c.TotalOwed);

        return new DashboardResponse
        {
            TodaySales = todaySales,
            WeekSales = weekSales,
            LowStockCount = lowStockCount,
            TotalProducts = totalProducts,
            TotalOwed = totalOwed
        };
    }
}

public class GetSalesSummaryQueryHandler : IRequestHandler<GetSalesSummaryQuery, IEnumerable<TimeSeriesPoint>>
{
    private readonly ISaleRepository _saleRepository;

    public GetSalesSummaryQueryHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<IEnumerable<TimeSeriesPoint>> Handle(GetSalesSummaryQuery request, CancellationToken cancellationToken)
    {
        var from = request.From ?? DateTime.UtcNow.AddMonths(-1);
        var to = request.To ?? DateTime.UtcNow;

        var sales = await _saleRepository.GetUserSalesAsync(request.UserId, from, to, cancellationToken);

        var calendar = CultureInfo.InvariantCulture.Calendar;
        var normalizedGroup = request.GroupBy.Trim().ToLowerInvariant();

        var data = normalizedGroup switch
        {
            "month" => sales
                .GroupBy(s => new { s.SoldAt.Year, s.SoldAt.Month })
                .Select(g => new TimeSeriesPoint { Label = $"{g.Key.Year}-{g.Key.Month:D2}", Value = g.Sum(s => s.TotalAmount) })
                .OrderBy(p => p.Label)
                .ToList(),
            "week" => sales
                .GroupBy(s => new { Year = s.SoldAt.Year, Week = calendar.GetWeekOfYear(s.SoldAt, CalendarWeekRule.FirstDay, DayOfWeek.Monday) })
                .Select(g => new TimeSeriesPoint { Label = $"W{g.Key.Week:00}/{g.Key.Year}", Value = g.Sum(s => s.TotalAmount) })
                .OrderBy(p => p.Label)
                .ToList(),
            _ => sales
                .GroupBy(s => s.SoldAt.Date)
                .Select(g => new TimeSeriesPoint { Label = g.Key.ToString("yyyy-MM-dd"), Value = g.Sum(s => s.TotalAmount) })
                .OrderBy(p => p.Label)
                .ToList()
        };

        return data;
    }
}

public class GetTopProductsQueryHandler : IRequestHandler<GetTopProductsQuery, IEnumerable<TopProductResponse>>
{
    private readonly ISaleRepository _saleRepository;

    public GetTopProductsQueryHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<IEnumerable<TopProductResponse>> Handle(GetTopProductsQuery request, CancellationToken cancellationToken)
    {
        var from = request.From ?? DateTime.UtcNow.AddMonths(-1);
        var to = request.To ?? DateTime.UtcNow;

        var sales = await _saleRepository.GetUserSalesAsync(request.UserId, from, to, cancellationToken);

        var topProducts = sales
            .SelectMany(s => s.Items)
            .GroupBy(item => new { item.ProductId, item.ProductName })
            .Select(g => new TopProductResponse
            {
                ProductId = g.Key.ProductId,
                ProductName = g.Key.ProductName,
                QuantitySold = g.Sum(item => item.Quantity),
                Revenue = g.Sum(item => item.Subtotal)
            })
            .OrderByDescending(r => r.QuantitySold)
            .ThenByDescending(r => r.Revenue)
            .Take(request.Limit)
            .ToList();

        return topProducts;
    }
}

public class GetInventoryValueQueryHandler : IRequestHandler<GetInventoryValueQuery, InventoryValueResponse>
{
    private readonly IProductRepository _productRepository;

    public GetInventoryValueQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<InventoryValueResponse> Handle(GetInventoryValueQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetUserProductsAsync(request.UserId, cancellationToken);

        var totalCost = products.Sum(p => p.CostPrice * p.StockQuantity);
        var totalSelling = products.Sum(p => p.SellingPrice * p.StockQuantity);

        return new InventoryValueResponse
        {
            TotalCostValue = totalCost,
            TotalSellingValue = totalSelling
        };
    }
}
