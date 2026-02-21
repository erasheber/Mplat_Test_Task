using FastEndpoints;
using FluentValidation;

namespace Dotnet_Test_Task.Api.Features.Payments.Stats;

public sealed class GetPaymentsStatsValidator : Validator<GetPaymentsStatsRequest>
{
    public GetPaymentsStatsValidator()
    {
        RuleFor(x => x.From)
            .Must(BeNullOrIsoDate)
            .WithMessage("From must be YYYY-MM-DD")
            .When(x => x.From is not null);

        RuleFor(x => x.To)
            .Must(BeNullOrIsoDate)
            .WithMessage("To must be YYYY-MM-DD")
            .When(x => x.To is not null);
    }

    private static bool BeNullOrIsoDate(string? s)
        => s is null || DateOnly.TryParseExact(s, "yyyy-MM-dd", out _);
}