using MMS.Communication.Responses.SubscriptionsPlans;

namespace MMS.Application.UseCases.SubscriptionPlan.List;

public interface IListSubscriptionPlanUseCase
{
    Task<ResponseListSubscriptionPlans> Execute();
}
