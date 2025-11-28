using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.ServiceDefinition;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Requests.ServicesDefinition;
using CommonTestUtilities.Services.LoggedUser;
using MMS.Application.UseCases.ServiceDefinition.Register;
using MMS.Exceptions;
using MMS.Exceptions.ExceptionsBase;
using Shouldly;
using Entity = MMS.Domain.Entities;

namespace UseCases.Test.ServiceDefinition.Register;

public class RegisterUserServicesUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterServiceDefinitionBuilder.Build();

        var user = UserBuilder.Build();

        var useCase = CreateUseCase(user);
        async Task Act() => await useCase.Execute(request);

        await Act().ShouldNotThrowAsync();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Success_No_Description(string? description)
    {
        var request = RequestRegisterServiceDefinitionBuilder.Build();
        request.Description = description;

        var user = UserBuilder.Build();

        var useCase = CreateUseCase(user);
        async Task Act() => await useCase.Execute(request);

        await Act().ShouldNotThrowAsync();
    }
    
    [Fact]
    public async Task Success_No_Status()
    {
        var request = RequestRegisterServiceDefinitionBuilder.Build();
        request.Status = null;

        var user = UserBuilder.Build();

        var useCase = CreateUseCase(user);
        async Task Act() => await useCase.Execute(request);

        await Act().ShouldNotThrowAsync();
    }
    
    [Fact]
    public async Task Error_Validator()
    {
        var request = RequestRegisterServiceDefinitionBuilder.Build();
        request.Title = string.Empty;

        var user = UserBuilder.Build();

        var useCase = CreateUseCase(user);
        async Task Act() => await useCase.Execute(request);

        var errors = await Act().ShouldThrowAsync<ErrorOnValidationException>();
        errors.ErrorMessages.ShouldHaveSingleItem().ShouldBe(ResourceMessagesException.EMPTY_TITLE);
    }
    
    private static RegisterServicesUseCase CreateUseCase(Entity.User user)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = ServiceDefinitionWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        
        return new RegisterServicesUseCase(loggedUser, repository, unitOfWork);
    }
}
