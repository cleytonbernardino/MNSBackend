using CommonTestUtilities.Cache;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.SubscriptionPlan;
using CommonTestUtilities.Services.LoggedUser;
using Microsoft.Extensions.Logging.Abstractions;
using MMS.Application.UseCases.SubscriptionPlan.Delete;
using MMS.Exceptions;
using MMS.Exceptions.ExceptionsBase;
using Shouldly;
using Entity = MMS.Domain.Entities;

namespace UseCases.Test.SubscriptionPlan.Delete;

public class DeleteSubscriptionPlanUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build(isAdmin: true);

        var useCase = CreateUseCase(user);
        async Task Act() => await useCase.Execute(id: 1);

        await Act().ShouldNotThrowAsync();
    }
    
    [Fact]
    public async Task Error_No_Permission()
    {
        var user = UserBuilder.Build(isAdmin: false);

        var useCase = CreateUseCase(user);
        async Task Act() => await useCase.Execute(id: 1);

        var errors = await Act().ShouldThrowAsync<NoPermissionException>();
        errors!.Message
            .ShouldBe(ResourceMessagesException.NO_PERMISSION);
    }
    
    private static DeleteSubscriptionPlanUseCase CreateUseCase(Entity.User user)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var logger = NullLogger<DeleteSubscriptionPlanUseCase>.Instance;
        var repository = new SubscriptionPlanUpdateOnlyRepositoryBuilder().Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var cacheServices = new CacheServiceBuilder().Build();
    
        return new DeleteSubscriptionPlanUseCase(loggedUser, logger, repository, unitOfWork, cacheServices);
    }
}
