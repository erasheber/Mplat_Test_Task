namespace Dotnet_Test_Task.Api.Features.Payments.Create;

public sealed class CreatePaymentRequest
{
    public string WalletNumber { get; init; } = null!;
    public string Account { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string? Phone { get; init; }
    public decimal Amount { get; init; }
    public string Currency { get; init; } = null!;
    public string? Comment { get; init; }
}