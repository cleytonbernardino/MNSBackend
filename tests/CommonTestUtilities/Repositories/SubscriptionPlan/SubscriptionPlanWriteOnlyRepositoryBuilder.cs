using MMS.Domain.Entities;
using MMS.Domain.Repositories.SubscriptionPlan;
using Moq;

namespace CommonTestUtilities.Repositories.SubscriptionPlan;

public static class SubscriptionPlanWriteOnlyRepositoryBuilder
{
    public static ISubscriptionPlanWriteOnlyRepository Build()
    {
        var mock = new Mock<ISubscriptionPlanWriteOnlyRepository>();
        mock.Setup(moc => moc.Register(It.IsAny<SubscriptionsPlan>()));
        return mock.Object;
    }
}
