using MediatR;
using SmartStock.Application.Dtos;
using SmartStock.Application.Features.Auth.Commands;
using SmartStock.Domain.Entities;
using SmartStock.Domain.Repositories;
using SmartStock.Infrastructure.Services;

namespace SmartStock.Application.Features.Auth.Handlers;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public RegisterCommandHandler(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingUser != null)
            throw new InvalidOperationException("Email is already registered.");

        var user = new User
        {
            Email = request.Email.Trim().ToLowerInvariant(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            FullName = request.FullName.Trim(),
            BusinessName = request.BusinessName?.Trim(),
            BusinessType = request.BusinessType?.Trim(),
            PhoneNumber = request.PhoneNumber?.Trim(),
            Role = "owner"
        };

        var refreshToken = _tokenService.CreateRefreshToken();
        user.RefreshTokens.Add(refreshToken);

        await _userRepository.AddAsync(user, cancellationToken);

        return new AuthResponse
        {
            User = ToUserResponse(user),
            Token = _tokenService.CreateAccessToken(user),
            RefreshToken = refreshToken.Token
        };
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

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new InvalidOperationException("Invalid email or password.");

        var refreshToken = _tokenService.CreateRefreshToken();
        refreshToken.UserId = user.Id;
        await _userRepository.AddRefreshTokenAsync(refreshToken, cancellationToken);
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user, cancellationToken);

        return new AuthResponse
        {
            User = ToUserResponse(user),
            Token = _tokenService.CreateAccessToken(user),
            RefreshToken = refreshToken.Token
        };
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

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public RefreshTokenCommandHandler(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var storedRefreshToken = await _userRepository.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken);
        if (storedRefreshToken?.User == null || !storedRefreshToken.IsActive)
            throw new InvalidOperationException("Invalid refresh token.");

        var now = DateTime.UtcNow;
        storedRefreshToken.RevokedAt = now;
        storedRefreshToken.UpdatedAt = now;

        var user = storedRefreshToken.User;
        var newRefreshToken = _tokenService.CreateRefreshToken();
        newRefreshToken.UserId = user.Id;

        await _userRepository.AddRefreshTokenAsync(newRefreshToken, cancellationToken);
        user.UpdatedAt = now;
        await _userRepository.UpdateAsync(user, cancellationToken);

        return new AuthResponse
        {
            User = ToUserResponse(user),
            Token = _tokenService.CreateAccessToken(user),
            RefreshToken = newRefreshToken.Token
        };
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

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
{
    private readonly IUserRepository _userRepository;

    public LogoutCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await _userRepository.RevokeRefreshTokensForUserAsync(request.UserId, cancellationToken);
        return Unit.Value;
    }
}
