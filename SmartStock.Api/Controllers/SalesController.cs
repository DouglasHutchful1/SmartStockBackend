using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        catch (Exception ex)
        {
            return BadRequest(new { error = new { code = "LIST_FAILED", message = ex.Message } });
        }
    }

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
        catch (InvalidOperationException)
        {
            return NotFound(new { error = new { code = "SALE_NOT_FOUND", message = "Sale not found." } });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSaleCommand command)
    {
        try
        {
            command.UserId = GetUserId();
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = new { code = "CREATE_FAILED", message = ex.Message } });
        }
    }

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
