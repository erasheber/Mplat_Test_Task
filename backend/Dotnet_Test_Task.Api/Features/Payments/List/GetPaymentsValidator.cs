using FastEndpoints;
using FluentValidation;

namespace Dotnet_Test_Task.Api.Features.Payments.List;

public sealed class GetPaymentsValidator : Validator<GetPaymentsRequest>
{
    public GetPaymentsValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0);

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(200);

        RuleFor(x => x.Sort)
            .Must(s => string.Equals(s, "asc", StringComparison.OrdinalIgnoreCase) ||
                       string.Equals(s, "desc", StringComparison.OrdinalIgnoreCase))
            .WithMessage("Sort must be 'asc' or 'desc'.");
    }
}