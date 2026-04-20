using MediatR;
using SmartStock.Application.Dtos;

namespace SmartStock.Application.Features.Auth.Commands;

public class RegisterCommand : IRequest<AuthResponse>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? BusinessName { get; set; }
    public string? BusinessType { get; set; }
    public string? PhoneNumber { get; set; }
}

public class LoginCommand : IRequest<AuthResponse>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class RefreshTokenCommand : IRequest<AuthResponse>
{
    public string RefreshToken { get; set; } = null!;
}

public class LogoutCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
}
