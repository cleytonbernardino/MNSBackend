using Entity = MMS.Domain.Entities;

namespace MMS.Domain.Repositories.SubscriptionPlan;

public interface ISubscriptionPlanWriteOnlyRepository
{
    Task Register(Entity.SubscriptionsPlan subscriptionPlan);
}
