using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services.LoggedUser;
using Microsoft.Extensions.Logging.Abstractions;
using MMS.Application.UseCases.Company.Register;
using MMS.Exceptions;
using MMS.Exceptions.ExceptionsBase;
using Shouldly;
using Entity = MMS.Domain.Entities;

namespace UseCases.Test.Company.Register;

public class RegisterCompanyUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        user.IsAdmin = true;

        var request = RequestRegisterCompanyBuilder.Build();

        var useCase = CreateUseCase(user);
        async Task act() => await useCase.Execute(request);

        await act().ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_Legal_Name_Empty()
    {
        var user = UserBuilder.Build();
        user.IsAdmin = true;

        var request = RequestRegisterCompanyBuilder.Build();
        request.LegalName = string.Empty;

        var useCase = CreateUseCase(user);
        async Task act() => await useCase.Execute(request);

        var errors = await act().ShouldThrowAsync<ErrorOnValidationException>();
        errors.ErrorMessages
            .Single()
            .ShouldBe(ResourceMessagesException.LEGAL_NAME_EMPTY);
    }

    [Fact]
    public async Task Error_Not_An_Administrator()
    {
        var user = UserBuilder.Build();
        user.IsAdmin = false;

        var request = RequestRegisterCompanyBuilder.Build();
        request.LegalName = string.Empty;

        var useCase = CreateUseCase(user);
        async Task act() => await useCase.Execute(request);

        var errors = await act().ShouldThrowAsync<NoPermissionException>();
        errors.Message
            .ShouldBe(ResourceMessagesException.NO_PERMISSION);
    }

    private static RegisterCompanyUseCase CreateUseCase(Entity.User user)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = CompanyWriteOnlyRepositoryBuilder.Build();
        var unityOfWork = UnitOfWorkBuilder.Build();
        var logger = NullLogger<RegisterCompanyUseCase>.Instance;

        return new RegisterCompanyUseCase(loggedUser, repository, unityOfWork, logger);
    }
}
