using SmartStock.Domain.Common;

namespace SmartStock.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? BusinessName { get; set; }
    public string? BusinessType { get; set; }
    public string? PhoneNumber { get; set; }
    public string Role { get; set; } = "owner";

    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
    public ICollection<Creditor> Creditors { get; set; } = new List<Creditor>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}

public class RefreshToken : BaseEntity
{
    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public bool IsActive => RevokedAt == null && DateTime.UtcNow < ExpiresAt;
    public Guid UserId { get; set; }
    public User? User { get; set; }
}
