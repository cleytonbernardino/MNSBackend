using CommonTestUtilities.Cache;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services.LoggedUser;
using Microsoft.Extensions.Logging.Abstractions;
using MMS.Application.UseCases.Company.Update;
using MMS.Exceptions;
using MMS.Exceptions.ExceptionsBase;
using Shouldly;
using Entity = MMS.Domain.Entities;

namespace UseCases.Test.Company.Update;

public class UpdateCompanyUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        user.IsAdmin = true;
        
        var company = CompanyBuilder.Build();
        var request = RequestUpdateCompanyBuilder.Build();
        
        var useCase = CreateUseCase(user, company);
        async Task Act() => await useCase.Execute(request, company.Id);

        await Act().ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_No_Permission()
    {
        var user = UserBuilder.Build();
        var company = CompanyBuilder.Build();
        var request = RequestUpdateCompanyBuilder.Build();
        
        var useCase = CreateUseCase(user, company);
        async Task Act() => await useCase.Execute(request, company.Id);

        var errors = await Act().ShouldThrowAsync<NoPermissionException>();
        errors.Message
            .ShouldBe(ResourceMessagesException.NO_PERMISSION);
    }

    [Fact]
    public async Task Error_Validation_Fail()
    {
        var user = UserBuilder.Build();
        user.IsAdmin = true;
        
        var company = CompanyBuilder.Build();
        
        var request = RequestUpdateCompanyBuilder.Build();
        request.DoingBusinessAs = string.Empty;
        
        var useCase = CreateUseCase(user, company);
        async Task Act() => await useCase.Execute(request, company.Id);

        var errors = await Act().ShouldThrowAsync<ErrorOnValidationException>();
        errors.ErrorMessages
            .ShouldHaveSingleItem()
            .ShouldBe(ResourceMessagesException.DBA_EMPTY);
    }
    
    private static UpdateCompanyUseCase CreateUseCase(Entity.User user, Entity.Company company)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new CompanyUpdateOnlyRepositoryBuilder().GetById(company);
        var cacheService = new CacheServiceBuilder().Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var logger = NullLogger<UpdateCompanyUseCase>.Instance;
        
        return new UpdateCompanyUseCase(loggedUser, repository.Build(), cacheService, unitOfWork, logger);
    }
}
