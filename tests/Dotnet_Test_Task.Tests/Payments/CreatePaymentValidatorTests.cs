using Dotnet_Test_Task.Api.Features.Payments.Create;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace Dotnet_Test_Task.Tests.Payments;

public class CreatePaymentValidatorTests
{
    [Fact]
    public void Validator_Should_Fail_ForInvalidEmail()
    {
        var v = new CreatePaymentValidator();

        var req = new CreatePaymentRequest
        {
            WalletNumber = "w",
            Account = "a",
            Email = "not-an-email",
            Amount = 1,
            Currency = "USD"
        };

        var res = v.TestValidate(req);

        res.IsValid.Should().BeFalse();
        res.Errors.Should().Contain(e => e.PropertyName == nameof(CreatePaymentRequest.Email));
    }

    [Fact]
    public void Validator_Should_Fail_ForNonPositiveAmount()
    {
        var v = new CreatePaymentValidator();

        var req = new CreatePaymentRequest
        {
            WalletNumber = "w",
            Account = "a",
            Email = "t@test.com",
            Amount = 0,
            Currency = "USD"
        };

        var res = v.TestValidate(req);

        res.IsValid.Should().BeFalse();
        res.Errors.Should().Contain(e => e.PropertyName == nameof(CreatePaymentRequest.Amount));
    }

    [Fact]
    public void Validator_Should_Fail_ForUnsupportedCurrency()
    {
        var v = new CreatePaymentValidator();

        var req = new CreatePaymentRequest
        {
            WalletNumber = "w",
            Account = "a",
            Email = "t@test.com",
            Amount = 1,
            Currency = "JPY"
        };

        var res = v.TestValidate(req);

        res.IsValid.Should().BeFalse();
        res.Errors.Should().Contain(e => e.PropertyName == nameof(CreatePaymentRequest.Currency));
    }
}