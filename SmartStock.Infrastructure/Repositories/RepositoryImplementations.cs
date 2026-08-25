using Microsoft.EntityFrameworkCore;
using SmartStock.Domain.Entities;
using SmartStock.Domain.Repositories;
using SmartStock.Infrastructure.Data;

namespace SmartStock.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Products.FirstOrDefaultAsync(p => p.Id == id && p.IsActive, cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Products.Where(p => p.IsActive).ToListAsync(cancellationToken);
    }

    public async Task<Product> AddAsync(Product entity, CancellationToken cancellationToken = default)
    {
        _context.Products.Add(entity);
        await SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(Product entity, CancellationToken cancellationToken = default)
    {
        _context.Products.Update(entity);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Product entity, CancellationToken cancellationToken = default)
    {
        _context.Products.Remove(entity);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Product?> GetByBarcodeAsync(string barcode, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Products.FirstOrDefaultAsync(
            p => p.Barcode == barcode && p.UserId == userId && p.IsActive, cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetUserProductsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Products.Where(p => p.UserId == userId && p.IsActive).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetLowStockProductsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Where(p => p.UserId == userId && p.IsActive && p.StockQuantity <= p.LowStockThreshold)
            .ToListAsync(cancellationToken);
    }
}

public class SaleRepository : ISaleRepository
{
    private readonly ApplicationDbContext _context;

    public SaleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Sales.Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Sale>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Sales.Include(s => s.Items).ToListAsync(cancellationToken);
    }

    public async Task<Sale> AddAsync(Sale entity, CancellationToken cancellationToken = default)
    {
        _context.Sales.Add(entity);
        await SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(Sale entity, CancellationToken cancellationToken = default)
    {
        _context.Sales.Update(entity);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Sale entity, CancellationToken cancellationToken = default)
    {
        _context.Sales.Remove(entity);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Sale?> GetSaleWithItemsAsync(Guid saleId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Sales.Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == saleId && s.UserId == userId, cancellationToken);
    }

    public async Task<IEnumerable<Sale>> GetUserSalesAsync(Guid userId, DateTime from, DateTime to, CancellationToken cancellationToken = default)
    {
        return await _context.Sales.Include(s => s.Items)
            .Where(s => s.UserId == userId && s.SoldAt >= from && s.SoldAt <= to)
            .OrderByDescending(s => s.SoldAt)
            .ToListAsync(cancellationToken);
    }
}

public class CreditorRepository : ICreditorRepository
{
    private readonly ApplicationDbContext _context;

    public CreditorRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Creditor?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Creditors.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Creditor>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Creditors.ToListAsync(cancellationToken);
    }

    public async Task<Creditor> AddAsync(Creditor entity, CancellationToken cancellationToken = default)
    {
        _context.Creditors.Add(entity);
        await SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(Creditor entity, CancellationToken cancellationToken = default)
    {
        _context.Creditors.Update(entity);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Creditor entity, CancellationToken cancellationToken = default)
    {
        _context.Creditors.Remove(entity);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Creditor>> GetUserCreditorsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Creditors.Where(c => c.UserId == userId).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CreditorPayment>> GetCreditorPaymentsAsync(Guid creditorId, CancellationToken cancellationToken = default)
    {
        return await _context.CreditorPayments.Where(p => p.CreditorId == creditorId).ToListAsync(cancellationToken);
    }
}

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Users.ToListAsync(cancellationToken);
    }

    public async Task<User> AddAsync(User entity, CancellationToken cancellationToken = default)
    {
        _context.Users.Add(entity);
        await SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(User entity, CancellationToken cancellationToken = default)
    {
        _context.Users.Update(entity);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(User entity, CancellationToken cancellationToken = default)
    {
        _context.Users.Remove(entity);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FirstOrDefaultAsync(
            u => u.Email == email.Trim().ToLowerInvariant(), cancellationToken);
    }

    public async Task<RefreshToken?> GetByRefreshTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);
    }

    public Task AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        _context.RefreshTokens.Add(refreshToken);
        // SaveChanges will be called by UpdateAsync
        return Task.CompletedTask;
    }

    public async Task RevokeRefreshTokensForUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var tokens = await _context.RefreshTokens
            .Where(rt => rt.UserId == userId && rt.RevokedAt == null && rt.ExpiresAt > now)
            .ToListAsync(cancellationToken);

        foreach (var token in tokens)
        {
            token.RevokedAt = now;
            token.UpdatedAt = now;
        }

        await SaveChangesAsync(cancellationToken);
    }
}
