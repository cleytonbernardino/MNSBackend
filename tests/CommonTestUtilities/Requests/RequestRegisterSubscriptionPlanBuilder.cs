using Bogus;
using MMS.Communication.Requests.SubscriptionsPlans;

namespace CommonTestUtilities.Requests;

public static class RequestRegisterSubscriptionPlanBuilder
{
    public static RequestRegisterSubscriptionPlan Build()
    {
        return new Faker<RequestRegisterSubscriptionPlan>()
            .RuleFor(request => request.Active, true)
            .RuleFor(request => request.Name, f => f.Lorem.Word())
            .RuleFor(request => request.Description, f => f.Lorem.Paragraph())
            .RuleFor(request => request.Properties, f => f.Lorem.Words(3))
            .RuleFor(request => request.Price, f => double.Parse(f.Commerce.Price()));
    }
}
