namespace MMS.Communication.Requests.SubscriptionsPlans;

public class RequestUpdateSubscriptionPlan
{
    public string? Id { get; set; }
    public bool? Active { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string[] Properties { get; set; } = [];
    public double? Price { get; set; }
}
