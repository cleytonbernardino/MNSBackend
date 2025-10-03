namespace MMS.Communication.Requests.Company;

public record RequestRegisterCompany
{
    public string CNPJ { get; set; } = string.Empty;
    public string LegalName { get; set; } = string.Empty;
    public string? DoingBusinessAs { get; set; } = string.Empty;
    public string CEP { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string AddressNumber { get; set; } = string.Empty;
    public string? BusinessEmail { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string? WhatsappApiNumber { get; set; } = string.Empty;
    public string? ManagerId { get; set; } = string.Empty;
    public string? WebSite { get; set; } = string.Empty;
}
