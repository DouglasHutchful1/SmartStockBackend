using MediatR;
using SmartStock.Application.Dtos;
using SmartStock.Application.Features.Creditors.Queries;
using SmartStock.Domain.Repositories;

namespace SmartStock.Application.Features.Creditors.Handlers;

public class GetCreditorQueryHandler : IRequestHandler<GetCreditorQuery, CreditorResponse>
{
    private readonly ICreditorRepository _creditorRepository;

    public GetCreditorQueryHandler(ICreditorRepository creditorRepository)
    {
        _creditorRepository = creditorRepository;
    }

    public async Task<CreditorResponse> Handle(GetCreditorQuery request, CancellationToken cancellationToken)
    {
        var creditor = await _creditorRepository.GetByIdAsync(request.CreditorId, cancellationToken);
        if (creditor == null || creditor.UserId != request.UserId)
            throw new InvalidOperationException("Creditor not found.");

        return ToResponse(creditor);
    }

    private static CreditorResponse ToResponse(Domain.Entities.Creditor creditor) => new()
    {
        Id = creditor.Id,
        Name = creditor.Name,
        PhoneNumber = creditor.PhoneNumber,
        Email = creditor.Email,
        Address = creditor.Address,
        TotalOwed = creditor.TotalOwed,
        CreditLimit = creditor.CreditLimit,
        Notes = creditor.Notes,
        CreatedAt = creditor.CreatedAt,
        UpdatedAt = creditor.UpdatedAt
    };
}

public class GetAllCreditorsQueryHandler : IRequestHandler<GetAllCreditorsQuery, IEnumerable<CreditorResponse>>
{
    private readonly ICreditorRepository _creditorRepository;

    public GetAllCreditorsQueryHandler(ICreditorRepository creditorRepository)
    {
        _creditorRepository = creditorRepository;
    }

    public async Task<IEnumerable<CreditorResponse>> Handle(GetAllCreditorsQuery request, CancellationToken cancellationToken)
    {
        var creditors = await _creditorRepository.GetUserCreditorsAsync(request.UserId, cancellationToken);

        var results = creditors.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            results = results.Where(c =>
                c.Name.Contains(term) ||
                (c.Email != null && c.Email.Contains(term)) ||
                (c.PhoneNumber != null && c.PhoneNumber.Contains(term)));
        }

        return results.OrderBy(c => c.Name).Select(ToResponse);
    }

    private static CreditorResponse ToResponse(Domain.Entities.Creditor creditor) => new()
    {
        Id = creditor.Id,
        Name = creditor.Name,
        PhoneNumber = creditor.PhoneNumber,
        Email = creditor.Email,
        Address = creditor.Address,
        TotalOwed = creditor.TotalOwed,
        CreditLimit = creditor.CreditLimit,
        Notes = creditor.Notes,
        CreatedAt = creditor.CreatedAt,
        UpdatedAt = creditor.UpdatedAt
    };
}

public class GetCreditorPaymentsQueryHandler : IRequestHandler<GetCreditorPaymentsQuery, IEnumerable<CreditorPaymentResponse>>
{
    private readonly ICreditorRepository _creditorRepository;

    public GetCreditorPaymentsQueryHandler(ICreditorRepository creditorRepository)
    {
        _creditorRepository = creditorRepository;
    }

    public async Task<IEnumerable<CreditorPaymentResponse>> Handle(GetCreditorPaymentsQuery request, CancellationToken cancellationToken)
    {
        var creditor = await _creditorRepository.GetByIdAsync(request.CreditorId, cancellationToken);
        if (creditor == null || creditor.UserId != request.UserId)
            throw new InvalidOperationException("Creditor not found.");

        var payments = await _creditorRepository.GetCreditorPaymentsAsync(request.CreditorId, cancellationToken);

        return payments.Select(p => new CreditorPaymentResponse
        {
            Id = p.Id,
            CreditorId = p.CreditorId,
            SaleId = p.SaleId,
            Amount = p.Amount,
            PaymentMethod = p.PaymentMethod,
            Notes = p.Notes,
            PaidAt = p.PaidAt
        });
    }
}
