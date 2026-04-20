using SmartStock.Domain.Entities;

namespace SmartStock.Domain.Repositories;

/// <summary>
/// Generic repository interface for data access patterns
/// </summary>
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Specialized product repository with domain-specific queries
/// </summary>
public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetByBarcodeAsync(string barcode, Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetUserProductsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetLowStockProductsAsync(Guid userId, CancellationToken cancellationToken = default);
}

/// <summary>
/// Specialized sales repository with domain-specific queries
/// </summary>
public interface ISaleRepository : IRepository<Sale>
{
    Task<Sale?> GetSaleWithItemsAsync(Guid saleId, Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Sale>> GetUserSalesAsync(Guid userId, DateTime from, DateTime to, CancellationToken cancellationToken = default);
}

/// <summary>
/// Specialized creditor repository with domain-specific queries
/// </summary>
public interface ICreditorRepository : IRepository<Creditor>
{
    Task<IEnumerable<Creditor>> GetUserCreditorsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CreditorPayment>> GetCreditorPaymentsAsync(Guid creditorId, CancellationToken cancellationToken = default);
}

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}
