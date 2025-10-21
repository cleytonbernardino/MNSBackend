using Bogus;
using MMS.Domain.Entities;

namespace CommonTestUtilities.Entities;

public static class SubscriptionPlanBuilder
{
    public static SubscriptionsPlan Build()
    {
        return new Faker<SubscriptionsPlan>()
            .RuleFor(plan => plan.Id, (short)0)
            .RuleFor(plan => plan.Active, true)
            .RuleFor(plan => plan.Name, f => f.Commerce.ProductName())
            .RuleFor(plan => plan.Description, f => f.Commerce.ProductDescription())
            .RuleFor(plan => plan.Price, f => f.Random.Double(0, 10000));
    }

    public static SubscriptionsPlan[] BuildInBatch(ushort count = 5)
    {
        SubscriptionsPlan[] plans = new SubscriptionsPlan[count];

        for (ushort i = 0; i < count; i++)
        {
            var plan = Build();
            plans[i] = plan;
        }

        return plans;
    }
}
