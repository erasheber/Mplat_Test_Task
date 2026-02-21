using Dotnet_Test_Task.Application.Interfaces;
using FastEndpoints;

namespace Dotnet_Test_Task.Api.Features.Payments.Create;

public sealed class CreatePaymentEndpoint(IPaymentsService service)
    : Endpoint<CreatePaymentRequest, CreatePaymentResponse>
{
    public override void Configure()
    {
        Post("/api/payments");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreatePaymentRequest req, CancellationToken ct)
    {
        var command = new CreatePaymentCommand(
            req.WalletNumber,
            req.Account,
            req.Email,
            req.Phone,
            req.Amount,
            req.Currency,
            req.Comment);

        var payment = await service.CreateAsync(command, ct);

        var response = new CreatePaymentResponse
        {
            Id = payment.Id,
            Status = payment.Status,
            CreatedAt = payment.CreatedAt
        };

        await Send.CreatedAtAsync($"/api/payments/{payment.Id}", response, cancellation: ct);
    }
}