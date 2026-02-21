using Dotnet_Test_Task.Domain.Payments;

namespace Dotnet_Test_Task.Application.Interfaces;

public interface IPaymentsService
{
    Task<Payment> CreateAsync(CreatePaymentCommand cmd, CancellationToken ct);

    Task<IReadOnlyList<Payment>> GetPaymentsAsync(GetPaymentsQuery query, CancellationToken ct);

    Task<PaymentsStatsResult> GetStatsAsync(GetPaymentsStatsQuery query, CancellationToken ct);
}

public sealed record CreatePaymentCommand(
    string WalletNumber,
    string Account,
    string Email,
    string? Phone,
    decimal Amount,
    string Currency,
    string? Comment);

public sealed record GetPaymentsQuery(int Page = 1, int PageSize = 20, bool SortByCreatedAtDesc = true);

public sealed record GetPaymentsStatsQuery(DateOnly FromInclusive, DateOnly ToInclusive);

public sealed record PaymentsStatsResult(
    decimal TotalAmount,
    long TotalCount,
    IReadOnlyList<DailyPaymentsStats> ByDays);