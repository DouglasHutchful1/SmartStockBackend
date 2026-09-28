using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStock.Application.Common;
using SmartStock.Application.Features.Creditors.Commands;
using SmartStock.Application.Features.Creditors.Queries;

namespace SmartStock.Api.Controllers;

[ApiController]
[Route("api/v1/creditors")]
[Authorize]
public class CreditorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CreditorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get a list of creditors for the current user, optionally filtered by search term.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search)
    {
        try
        {
            var userId = GetUserId();
            var query = new GetAllCreditorsQuery { UserId = userId, Search = search };
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
    /// Get creditor details by id for the current user.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var userId = GetUserId();
            var query = new GetCreditorQuery { CreditorId = id, UserId = userId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (AppException ex) when (ex is NotFoundException)
        {
            return NotFound(new { error = new { code = ex.Code, message = ex.Message } });
        }
        catch (InvalidOperationException)
        {
            return NotFound(new { error = new { code = "CREDITOR_NOT_FOUND", message = "Creditor not found." } });
        }
    }

    /// <summary>
    /// Create a new creditor record for the current user.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCreditorCommand command)
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
        catch (Exception ex)
        {
            return BadRequest(new { error = new { code = "CREATE_FAILED", message = ex.Message } });
        }
    }

    /// <summary>
    /// Update an existing creditor for the current user.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCreditorCommand command)
    {
        try
        {
            command.CreditorId = id;
            command.UserId = GetUserId();
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (AppException ex) when (ex is NotFoundException)
        {
            return NotFound(new { error = new { code = ex.Code, message = ex.Message } });
        }
        catch (AppException ex)
        {
            return BadRequest(new { error = new { code = ex.Code, message = ex.Message } });
        }
        catch (InvalidOperationException)
        {
            return NotFound(new { error = new { code = "CREDITOR_NOT_FOUND", message = "Creditor not found." } });
        }
    }

    /// <summary>
    /// Delete a creditor record for the current user.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var userId = GetUserId();
            var command = new DeleteCreditorCommand { CreditorId = id, UserId = userId };
            await _mediator.Send(command);
            return NoContent();
        }
        catch (AppException ex) when (ex is NotFoundException)
        {
            return NotFound(new { error = new { code = ex.Code, message = ex.Message } });
        }
        catch (InvalidOperationException)
        {
            return NotFound(new { error = new { code = "CREDITOR_NOT_FOUND", message = "Creditor not found." } });
        }
    }

    /// <summary>
    /// Get all payments made to a creditor.
    /// </summary>
    [HttpGet("{id}/payments")]
    public async Task<IActionResult> GetPayments(Guid id)
    {
        try
        {
            var userId = GetUserId();
            var query = new GetCreditorPaymentsQuery { CreditorId = id, UserId = userId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (AppException ex) when (ex is NotFoundException)
        {
            return NotFound(new { error = new { code = ex.Code, message = ex.Message } });
        }
        catch (InvalidOperationException)
        {
            return NotFound(new { error = new { code = "CREDITOR_NOT_FOUND", message = "Creditor not found." } });
        }
    }

    /// <summary>
    /// Create a payment record for a creditor and adjust balances.
    /// </summary>
    [HttpPost("{id}/payments")]
    public async Task<IActionResult> CreatePayment(Guid id, [FromBody] CreateCreditorPaymentCommand command)
    {
        try
        {
            command.CreditorId = id;
            command.UserId = GetUserId();
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetPayments), new { id }, result);
        }
        catch (AppException ex) when (ex is NotFoundException)
        {
            return NotFound(new { error = new { code = ex.Code, message = ex.Message } });
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

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirstValue("sub") ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(userIdClaim, out var id) ? id : Guid.Empty;
    }
}
