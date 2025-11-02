using CommonTestUtilities.Cache;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.SubscriptionPlan;
using MMS.Application.UseCases.SubscriptionPlan.List;
using Shouldly;
using Entity = MMS.Domain.Entities;

namespace UseCases.Test.SubscriptionPlan.List;

public class ListSubscriptionPlansUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var plans = SubscriptionPlanBuilder.BuildInBatch();

        var useCase = CreateUseCase(plans);
        async Task Act() => await useCase.Execute();

        await Act().ShouldNotThrowAsync();
    }
    
    private static ListSubscriptionPlanUseCase CreateUseCase(Entity.SubscriptionsPlan[] plansToMock)
    {
        var cacheService = new CacheServiceBuilder().Build();
        var repository = new SubscriptionPlanReadOnlyRepositoryBuilder().List(plansToMock).Build();
        var idEncoder = new IdEncoderBuilder().Build();
        
        return new ListSubscriptionPlanUseCase(
            repository: repository, idEncoder: idEncoder, cacheService: cacheService
            );
    }
}
