namespace MMS.Communication.Responses.Company;

public record ResponseRegisterCompany
{
    public ResponseShortCompanies Users { get; set; }
}
