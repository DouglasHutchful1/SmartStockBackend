using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStock.Application.Common;
using SmartStock.Application.Features.Sales.Commands;
using SmartStock.Application.Features.Sales.Queries;

namespace SmartStock.Api.Controllers;

[ApiController]
[Route("api/v1/sales")]
[Authorize]
public class SalesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SalesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// List sales for the current user with optional date range and status filters.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> List([FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] string? status, [FromQuery] int page = 1)
    {
        try
        {
            var userId = GetUserId();
            var query = new GetSalesQuery
            {
                UserId = userId,
                From = from,
                To = to,
                Status = status,
                Page = page
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (AppException ex)
        {
            return BadRequest(new { error = new { code = ex.Code, message = ex.Message } });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = new { code = "LIST_FAILED", message = ex.Message } });
        }
    }

    /// <summary>
    /// Get sale details by id for the current user.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var userId = GetUserId();
            var query = new GetSaleByIdQuery { SaleId = id, UserId = userId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (NotFoundException)
        {
            return NotFound(new { error = new { code = "SALE_NOT_FOUND", message = "Sale not found." } });
        }
        catch (AppException ex)
        {
            return BadRequest(new { error = new { code = ex.Code, message = ex.Message } });
        }
        catch (InvalidOperationException)
        {
            return NotFound(new { error = new { code = "SALE_NOT_FOUND", message = "Sale not found." } });
        }
    }

    /// <summary>
    /// Create a new sale and adjust product stock accordingly.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSaleCommand command)
    {
        try
        {
            command.UserId = GetUserId();
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (AppException ex)
        {
            return BadRequest(new { error = new { code = ex.Code, message = ex.Message } });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = new { code = "CREATE_FAILED", message = ex.Message } });
        }
    }

    /// <summary>
    /// Refund a sale and restore product stock.
    /// </summary>
    [HttpPost("{id}/refund")]
    public async Task<IActionResult> Refund(Guid id)
    {
        try
        {
            var userId = GetUserId();
            var command = new RefundSaleCommand { SaleId = id, UserId = userId };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (AppException ex)
        {
            return BadRequest(new { error = new { code = ex.Code, message = ex.Message } });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = new { code = "REFUND_FAILED", message = ex.Message } });
        }
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirstValue("sub") ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(userIdClaim, out var id) ? id : Guid.Empty;
    }
}
