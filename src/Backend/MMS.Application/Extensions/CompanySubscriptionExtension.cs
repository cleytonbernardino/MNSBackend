using MMS.Communication.Requests.Company;
using MMS.Domain.Entities;

namespace MMS.Application.Extensions;

public static class CompanySubscriptionExtension
{
    public static CompanySubscription ToEntity(this RequestRegisterCompanySubscription request)
    {
        return new CompanySubscription { IsBillingAnnual = request.IsBillingAnnual };
    }
}
