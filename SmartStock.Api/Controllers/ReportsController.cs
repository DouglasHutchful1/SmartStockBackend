using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStock.Application.Common;
using SmartStock.Application.Features.Reports.Queries;

namespace SmartStock.Api.Controllers;

[ApiController]
[Route("api/v1/reports")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        try
        {
            var userId = GetUserId();
            var query = new GetDashboardQuery { UserId = userId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (AppException ex)
        {
            return BadRequest(new { error = new { code = ex.Code, message = ex.Message } });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = new { code = "DASHBOARD_FAILED", message = ex.Message } });
        }
    }

    [HttpGet("sales-summary")]
    public async Task<IActionResult> SalesSummary([FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] string groupBy = "day")
    {
        try
        {
            var userId = GetUserId();
            var query = new GetSalesSummaryQuery { UserId = userId, From = from, To = to, GroupBy = groupBy };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (AppException ex)
        {
            return BadRequest(new { error = new { code = ex.Code, message = ex.Message } });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = new { code = "SUMMARY_FAILED", message = ex.Message } });
        }
    }

    [HttpGet("top-products")]
    public async Task<IActionResult> TopProducts([FromQuery] int limit = 10, [FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null)
    {
        try
        {
            var userId = GetUserId();
            var query = new GetTopProductsQuery { UserId = userId, Limit = limit, From = from, To = to };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (AppException ex)
        {
            return BadRequest(new { error = new { code = ex.Code, message = ex.Message } });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = new { code = "TOP_PRODUCTS_FAILED", message = ex.Message } });
        }
    }

    [HttpGet("inventory-value")]
    public async Task<IActionResult> InventoryValue()
    {
        try
        {
            var userId = GetUserId();
            var query = new GetInventoryValueQuery { UserId = userId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (AppException ex)
        {
            return BadRequest(new { error = new { code = ex.Code, message = ex.Message } });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = new { code = "INVENTORY_FAILED", message = ex.Message } });
        }
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirstValue("sub") ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(userIdClaim, out var id) ? id : Guid.Empty;
    }
}
