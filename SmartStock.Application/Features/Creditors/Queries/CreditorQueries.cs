using MediatR;
using SmartStock.Application.Dtos;

namespace SmartStock.Application.Features.Creditors.Queries;

public class GetCreditorQuery : IRequest<CreditorResponse>
{
    public required Guid CreditorId { get; set; }
    public required Guid UserId { get; set; }
}

public class GetAllCreditorsQuery : IRequest<IEnumerable<CreditorResponse>>
{
    public required Guid UserId { get; set; }
    public string? Search { get; set; }
}

public class GetCreditorPaymentsQuery : IRequest<IEnumerable<CreditorPaymentResponse>>
{
    public required Guid CreditorId { get; set; }
    public required Guid UserId { get; set; }
}
