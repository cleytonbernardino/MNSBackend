using MMS.Communication.Requests.SubscriptionsPlans;

namespace MMS.Application.UseCases.SubscriptionPlan.Update;

public interface IUpdateSubscriptionPlanUseCase
{
    Task Execute(RequestUpdateSubscriptionPlan request);
}
