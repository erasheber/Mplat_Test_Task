using Dotnet_Test_Task.Domain.Payments;

namespace Dotnet_Test_Task.Api.Features.Payments.Create;

public sealed class CreatePaymentResponse
{
    public Guid Id { get; init; }
    public PaymentStatus Status { get; init; }
    public DateTime CreatedAt { get; init; }
}