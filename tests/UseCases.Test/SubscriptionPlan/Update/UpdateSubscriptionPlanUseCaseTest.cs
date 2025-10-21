using CommonTestUtilities.Cache;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services.LoggedUser;
using Microsoft.Extensions.Logging.Abstractions;
using MMS.Application.UseCases.SubscriptionPlan.Update;
using MMS.Exceptions;
using MMS.Exceptions.ExceptionsBase;
using Shouldly;
using Entity = MMS.Domain.Entities;

namespace UseCases.Test.SubscriptionPlan.Update;

public class UpdateSubscriptionPlanUseCaseTest
{
    private readonly IdEncoderForTests _idEncoder = new();
    
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build(isAdmin: true);
        var subscriptionPlan = SubscriptionPlanBuilder.Build();

        var request = RequestUpdateSubscriptionPlanBuilder.Build();
        request.Id = _idEncoder.Encode(subscriptionPlan.Id);
        
        var useCase = CreateUseCase(user, subscriptionPlan);
        async Task Act() => await useCase.Execute(request);

        await Act().ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_No_Permission()
    {
        var user = UserBuilder.Build(isAdmin: false);
        var subscriptionPlan = SubscriptionPlanBuilder.Build();

        var request = RequestUpdateSubscriptionPlanBuilder.Build();
        
        var useCase = CreateUseCase(user, subscriptionPlan);
        async Task Act() => await useCase.Execute(request);

        var errors = await Act().ShouldThrowAsync<NoPermissionException>();
        errors.Message.ShouldBe(ResourceMessagesException.NO_PERMISSION);
    }

    [Fact]
    public async Task Error_Subscription_Plan_Not_Found()
    {
        var user = UserBuilder.Build(isAdmin: true);
        var subscriptionPlan = SubscriptionPlanBuilder.Build();
        subscriptionPlan.Id = 1;

        var request = RequestUpdateSubscriptionPlanBuilder.Build();
        request.Id = _idEncoder.Encode(subscriptionPlan.Id + 1);
        
        var useCase = CreateUseCase(user, subscriptionPlan);
        async Task Act() => await useCase.Execute(request);

        var errors = await Act().ShouldThrowAsync<NotFoundException>();
        errors.Message.ShouldBe(ResourceMessagesException.PLAN_NOT_FOUND);
    }
    
    
    private static UpdateSubscriptionPlanUseCase CreateUseCase(Entity.User user, Entity.SubscriptionsPlan planToMock)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new SubscriptionPlanUpdateOnlyRepositoryBuilder().GetById(planToMock);
        var idEncoder = new IdEncoderBuilder().Build();
        var logger = NullLogger<UpdateSubscriptionPlanUseCase>.Instance;
        var unitOfWork = UnitOfWorkBuilder.Build();
        var cacheServices = new CacheServiceBuilder().Build();
        
        return new UpdateSubscriptionPlanUseCase(
            loggedUser, repository.Build(), idEncoder, logger, unitOfWork, cacheServices);
    }
}
