namespace MMS.Communication.Requests.Company;

public record RequestRegisterCompanySubscription
{
    public string CompanyId { get; set; } = string.Empty;
    public string SubscriptionId { get; set; } = string.Empty;
    public bool IsBillingAnnual { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    public DateTime NextBillingDate { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
}
