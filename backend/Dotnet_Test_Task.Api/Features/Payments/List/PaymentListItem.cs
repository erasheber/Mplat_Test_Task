using Dotnet_Test_Task.Domain.Payments;

namespace Dotnet_Test_Task.Api.Features.Payments.List;

public sealed class PaymentListItem
{
    public DateTime CreatedAt { get; init; }
    public string Account { get; init; } = null!;
    public string Email { get; init; } = null!;
    public decimal Amount { get; init; }
    public string Currency { get; init; } = null!;
    public PaymentStatus Status { get; init; }
    public string? Comment { get; init; }
}