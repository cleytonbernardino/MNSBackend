namespace MMS.Domain.Entities;

public class ShortCompany
{
    public long Id { get; set; }
    public bool Active { get; set; } = false;
    public string DoingBusinessAs { get; set; } = string.Empty;
    public string? SubscriptionPlan { get; set; } = string.Empty;
    public string? ManagerName { get; set; }
}
