using MMS.Communication.Requests.Company;
using MMS.Domain.Entities;

namespace MMS.Application.Extensions;

public static class CompanyRequestToCompany
{
    public static Company ToCompany(this RequestRegisterCompany request)
    {
        Company company = new()
        {
            CNPJ = request.CNPJ,
            LegalName = request.LegalName,
            DoingBusinessAs = request.DoingBusinessAs ?? string.Empty,
            CEP = request.CEP,
            Address = request.Address,
            AddressNumber = request.AddressNumber,
            BusinessEmail = request.BusinessEmail,
            PhoneNumber = request.PhoneNumber
        };

        return company;
    }

    public static Company Join(this Company company, RequestUpdateCompany request)
    {
        company.DoingBusinessAs = request.DoingBusinessAs;
        company.CEP = request.CEP;
        company.Address = request.Address;
        company.AddressNumber = request.AddressNumber;
        company.BusinessEmail = request.BusinessEmail;
        company.PhoneNumber = request.PhoneNumber;
        company.WhatsappAPINumber = request.WhatsappApiNumber;
        company.Website = request.WebSite;

        return company;
    }
}
