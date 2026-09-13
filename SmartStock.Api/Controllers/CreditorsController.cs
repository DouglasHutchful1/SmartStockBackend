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
