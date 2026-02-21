using Dotnet_Test_Task.Domain.Payments;

namespace Dotnet_Test_Task.Application.Interfaces;

public interface IPaymentsRepository
{
    Task AddAsync(Payment payment, CancellationToken ct);

    Task<IReadOnlyList<Payment>> GetAllAsync(
        int page,
        int pageSize,
        bool sortByCreatedAtDesc,
        CancellationToken ct);

    Task<Payment?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<(decimal TotalAmount, long TotalCount)> GetTotalsAsync(CancellationToken ct);

    Task<IReadOnlyList<DailyPaymentsStats>> GetDailyStatsAsync(DateOnly fromInclusive, DateOnly toInclusive, CancellationToken ct);
}

public sealed record DailyPaymentsStats(DateOnly Date, long Count, decimal Amount);