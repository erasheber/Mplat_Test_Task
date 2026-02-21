using FastEndpoints;
using FluentValidation;

namespace Dotnet_Test_Task.Api.Features.Payments.Create;

public sealed class CreatePaymentValidator : Validator<CreatePaymentRequest>
{
    private static readonly HashSet<string> AllowedCurrencies = new(StringComparer.OrdinalIgnoreCase)
    {
        "KZT", "RUB", "USD", "EUR"
    };

    public CreatePaymentValidator()
    {
        RuleFor(x => x.WalletNumber)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(x => x.Account)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);

        RuleFor(x => x.Phone)
            .MaximumLength(32)
            .Matches(@"^\+?[0-9]{7,15}$")
            .When(x => !string.IsNullOrWhiteSpace(x.Phone))
            .WithMessage("Phone must contain 7-15 digits and may start with '+'.");

        RuleFor(x => x.Amount)
            .GreaterThan(0);

        RuleFor(x => x.Currency)
            .NotEmpty()
            .Must(c => AllowedCurrencies.Contains(c))
            .WithMessage("Currency must be one of: KZT, RUB, USD, EUR.");

        RuleFor(x => x.Comment)
            .MaximumLength(1024);
    }
}