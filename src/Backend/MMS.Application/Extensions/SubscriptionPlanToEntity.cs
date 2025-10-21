using MMS.Communication.Requests.SubscriptionsPlans;
using MMS.Communication.Responses.SubscriptionsPlans;
using MMS.Domain.Entities;

namespace MMS.Application.Extensions;

public static class SubscriptionPlanToEntity
{
    
    public static ResponseListSubscriptionPlans ToResponse(this SubscriptionsPlan[] entities)
    {
        ResponseListSubscriptionPlans response = new();
        foreach (var entity in entities)
        {
            var subscriptionPlanResponse = CreateSubscriptionPlanResponse(entity);
            response.SubscriptionPlans.Add(subscriptionPlanResponse);
        }
        return response;
    }

}
