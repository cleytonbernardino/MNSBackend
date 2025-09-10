namespace MMS.Domain.Entities;

public record RefreshToken
{
    public long Id { get; init; }
    public Guid UserIdentifier { get; set; }
    public string? Token { get; set; }
    public DateTime? ExpiryDate { get; set; }
}
