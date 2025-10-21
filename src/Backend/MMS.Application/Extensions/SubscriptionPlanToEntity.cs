using MMS.Communication.Requests.SubscriptionsPlans;
using MMS.Communication.Responses.SubscriptionsPlans;
using MMS.Domain.Entities;

namespace MMS.Application.Extensions;

public static class SubscriptionPlanToEntity
{
    public static SubscriptionsPlan ToEntity(this RequestRegisterSubscriptionPlan request)
    {
        return new SubscriptionsPlan
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price
        };
    }
    
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

    public static SubscriptionsPlan Update(this SubscriptionsPlan entity, RequestUpdateSubscriptionPlan request)
    {
        return new SubscriptionsPlan
        {
            Active = request.Active ?? entity.Active,
            Name = request.Name ?? entity.Name,
            Description = request.Description ?? entity.Description,
            Price = request.Price ?? entity.Price
        };
    }
}
