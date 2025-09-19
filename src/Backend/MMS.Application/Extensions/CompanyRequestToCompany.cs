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
            AddressNumber = request.AddressNumber,
            BusinessEmail = request.BusinessEmail,
            PhoneNumber = request.PhoneNumber
        };

        return company;
    }
}
