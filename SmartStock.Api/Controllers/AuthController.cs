using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStock.Application.Features.Auth.Commands;
using SmartStock.Application.Features.Auth.Handlers;
using SmartStock.Application.Features.Creditors.Handlers;
using SmartStock.Application.Features.Creditors.Queries;
using SmartStock.Application.Features.Reports.Handlers;

namespace SmartStock.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IMediator mediator, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = new { code = "REGISTRATION_FAILED", message = ex.Message } });
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = new { code = "LOGIN_FAILED", message = ex.Message } });
        }
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = new { code = "REFRESH_FAILED", message = ex.Message } });
        }
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        try
        {
            var command = new LogoutCommand { UserId = GetUserId() };
            await _mediator.Send(command);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = new { code = "LOGOUT_FAILED", message = ex.Message } });
        }
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        try
        {
            var userId = GetUserId();
            // TODO: Implement GetUserQuery or fetch directly
            return Ok(new { message = "Implemented in next phase" });
        }
        catch (Exception ex)
        {
            return NotFound(new { error = new { code = "USER_NOT_FOUND", message = ex.Message } });
        }
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirstValue("sub") ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(userIdClaim, out var id) ? id : Guid.Empty;
    }
}
