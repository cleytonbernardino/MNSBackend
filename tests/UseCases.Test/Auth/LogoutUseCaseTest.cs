using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Microsoft.Extensions.Logging.Abstractions;
using MMS.Application.UseCases.Auth.Logout;
using Shouldly;

namespace UseCases.Test.Auth;

public class LogoutUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var userIdentifier = Guid.NewGuid();
        var request = RequestRefreshTokenBuilder.Build();
        
        var useCase = CreateUseCase(userIdentifier);

        async Task act() => await useCase.Execute(request, "token");

        await act().ShouldNotThrowAsync();
    }
    
    private static LogoutUseCase CreateUseCase(Guid userIdentifier)
    {
        var refreshTokenHandler = new RefreshTokenHandlerBuilder()
            .ValidateAccessTokenAndGetData(userIdentifier)
            .Delete(userIdentifier);
        var logger = NullLogger.Instance;
        
        return new LogoutUseCase(refreshTokenHandler.Build(), logger);
    }
}
