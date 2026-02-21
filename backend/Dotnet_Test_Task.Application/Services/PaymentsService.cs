using Dotnet_Test_Task.Application.Interfaces;
using Dotnet_Test_Task.Domain.Payments;

namespace Dotnet_Test_Task.Application.Services;

public sealed class PaymentsService(IPaymentsRepository repo) : IPaymentsService
{
    public async Task<Payment> CreateAsync(CreatePaymentCommand cmd, CancellationToken ct)
    {
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            WalletNumber = cmd.WalletNumber.Trim(),
            Account = cmd.Account.Trim(),
            Email = cmd.Email.Trim(),
            Phone = string.IsNullOrWhiteSpace(cmd.Phone) ? null : cmd.Phone.Trim(),
            Amount = cmd.Amount,
            Currency = cmd.Currency.Trim().ToUpperInvariant(),
            Comment = string.IsNullOrWhiteSpace(cmd.Comment) ? null : cmd.Comment.Trim(),
            Status = PaymentStatus.Created,
            CreatedAt = DateTime.UtcNow
        };

        await repo.AddAsync(payment, ct);
        return payment;
    }

    public Task<IReadOnlyList<Payment>> GetPaymentsAsync(GetPaymentsQuery query, CancellationToken ct)
        => repo.GetAllAsync(query.Page, query.PageSize, query.SortByCreatedAtDesc, ct);

    public async Task<PaymentsStatsResult> GetStatsAsync(GetPaymentsStatsQuery query, CancellationToken ct)
    {
        var (totalAmount, totalCount) = await repo.GetTotalsAsync(ct);
        var byDays = await repo.GetDailyStatsAsync(query.FromInclusive, query.ToInclusive, ct);
        return new PaymentsStatsResult(totalAmount, totalCount, byDays);
    }
}