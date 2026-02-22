using Dotnet_Test_Task.Application.Interfaces;
using Dotnet_Test_Task.Application.Services;
using Dotnet_Test_Task.Domain.Payments;
using FluentAssertions;
using Moq;

namespace Dotnet_Test_Task.Tests.Payments;

public class PaymentsServiceTests
{
    private readonly Mock<IPaymentsRepository> _repo = new();
    private readonly PaymentsService _sut;

    public PaymentsServiceTests()
    {
        _sut = new PaymentsService(_repo.Object);
    }

    [Fact]
    public async Task CreateAsync_Should_CreatePayment_WithCreatedStatus_AndCallRepo()
    {
        // arrange
        _repo.Setup(r => r.AddAsync(It.IsAny<Payment>(), It.IsAny<CancellationToken>()))
             .Returns(Task.CompletedTask);

        var cmd = new CreatePaymentCommand(
            WalletNumber: "  wallet123  ",
            Account: "  user1  ",
            Email: "  test@test.com  ",
            Phone: "  +77001234567 ",
            Amount: 10.5m,
            Currency: " usd ",
            Comment: "  hello  ");

        // act
        var payment = await _sut.CreateAsync(cmd, CancellationToken.None);

        // assert
        payment.Id.Should().NotBe(Guid.Empty);
        payment.Status.Should().Be(PaymentStatus.Created);
        payment.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, precision: TimeSpan.FromSeconds(5));

        payment.WalletNumber.Should().Be("wallet123");
        payment.Account.Should().Be("user1");
        payment.Email.Should().Be("test@test.com");
        payment.Phone.Should().Be("+77001234567");
        payment.Currency.Should().Be("USD");
        payment.Comment.Should().Be("hello");
        payment.Amount.Should().Be(10.5m);
        
        _repo.Verify(r => r.AddAsync(It.Is<Payment>(p => p.Id == payment.Id), It.IsAny<CancellationToken>()), Times.Once);
        _repo.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task CreateAsync_Should_SetPhoneAndCommentNull_WhenWhitespace()
    {
        _repo.Setup(r => r.AddAsync(It.IsAny<Payment>(), It.IsAny<CancellationToken>()))
             .Returns(Task.CompletedTask);

        var cmd = new CreatePaymentCommand(
            WalletNumber: "w",
            Account: "a",
            Email: "e@e.com",
            Phone: "   ",
            Amount: 1m,
            Currency: "EUR",
            Comment: null);

        var payment = await _sut.CreateAsync(cmd, CancellationToken.None);

        payment.Phone.Should().BeNull();
        payment.Comment.Should().BeNull();
    }

    [Fact]
    public async Task GetPaymentsAsync_Should_Proxy_ToRepository()
    {
        var expected = new List<Payment>
        {
            new() { Id = Guid.NewGuid(), WalletNumber = "w", Account = "a", Email = "e@e.com", Amount = 1, Currency = "USD", Status = PaymentStatus.Created, CreatedAt = DateTime.UtcNow }
        };

        _repo.Setup(r => r.GetAllAsync(2, 50, false, It.IsAny<CancellationToken>()))
             .ReturnsAsync(expected);

        var result = await _sut.GetPaymentsAsync(new GetPaymentsQuery(Page: 2, PageSize: 50, SortByCreatedAtDesc: false), CancellationToken.None);

        result.Should().BeSameAs(expected);

        _repo.Verify(r => r.GetAllAsync(2, 50, false, It.IsAny<CancellationToken>()), Times.Once);
        _repo.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task GetStatsAsync_Should_ComposeTotalsAndByDays()
    {
        _repo.Setup(r => r.GetTotalsAsync(It.IsAny<CancellationToken>()))
             .ReturnsAsync((TotalAmount: 123.45m, TotalCount: 7L));

        var from = new DateOnly(2026, 2, 1);
        var to = new DateOnly(2026, 2, 2);
        const int tzOffsetMinutes = -300;

        var byDays = new List<DailyPaymentsStats>
        {
            new(new DateOnly(2026, 2, 1), 2, 10m),
            new(new DateOnly(2026, 2, 2), 5, 113.45m)
        };

        _repo.Setup(r => r.GetDailyStatsAsync(from, to, tzOffsetMinutes, It.IsAny<CancellationToken>()))
             .ReturnsAsync(byDays);

        var result = await _sut.GetStatsAsync(new GetPaymentsStatsQuery(from, to, tzOffsetMinutes), CancellationToken.None);

        result.TotalAmount.Should().Be(123.45m);
        result.TotalCount.Should().Be(7);
        result.ByDays.Should().BeSameAs(byDays);

        _repo.Verify(r => r.GetTotalsAsync(It.IsAny<CancellationToken>()), Times.Once);
        _repo.Verify(r => r.GetDailyStatsAsync(from, to, tzOffsetMinutes, It.IsAny<CancellationToken>()), Times.Once);
        _repo.VerifyNoOtherCalls();
    }
}