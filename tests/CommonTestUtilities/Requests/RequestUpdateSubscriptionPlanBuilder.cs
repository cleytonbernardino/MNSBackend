using Bogus;
using MMS.Communication.Requests.SubscriptionsPlans;

namespace CommonTestUtilities.Requests;

public static class RequestUpdateSubscriptionPlanBuilder
{
    public static RequestUpdateSubscriptionPlan Build()
    {
        return new Faker<RequestUpdateSubscriptionPlan>()
            .RuleFor(req => req.Id, "yyy")
            .RuleFor(req => req.Active, true)
            .RuleFor(req => req.Name, f => f.Commerce.ProductName())
            .RuleFor(req => req.Description, f => f.Commerce.ProductDescription())
            .RuleFor(req => req.Price, f => f.Random.Double(0, 10000));

    }
}
