using MMS.Domain.Entities;

namespace MMS.Domain.Repositories.SubscriptionPlan;

public interface ISubscriptionPlanUpdateOnlyRepository
{
    Task<SubscriptionsPlan?> GetById(short id);
    void Update(SubscriptionsPlan subscriptionsPlan);
}
