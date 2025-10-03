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
        return new Company
        {
            DoingBusinessAs = request.DoingBusinessAs,
            CEP = request.CEP, 
            Address = request.Address,
            AddressNumber = request.AddressNumber,
            BusinessEmail = request.BusinessEmail,
            PhoneNumber = request.PhoneNumber,
            WhatsappAPINumber = request.WhatsappApiNumber,
            Website = request.WebSite
        };
    }
}
