using CommonTestUtilities.Entities;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using MMS.Application.UseCases.Auth.RefreshToken;
using Shouldly;
using MMS.Exceptions;
using MMS.Exceptions.ExceptionsBase;
using Entity = MMS.Domain.Entities;

namespace UseCases.Test.Auth;

public class RefreshTokenUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();

        string refreshRequest = "RefreshToken";
        
        var token = RefreshTokenBuilder.Build();
        
        var useCase = CreateUseCase(user, token);
        
        async Task act() => await useCase.Execute(refreshRequest, token.Token!);
        
        await act().ShouldNotThrowAsync();
    }
    
    [Fact]
    public async Task Error_Refresh_Token_Invalid()
    {
        var user = UserBuilder.Build();

        string refreshRequest = "RefreshToken";
        
        var token = RefreshTokenBuilder.Build();
        
        var useCase = CreateUseCase(user, token, invalidRequestToken: true);
        
        async Task act() => await useCase.Execute(refreshRequest, "Invalid");
        
        var errors = await act().ShouldThrowAsync<NoPermissionException>();
        errors.Message.ShouldBe(ResourceMessagesException.REFRESH_TOKEN_NOT_FOUND);
    }
    
    private static RefreshTokenUseCase CreateUseCase(
        Entity.User user, Entity.RefreshToken token, bool invalidRequestToken = false
        )
    {
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();

        var refreshTokenGenerator = new RefreshTokenHandlerBuilder()
            .GenerateTokenAndSave(user.UserIdentifier, token.Token)
            .ValidateAccessTokenAndGetData(user.UserIdentifier);
        if (!invalidRequestToken)
            refreshTokenGenerator.GetRefreshToken(token);
        
        return new RefreshTokenUseCase(refreshTokenGenerator.Build(), accessTokenGenerator);
    }
}
