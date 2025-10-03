namespace MMS.Communication.Responses.Company;

public class ResponseCompany
{
    public string Id { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    public bool Active { get; set; }
    public string CNPJ { get; set; } = string.Empty;
    public string LegalName { get; set; } = string.Empty;
    public string? DoingBusinessAs { get; set; }
    public string? BusinessSector { get; set; }
    public string CEP { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string AddressNumber { get; set; } = string.Empty;
    public string? BusinessEmail { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    //public string? WhatsappAPINumber { get; set; }
    public string Manager { get; set; } = string.Empty;
    //public CompanySubscription? CompanySubscription { get; set; }
    public bool SubscriptionStatus { get; set; }
    public string? Website { get; set; }
}
