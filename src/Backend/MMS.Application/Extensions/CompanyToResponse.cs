using MMS.Communication.Responses.Company;
using MMS.Domain.Entities;

namespace MMS.Application.Extensions;

public static class CompanyToResponse
{
    public static ResponseCompany ToResponse(this Company company)
    {
        return new ResponseCompany
        {
            CreatedOn = company.CreatedOn,
            UpdatedOn = company.UpdatedOn,
            Active = company.Active,
            CNPJ = company.CNPJ,
            LegalName = company.LegalName,
            DoingBusinessAs = company.DoingBusinessAs,
            BusinessSector = company.BusinessSector,
            CEP = company.CEP,
            AddressNumber = company.AddressNumber,
            BusinessEmail = company.BusinessEmail,
            PhoneNumber = company.PhoneNumber,
            Manager = company.Manager is null ? "" : company.Manager!.FirstName,
            SubscriptionStatus = company.SubscriptionStatus,
            Website = company.Website
        };
    }
}
