namespace MMS.Communication.Responses.Company;

public record ResponseShortCompanies
{
    public List<ResponseShortCompany> Companies { get; set; } = [];
}

public record ResponseShortCompany
{
    public string Id { get; set; } = string.Empty;
    public bool Active { get; set; } = false;
    public string DoingBusinessAs { get; set; } = string.Empty;
    public string ManagerName { get; set; } = string.Empty;
    public string SubscriptionPlan { get; set; } = string.Empty;
}
