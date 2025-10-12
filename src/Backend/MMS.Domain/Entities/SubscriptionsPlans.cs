namespace MMS.Domain.Entities;

public class SubscriptionsPlans : EntityBase<short>
{
    public string Name { get; set; } = string.Empty;
    public bool IsBillingAnnual { get; set; }
    public short PaymentStatus { get; set; }
    public DateTime NextBillingDate { get; set; }
    public short PaymentMethod { get; set; }
}
