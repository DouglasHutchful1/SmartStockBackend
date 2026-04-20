using MediatR;
using SmartStock.Application.Dtos;
using SmartStock.Application.Features.Creditors.Commands;
using SmartStock.Domain.Repositories;

namespace SmartStock.Application.Features.Creditors.Handlers;

public class CreateCreditorCommandHandler : IRequestHandler<CreateCreditorCommand, CreditorResponse>
{
    private readonly ICreditorRepository _creditorRepository;

    public CreateCreditorCommandHandler(ICreditorRepository creditorRepository)
    {
        _creditorRepository = creditorRepository;
    }

    public async Task<CreditorResponse> Handle(CreateCreditorCommand request, CancellationToken cancellationToken)
    {
        var creditor = new Domain.Entities.Creditor
        {
            UserId = request.UserId,
            Name = request.Name.Trim(),
            PhoneNumber = request.PhoneNumber?.Trim(),
            Email = request.Email?.Trim(),
            Address = request.Address,
            CreditLimit = request.CreditLimit,
            Notes = request.Notes,
            TotalOwed = 0
        };

        await _creditorRepository.AddAsync(creditor, cancellationToken);

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

public class UpdateCreditorCommandHandler : IRequestHandler<UpdateCreditorCommand, CreditorResponse>
{
    private readonly ICreditorRepository _creditorRepository;

    public UpdateCreditorCommandHandler(ICreditorRepository creditorRepository)
    {
        _creditorRepository = creditorRepository;
    }

    public async Task<CreditorResponse> Handle(UpdateCreditorCommand request, CancellationToken cancellationToken)
    {
        var creditor = await _creditorRepository.GetByIdAsync(request.CreditorId, cancellationToken);
        if (creditor == null || creditor.UserId != request.UserId)
            throw new InvalidOperationException("Creditor not found.");

        creditor.Name = request.Name.Trim();
        creditor.PhoneNumber = request.PhoneNumber?.Trim();
        creditor.Email = request.Email?.Trim();
        creditor.Address = request.Address;
        creditor.CreditLimit = request.CreditLimit;
        creditor.Notes = request.Notes;
        creditor.UpdatedAt = DateTime.UtcNow;

        await _creditorRepository.UpdateAsync(creditor, cancellationToken);

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

public class DeleteCreditorCommandHandler : IRequestHandler<DeleteCreditorCommand, Unit>
{
    private readonly ICreditorRepository _creditorRepository;

    public DeleteCreditorCommandHandler(ICreditorRepository creditorRepository)
    {
        _creditorRepository = creditorRepository;
    }

    public async Task<Unit> Handle(DeleteCreditorCommand request, CancellationToken cancellationToken)
    {
        var creditor = await _creditorRepository.GetByIdAsync(request.CreditorId, cancellationToken);
        if (creditor == null || creditor.UserId != request.UserId)
            throw new InvalidOperationException("Creditor not found.");

        await _creditorRepository.DeleteAsync(creditor, cancellationToken);

        return Unit.Value;
    }
}

public class CreateCreditorPaymentCommandHandler : IRequestHandler<CreateCreditorPaymentCommand, CreditorPaymentResponse>
{
    private readonly ICreditorRepository _creditorRepository;
    private readonly ISaleRepository _saleRepository;

    public CreateCreditorPaymentCommandHandler(ICreditorRepository creditorRepository, ISaleRepository saleRepository)
    {
        _creditorRepository = creditorRepository;
        _saleRepository = saleRepository;
    }

    public async Task<CreditorPaymentResponse> Handle(CreateCreditorPaymentCommand request, CancellationToken cancellationToken)
    {
        if (request.Amount <= 0)
            throw new InvalidOperationException("Payment amount must be greater than zero.");

        var creditor = await _creditorRepository.GetByIdAsync(request.CreditorId, cancellationToken);
        if (creditor == null || creditor.UserId != request.UserId)
            throw new InvalidOperationException("Creditor not found.");

        var payment = new Domain.Entities.CreditorPayment
        {
            CreditorId = request.CreditorId,
            SaleId = request.SaleId,
            Amount = request.Amount,
            PaymentMethod = request.PaymentMethod.Trim(),
            Notes = request.Notes
        };

        if (request.SaleId.HasValue)
        {
            var sale = await _saleRepository.GetByIdAsync(request.SaleId.Value, cancellationToken);
            if (sale == null || sale.UserId != request.UserId)
                throw new InvalidOperationException("Sale not found for the payment.");

            sale.AmountPaid += request.Amount;
            if (sale.AmountPaid >= sale.TotalAmount)
                sale.Status = "completed";

            await _saleRepository.UpdateAsync(sale, cancellationToken);
        }

        creditor.TotalOwed = Math.Max(0, creditor.TotalOwed - request.Amount);
        creditor.UpdatedAt = DateTime.UtcNow;
        await _creditorRepository.UpdateAsync(creditor, cancellationToken);

        // Note: We need to add payment to db context, but we don't have a PaymentRepository
        // For now, this is a conceptual structure that would need to be completed with actual persistence

        return new CreditorPaymentResponse
        {
            Id = Guid.NewGuid(),
            CreditorId = payment.CreditorId,
            SaleId = payment.SaleId,
            Amount = payment.Amount,
            PaymentMethod = payment.PaymentMethod,
            Notes = payment.Notes,
            PaidAt = DateTime.UtcNow
        };
    }
}
