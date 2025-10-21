namespace MMS.Domain.Entities;

public class SubscriptionsPlan : EntityBase<short>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Price { get; set; }
}
