using MMS.Domain.Repositories.SubscriptionPlan;
using Moq;
using Entity = MMS.Domain.Entities;

namespace CommonTestUtilities.Repositories.SubscriptionPlan;

public class SubscriptionPlanUpdateOnlyRepositoryBuilder
{
    private readonly Mock<ISubscriptionPlanUpdateOnlyRepository> _mock = new();

    public SubscriptionPlanUpdateOnlyRepositoryBuilder GetById(Entity.SubscriptionsPlan plan)
    {
        _mock.Setup(mock => mock.GetById(plan.Id)).ReturnsAsync(plan);
        return this;
    }
    
    public ISubscriptionPlanUpdateOnlyRepository Build() => _mock.Object;
}
