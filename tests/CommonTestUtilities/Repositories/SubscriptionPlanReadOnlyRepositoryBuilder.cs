using MMS.Domain.Repositories.SubscriptionPlan;
using Moq;
using Entity = MMS.Domain.Entities;

namespace CommonTestUtilities.Repositories;

public class SubscriptionPlanReadOnlyRepositoryBuilder
{
    private readonly Mock<ISubscriptionPlanReadOnlyRepository> _mock = new();

    public SubscriptionPlanReadOnlyRepositoryBuilder List(Entity.SubscriptionsPlan[] plans)
    {
        _mock.Setup(mock => mock.List()).ReturnsAsync(plans);
        return this;
    }

    public ISubscriptionPlanReadOnlyRepository Build() => _mock.Object;
}
