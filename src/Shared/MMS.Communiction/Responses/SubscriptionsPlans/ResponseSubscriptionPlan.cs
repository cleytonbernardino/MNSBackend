namespace MMS.Communication.Responses.SubscriptionsPlans;

public class ResponseSubscriptionPlan
{
    public string Id { get; set; } = string.Empty;
    public bool Active { get; set; } = false;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Price { get; set; }
}
