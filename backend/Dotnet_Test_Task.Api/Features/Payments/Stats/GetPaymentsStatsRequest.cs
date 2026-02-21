namespace Dotnet_Test_Task.Api.Features.Payments.Stats;

public sealed class GetPaymentsStatsRequest
{
    public string? From { get; init; }
    public string? To { get; init; }
}