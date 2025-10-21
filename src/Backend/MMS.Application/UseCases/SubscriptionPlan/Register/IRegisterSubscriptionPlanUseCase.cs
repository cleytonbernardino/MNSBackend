using MMS.Communication.Requests.SubscriptionsPlans;

namespace MMS.Application.UseCases.SubscriptionPlan.Register;

public interface IRegisterSubscriptionPlanUseCase
{
    public Task Execute(RequestRegisterSubscriptionPlan request);
}
