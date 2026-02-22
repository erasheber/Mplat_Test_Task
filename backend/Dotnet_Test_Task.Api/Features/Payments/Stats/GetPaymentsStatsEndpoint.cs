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
        int tzOffsetMinutes = 0;
        if (HttpContext.Request.Headers.TryGetValue("X-Timezone-Offset", out var h) &&
            int.TryParse(h.ToString(), out var parsed) &&
            parsed >= -14 * 60 && parsed <= 14 * 60)
        {
            tzOffsetMinutes = parsed;
        }
        
        var offset = TimeSpan.FromMinutes(-tzOffsetMinutes);
        
        var todayLocal = DateOnly.FromDateTime(DateTime.UtcNow + offset);
        
        var defaultFrom = todayLocal.AddDays(-29);
        var defaultTo = todayLocal;

        if (!TryParseDate(req.From, out var from))
            from = defaultFrom;

        if (!TryParseDate(req.To, out var to))
            to = defaultTo;

        if (to < from)
            (from, to) = (to, from);
        
        if (from > todayLocal)
        {
            await Send.OkAsync(new GetPaymentsStatsResponse
            {
                TotalAmount = 0,
                TotalCount = 0,
                ByDays = new List<DailyStatsItem>()
            }, ct);
            return;
        }

        if (to > todayLocal)
            to = todayLocal;
        
        var query = new GetPaymentsStatsQuery(from, to, tzOffsetMinutes);
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