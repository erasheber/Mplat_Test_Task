namespace Dotnet_Test_Task.Domain.Payments;

public class Payment
{
    public Guid Id { get; set; }

    public string WalletNumber { get; set; } = null!;
    public string Account { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }

    public decimal Amount { get; set; }
    public string Currency { get; set; } = null!;
    public string? Comment { get; set; }

    public PaymentStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}