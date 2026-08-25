using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStock.Application.Dtos;
using SmartStock.Application.Features.Auth.Commands;
using SmartStock.Domain.Entities;
using SmartStock.Domain.Repositories;

namespace SmartStock.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserRepository _userRepository;

    public AuthController(IMediator mediator, IUserRepository userRepository)
    {
        _mediator = mediator;
        _userRepository = userRepository;
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
            if (userId == Guid.Empty)
                return Unauthorized(new { error = new { code = "INVALID_TOKEN", message = "Invalid user token." } });

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return NotFound(new { error = new { code = "USER_NOT_FOUND", message = "User was not found." } });

            return Ok(ToUserResponse(user));
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

    private static UserResponse ToUserResponse(User user) => new()
    {
        Id = user.Id,
        Email = user.Email,
        FullName = user.FullName,
        BusinessName = user.BusinessName,
        BusinessType = user.BusinessType,
        PhoneNumber = user.PhoneNumber,
        Role = user.Role,
        CreatedAt = user.CreatedAt,
        UpdatedAt = user.UpdatedAt
    };
}
