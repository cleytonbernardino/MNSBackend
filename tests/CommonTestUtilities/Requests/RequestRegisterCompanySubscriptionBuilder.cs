using Bogus;
using MMS.Communication.Requests.Company;

namespace CommonTestUtilities.Requests;

public static class RequestRegisterCompanySubscriptionBuilder
{
    public static RequestRegisterCompanySubscription Build()
    {
        return new Faker<RequestRegisterCompanySubscription>()
            .RuleFor(request => request.CompanyId, "yyy")
            .RuleFor(request => request.SubscriptionId, "yyy")
            .RuleFor(request => request.IsBillingAnnual, false);
    }
}
