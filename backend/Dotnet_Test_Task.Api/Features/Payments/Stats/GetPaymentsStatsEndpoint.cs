using Dotnet_Test_Task.Application.Interfaces;
using FastEndpoints;

namespace Dotnet_Test_Task.Api.Features.Payments.Stats;

public sealed class GetPaymentsStatsEndpoint(IPaymentsService service)
    : Endpoint<GetPaymentsStatsRequest, GetPaymentsStatsResponse>
{
    public override void Configure()
    {
        Get("/api/payments/stats");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetPaymentsStatsRequest req, CancellationToken ct)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var defaultFrom = today.AddDays(-29);
        var defaultTo = today;

        if (!TryParseDate(req.From, out var from))
            from = defaultFrom;

        if (!TryParseDate(req.To, out var to))
            to = defaultTo;

        if (to < from)
            (from, to) = (to, from);
        
        if (from > today)
        {
            await Send.OkAsync(new GetPaymentsStatsResponse(), ct);
            return;
        }
        
        if (to > today) to = today;

        var maxDays = 366;
        if (to.DayNumber - from.DayNumber > maxDays)
            to = from.AddDays(maxDays);

        var query = new GetPaymentsStatsQuery(from, to);
        var stats = await service.GetStatsAsync(query, ct);

        var response = new GetPaymentsStatsResponse
        {
            TotalAmount = stats.TotalAmount,
            TotalCount = stats.TotalCount,
            ByDays = stats.ByDays
                .Select(d => new DailyStatsItem
                {
                    Date = d.Date.ToString("yyyy-MM-dd"),
                    Count = d.Count,
                    Amount = d.Amount
                })
                .ToList()
        };

        await Send.OkAsync(response, ct);
    }

    private static bool TryParseDate(string? input, out DateOnly value)
        => DateOnly.TryParseExact(input, "yyyy-MM-dd", out value);
}