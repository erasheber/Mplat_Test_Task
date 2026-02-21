namespace Dotnet_Test_Task.Api.Features.Payments.List;

public sealed class GetPaymentsRequest
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;

    public string Sort { get; init; } = "desc";
}