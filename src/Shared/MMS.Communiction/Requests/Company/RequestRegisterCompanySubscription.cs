namespace MMS.Communication.Requests.Company;

public class RequestRegisterCompanySubscription
{
    public string CompanyId { get; set; } = string.Empty;
    public string SubscriptionId { get; set; } = string.Empty;
    public bool IsBillingAnnual { get; set; } = false;
}
