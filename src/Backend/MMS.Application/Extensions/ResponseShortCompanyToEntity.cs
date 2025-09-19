using MMS.Communication.Responses.Company;
using MMS.Domain.Entities;

namespace MMS.Application.Extensions;

internal static class ResponseShortCompanyToEntity
{
    public static ShortCompany ToShortCompany(this ResponseShortCompany response)
    {
        return new ShortCompany
        {
            DoingBusinessAs = response.DoingBusinessAs, SubscriptionStatus = response.SubscriptionStatus
        };
    }

    public static ResponseShortCompany ToResponse(this ShortCompany company)
    {
        return new ResponseShortCompany
        {
            DoingBusinessAs = company.DoingBusinessAs,
            SubscriptionPlan = company.SubscriptionPlan!,
            SubscriptionStatus = company.SubscriptionStatus,
            ManagerName = company.ManagerName!
        };
    }
}
