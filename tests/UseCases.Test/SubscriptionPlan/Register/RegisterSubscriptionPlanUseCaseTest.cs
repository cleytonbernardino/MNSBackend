using CommonTestUtilities.Cache;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.SubscriptionPlan;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services.LoggedUser;
using Microsoft.Extensions.Logging.Abstractions;
using MMS.Application.UseCases.SubscriptionPlan.Register;
using MMS.Exceptions;
using MMS.Exceptions.ExceptionsBase;
using Shouldly;
using Entity = MMS.Domain.Entities;

namespace UseCases.Test.SubscriptionPlan.Register;

public class RegisterSubscriptionPlanUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        user.IsAdmin = true;
        
        var request = RequestRegisterSubscriptionPlanBuilder.Build();

        var useCase = CreateUseCase(user);
        async Task Act() => await useCase.Execute(request);

        await Act().ShouldNotThrowAsync();
    }
    
    [Fact]
    public async Task Error_No_Permission()
    {
        var user = UserBuilder.Build();
        user.IsAdmin = false;
        
        var request = RequestRegisterSubscriptionPlanBuilder.Build();

        var useCase = CreateUseCase(user);
        async Task Act() => await useCase.Execute(request);

        var errors = await Act().ShouldThrowAsync<NoPermissionException>();
        errors.Message.ShouldBe(ResourceMessagesException.NO_PERMISSION);
    }

    [Fact]
    public async Task Error_Validator()
    {
        var user = UserBuilder.Build();
        user.IsAdmin = true;
        
        var request = RequestRegisterSubscriptionPlanBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase(user);
        async Task Act() => await useCase.Execute(request);

        var errors = await Act().ShouldThrowAsync<ErrorOnValidationException>();
        errors.ErrorMessages.ShouldHaveSingleItem().ShouldBe(ResourceMessagesException.NAME_CANNOT_BE_EMPTY);
    }

    private static RegisterSubscriptionPlan CreateUseCase(Entity.User user)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var logger = NullLogger<RegisterSubscriptionPlan>.Instance;
        var repository = SubscriptionPlanWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var cacheService = new CacheServiceBuilder().Build();
        
        return new RegisterSubscriptionPlan(loggedUser, repository, logger, unitOfWork, cacheService);
    }
}
