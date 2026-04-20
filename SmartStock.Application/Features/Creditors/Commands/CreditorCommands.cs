using MediatR;
using SmartStock.Application.Dtos;

namespace SmartStock.Application.Features.Creditors.Commands;

public class CreateCreditorCommand : IRequest<CreditorResponse>
{
    public required Guid UserId { get; set; }
    public required string Name { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public decimal CreditLimit { get; set; }
    public string? Notes { get; set; }
}

public class UpdateCreditorCommand : IRequest<CreditorResponse>
{
    public required Guid CreditorId { get; set; }
    public required Guid UserId { get; set; }
    public required string Name { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public decimal CreditLimit { get; set; }
    public string? Notes { get; set; }
}

public class DeleteCreditorCommand : IRequest<Unit>
{
    public required Guid CreditorId { get; set; }
    public required Guid UserId { get; set; }
}

public class CreateCreditorPaymentCommand : IRequest<CreditorPaymentResponse>
{
    public required Guid CreditorId { get; set; }
    public required Guid UserId { get; set; }
    public Guid? SaleId { get; set; }
    public required decimal Amount { get; set; }
    public required string PaymentMethod { get; set; }
    public string? Notes { get; set; }
}
