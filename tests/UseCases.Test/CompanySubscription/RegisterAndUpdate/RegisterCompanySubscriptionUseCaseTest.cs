using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Company;
using CommonTestUtilities.Repositories.CompanySubscription;
using CommonTestUtilities.Repositories.SubscriptionPlan;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services.LoggedUser;
using Microsoft.Extensions.Logging.Abstractions;
using MMS.Application.UseCases.CompanySubscription.RegisterAndUpdate;
using MMS.Exceptions;
using MMS.Exceptions.ExceptionsBase;
using Shouldly;
using Entity = MMS.Domain.Entities;

namespace UseCases.Test.CompanySubscription.RegisterAndUpdate;

public class RegisterCompanySubscriptionUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build(isAdmin: true);
        var request = RequestRegisterCompanySubscriptionBuilder.Build();
        
        var useCase = CreateUseCase(user);
        async Task Act() => await useCase.Execute(request);

        await Act().ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_Validator()
    {
        var user = UserBuilder.Build(isAdmin: true);
        
        var request = RequestRegisterCompanySubscriptionBuilder.Build();
        request.CompanyId = string.Empty;

        var useCase = CreateUseCase(user);
        async Task Act() => await useCase.Execute(request);

        var errors = await Act().ShouldThrowAsync<ErrorOnValidationException>();
        errors.ErrorMessages
            .ShouldHaveSingleItem().ShouldBe(ResourceMessagesException.EMPTY_COMPANY_ID);
    }
    
    
    [Fact]
    public async Task Error_No_Permission()
    {
        var user = UserBuilder.Build(isAdmin: false);
        var request = RequestRegisterCompanySubscriptionBuilder.Build();

        var useCase = CreateUseCase(user);
        async Task Act() => await useCase.Execute(request);

        var errors = await Act().ShouldThrowAsync<NoPermissionException>();
        errors.Message.ShouldBe(ResourceMessagesException.NO_PERMISSION);
    }

    [Fact]
    public async Task Error_Company_Not_Found()
    {
        var user = UserBuilder.Build(isAdmin: true);
        var request = RequestRegisterCompanySubscriptionBuilder.Build();

        var useCase = CreateUseCase(user, companyExists: false);
        async Task Act() => await useCase.Execute(request);

        var errors = await Act().ShouldThrowAsync<NotFoundException>();
        errors.Message.ShouldBe(ResourceMessagesException.COMPANY_NOT_FOUND);
    }
    
    [Fact]
    public async Task Error_Subscription_Plan_Not_Found()
    {
        var user = UserBuilder.Build(isAdmin: true);
        var request = RequestRegisterCompanySubscriptionBuilder.Build();

        var useCase = CreateUseCase(user, subscriptionPlanExists: false);
        async Task Act() => await useCase.Execute(request);

        var errors = await Act().ShouldThrowAsync<NotFoundException>();
        errors.Message.ShouldBe(ResourceMessagesException.SUBSCRIPTION_DOES_NOT_EXIST);
    }
    
    [Fact]
    public async Task Error_Subscription_Plan_ID_Not_Valid()
    {
        IdEncoderForTests idEncoder = new();
        
        var user = UserBuilder.Build(isAdmin: true);
        
        var request = RequestRegisterCompanySubscriptionBuilder.Build();
        request.SubscriptionId = idEncoder.Encode(60000);

        var useCase = CreateUseCase(user);
        async Task Act() => await useCase.Execute(request);

        var errors = await Act().ShouldThrowAsync<ErrorOnValidationException>();
        errors.ErrorMessages
            .ShouldHaveSingleItem().ShouldBe(ResourceMessagesException.SUBSCRIPTION_DOES_NOT_EXIST);
    }
    
    private static RegisterCompanySubscriptionUseCase CreateUseCase(
        Entity.User user, bool companyExists = true, bool subscriptionPlanExists = true)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var logger = NullLogger<RegisterCompanySubscriptionUseCase>.Instance;
        var idEncoder = new IdEncoderBuilder().Build();
        var repository = CompanySubscriptionWriteOnlyBuilder.Build();
        var companyReadOnlyRepository = new CompanyReadOnlyRepositoryBuilder().Exists(companyExists).Build();
        var subscriptionPlanReadOnlyRepository = new SubscriptionPlanReadOnlyRepositoryBuilder()
            .Exists(subscriptionPlanExists).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        
        return new RegisterCompanySubscriptionUseCase(
            loggedUser, logger, idEncoder, repository, companyReadOnlyRepository, 
            subscriptionPlanReadOnlyRepository, unitOfWork);
    }
}
