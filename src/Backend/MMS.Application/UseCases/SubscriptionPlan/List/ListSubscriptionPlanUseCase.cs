using MMS.Application.Extensions;
using MMS.Application.Services.Encoders;
using MMS.Communication.Responses.SubscriptionsPlans;
using MMS.Domain.Repositories.SubscriptionPlan;
using MMS.Domain.Services.Cache;

namespace MMS.Application.UseCases.SubscriptionPlan.List;

public class ListSubscriptionPlanUseCase(
    ISubscriptionPlanReadOnlyRepository repository,
    IIdEncoder idEncoder,
    ICacheService cacheService
    ) : IListSubscriptionPlanUseCase
{
    private readonly ICacheService _cacheService = cacheService;
    private readonly ISubscriptionPlanReadOnlyRepository _repository = repository;
    private readonly IIdEncoder _idEncoder = idEncoder;

    private const string CACHE_KEY = "SubscriptionPlans";
    
    public async Task<ResponseListSubscriptionPlans> Execute()
    {
        var cache = await _cacheService.GetCache<ResponseListSubscriptionPlans>(CACHE_KEY);
        if (cache is not null)
            return cache;
        
        var result = await _repository.List();

        var subscriptionPlans = result.ToResponse();
        foreach (var subscriptionPlan in subscriptionPlans.SubscriptionPlans)
        {
            subscriptionPlan.Id = _idEncoder.Encode(long.Parse(subscriptionPlan.Id));
        }

        await _cacheService.SaveCache(CACHE_KEY, subscriptionPlans, expirationTime: 0);
        return subscriptionPlans;
    }
}
