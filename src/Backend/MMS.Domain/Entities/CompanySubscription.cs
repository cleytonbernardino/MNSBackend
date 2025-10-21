namespace MMS.Domain.Entities;

public class CompanySubscription : EntityBase
{
    public long CompanyId { get; set; }
    public SubscriptionsPlan SubscriptionPlan { get; set; }
    public bool IsBillingAnnual { get; set; }
    public short PaymentStatus { get; set; }
    public DateTime NextBillingDate { get; set; }
    public short PaymentMethod { get; set; }
}
