using MMS.Communication;
using MMS.Domain.Entities;

namespace MMS.Application.Extensions;

public static class CompanyRequestToCompany
{
    public static Company ToCompany(this RequestRegisterCompany request)
    {
        Company company = new()
        {
            CNPJ = request.CNJP,
            LegalName = request.LegalName,
            DoingBusinessAs = request.DoingBusinessAs ?? string.Empty,
            CEP = request.CEP,
            AddressNumber = request.AddressNumber,
            BusinessEmail = request.BusinessEmail,
            PhoneNumber = request.PhoneNumber,
            Website = request.Website
        };

        return company;
    }
}
