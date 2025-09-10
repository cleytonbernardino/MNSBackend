namespace MMS.Domain.Entities;

public class Company : EntityBase
{
    public DateTime UpdatedOn { get; set; }
    public string CNPJ { get; set; } = string.Empty;
    public string LegalName { get; set; } = string.Empty;
    public string? DoingBusinessAs { get; set; }
    public string? BusinessSector { get; set; }
    public string CEP { get; set; } = string.Empty;
    public string AddressNumber { get; set; } = string.Empty;
    public string? BusinessEmail { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string? WhatsappAPINumber { get; set; }
    public long? ManagerId { get; set; }
    public Int16? SubscriptionPlan { get; set; }
    public bool SubscriptionStatus { get; set; }
    public string? Website { get; set; }
}
