namespace MMS.Communication.Requests.Company;

public record RequestRegisterSubscriptionPlan
{
    public bool Active { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Price { get; set; }
}
