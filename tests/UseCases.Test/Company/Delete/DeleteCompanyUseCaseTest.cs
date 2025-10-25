using CommonTestUtilities.Cache;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Company;
using CommonTestUtilities.Services.LoggedUser;
using Microsoft.Extensions.Logging.Abstractions;
using MMS.Application.UseCases.Company.Delete;
using MMS.Exceptions;
using MMS.Exceptions.ExceptionsBase;
using Shouldly;
using Entity = MMS.Domain.Entities;

namespace UseCases.Test.Company.Delete;

public class DeleteCompanyUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        user.IsAdmin = true;
        
        var useCase = CreateUseCase(user);
        async Task Act() => await useCase.Execute(1);

        await Act().ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_User_No_Permission()
    {
        var user = UserBuilder.Build();
        
        var useCase = CreateUseCase(user);
        async Task Act() => await useCase.Execute(1);

        var errors = await Act().ShouldThrowAsync<NoPermissionException>();
        errors.Message.ShouldBe(ResourceMessagesException.NO_PERMISSION);
    }
    
    private static DeleteCompanyUseCase CreateUseCase(Entity.User user)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = CompanyWriteOnlyRepositoryBuilder.Build();
        var cacheService = new CacheServiceBuilder().Build();
        var unityOfWork = UnitOfWorkBuilder.Build();
        var logger = NullLogger<DeleteCompanyUseCase>.Instance;
        
        return new DeleteCompanyUseCase(loggedUser, repository, cacheService, unityOfWork, logger);
    }
}
