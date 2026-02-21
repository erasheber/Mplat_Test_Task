namespace Dotnet_Test_Task.Api.Features.Payments.Stats;

public sealed class GetPaymentsStatsResponse
{
    public decimal TotalAmount { get; init; }
    public long TotalCount { get; init; }
    public List<DailyStatsItem> ByDays { get; init; } = [];
}

public sealed class DailyStatsItem
{
    public string Date { get; init; } = null!;
    public long Count { get; init; }
    public decimal Amount { get; init; }
}