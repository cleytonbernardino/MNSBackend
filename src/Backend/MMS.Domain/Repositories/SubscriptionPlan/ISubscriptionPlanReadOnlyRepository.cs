using MMS.Domain.Entities;

namespace MMS.Domain.Repositories.SubscriptionPlan;

public interface ISubscriptionPlanReadOnlyRepository
{
    Task<SubscriptionsPlan[]> List();
}
