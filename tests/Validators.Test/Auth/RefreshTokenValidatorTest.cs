using CommonTestUtilities.Requests;
using MMS.Application.UseCases.Auth.RefreshToken;
using Shouldly;

namespace Validators.Test.Auth;

public class RefreshTokenValidatorTest
{
    [Fact]
    public void Success()
    {
        var request = RequestRefreshTokenBuilder.Build();

        RefreshTokenValidator validator = new();
        var result = validator.Validate(request);
        
        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_Access_Token_Empty()
    {
        var request = RequestRefreshTokenBuilder.Build();
        request.AccessToken = string.Empty;

        RefreshTokenValidator validator = new();
        var result = validator.Validate(request);
        
        result.IsValid.ShouldBeFalse();
    }
    
    [Fact]
    public void Error_Refresh_Token_Empty()
    {
        var request = RequestRefreshTokenBuilder.Build();
        request.RefreshToken = string.Empty;

        RefreshTokenValidator validator = new();
        var result = validator.Validate(request);
        
        result.IsValid.ShouldBeFalse();
    }
}
