using Dotnet_Test_Task.Application.Interfaces;
using FastEndpoints;

namespace Dotnet_Test_Task.Api.Features.Payments.List;

public sealed class GetPaymentsEndpoint(IPaymentsService service)
    : Endpoint<GetPaymentsRequest, List<PaymentListItem>>
{
    public override void Configure()
    {
        Get("/api/payments");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetPaymentsRequest req, CancellationToken ct)
    {
        var sortDesc = !string.Equals(req.Sort, "asc", StringComparison.OrdinalIgnoreCase);

        var query = new GetPaymentsQuery(
            Page: req.Page,
            PageSize: req.PageSize,
            SortByCreatedAtDesc: sortDesc);

        var payments = await service.GetPaymentsAsync(query, ct);

        var response = payments.Select(p => new PaymentListItem
        {
            CreatedAt = p.CreatedAt,
            Account = p.Account,
            Email = p.Email,
            Amount = p.Amount,
            Currency = p.Currency,
            Status = p.Status,
            Comment = p.Comment
        }).ToList();

        await Send.OkAsync(response, ct);
    }
}