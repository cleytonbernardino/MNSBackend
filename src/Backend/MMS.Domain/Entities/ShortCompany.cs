namespace MMS.Domain.Entities;

public class ShortCompany
{
    public long Id { get; set; }
    public string DoingBusinessAs { get; set; } = string.Empty;
    public bool SubscriptionStatus { get; set; } = false;
}
