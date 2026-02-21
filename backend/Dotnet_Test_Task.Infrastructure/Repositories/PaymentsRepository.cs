using Dotnet_Test_Task.Application.Interfaces;
using Dotnet_Test_Task.Domain.Payments;
using Dotnet_Test_Task.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Dotnet_Test_Task.Infrastructure.Repositories;

public sealed class PaymentsRepository(PaymentsDbContext db) : IPaymentsRepository
{
    public async Task AddAsync(Payment payment, CancellationToken ct)
    {
        db.Payments.Add(payment);
        await db.SaveChangesAsync(ct);
    }

    public Task<Payment?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return db.Payments.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<IReadOnlyList<Payment>> GetAllAsync(
        int page,
        int pageSize,
        bool sortByCreatedAtDesc,
        CancellationToken ct)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize is < 1 ? 20 : pageSize > 200 ? 200 : pageSize;

        var query = db.Payments.AsNoTracking();

        query = sortByCreatedAtDesc
            ? query.OrderByDescending(x => x.CreatedAt)
            : query.OrderBy(x => x.CreatedAt);

        return await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<(decimal TotalAmount, long TotalCount)> GetTotalsAsync(CancellationToken ct)
    {
        var totalCount = await db.Payments.LongCountAsync(ct);
        var totalAmount = await db.Payments.SumAsync(x => (decimal?)x.Amount, ct) ?? 0m;
        return (totalAmount, totalCount);
    }

    public async Task<IReadOnlyList<DailyPaymentsStats>> GetDailyStatsAsync(
        DateOnly fromInclusive,
        DateOnly toInclusive,
        CancellationToken ct)
    {
        if (toInclusive < fromInclusive)
            (fromInclusive, toInclusive) = (toInclusive, fromInclusive);

        var fromDateTime = fromInclusive.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        var toDateTimeExclusive = toInclusive.AddDays(1).ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);

        var rows = await db.Payments.AsNoTracking()
            .Where(x => x.CreatedAt >= fromDateTime && x.CreatedAt < toDateTimeExclusive)
            .GroupBy(x => x.CreatedAt.Date)
            .Select(g => new
            {
                Date = g.Key,
                Count = g.LongCount(),
                Amount = g.Sum(x => x.Amount)
            })
            .OrderBy(x => x.Date)
            .ToListAsync(ct);

        return rows
            .Select(x => new DailyPaymentsStats(
                DateOnly.FromDateTime(x.Date),
                x.Count,
                x.Amount))
            .ToList();
    }
}