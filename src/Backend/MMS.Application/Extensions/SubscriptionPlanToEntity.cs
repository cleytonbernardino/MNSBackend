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
            Properties = request.Properties.ToList(),
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
        var result = new SubscriptionsPlan
        {
            Active = request.Active ?? entity.Active,
            Name = request.Name ?? entity.Name,
            Description = request.Description ?? entity.Description,
            Properties = entity.Properties,
            Price = request.Price ?? entity.Price
        };
        if (request.Properties.Length != 0)
            result.Properties = request.Properties.ToList();
        return result;
    }
    
    private static ResponseSubscriptionPlan CreateSubscriptionPlanResponse(SubscriptionsPlan subscriptionsPlan)
    {
        return new ResponseSubscriptionPlan
        {
            Id = subscriptionsPlan.Id.ToString(),
            Active = subscriptionsPlan.Active,
            Name = subscriptionsPlan.Name,
            Description = subscriptionsPlan.Description,
            Properties = subscriptionsPlan.Properties.ToArray(),
            Price = subscriptionsPlan.Price
        };
    }
}
